using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using magartubi.Classes;
using magartubi.Classes.UtiliMagazzino;
using Erg;
using Erg.ErgApplication;
using Erg.CSharp;
using Db;
using DevExpress.XtraEditors;

using static Erg.Program;

using System.Data.Entity.Infrastructure;

namespace magartubi.Forms
{
    public partial class Form1 : Form
    {

        private ergdisEntities ergEnt;
        //dizionario con associazioni e frequenze tra prodotti
        private Dictionary<string, Dictionary<string, int>> arts;
        // dizionario con i dettagli dei prodotti in grid1
        private Dictionary<string, ArtCustom> arts_det;
        private List<string> um_selezionate;

        private List<ArtCustom> origine_dati_grid1;
        private List<RecordAssociati> origine_dati_grid2;

        //la chiave corrisponde al passo utilizzato per configurare il magazzino e il valore e' il magazzino configurato
        //dati da passare alla form2 per rappresentare le prove fatte 
        //public momentaneo
        public Dictionary<int, Warehouse> config_magazzini;
        //lista di appoggio per il calcolo delle missioni eseguite su richiesta della form2
        private List<List<TargetMissione>> missioni_eseguite;

        private readonly int passi_test;
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Simulatore Stoccaggio Merci";
            ergEnt = ergdisEntities.Factory(Program.myApp.AppInfo);

            gridView1.BestFitColumns();
            gridView2.BestFitColumns();

            missioni_eseguite = new List<List<TargetMissione>>();
            origine_dati_grid1 = new List<ArtCustom>();
            origine_dati_grid2 = new List<RecordAssociati>();
            arts = new Dictionary<string, Dictionary<string, int>>();
            arts_det = new Dictionary<string, ArtCustom>();
            um_selezionate = new List<string>();
            config_magazzini = new Dictionary<int, Warehouse>();
            recordBindingSource.DataSource = origine_dati_grid1;
            recordAssociatiBindingSource.DataSource = origine_dati_grid2;
            passi_test = 15;
            List<string> vis_um = new List<string> { "FS ", "CA " };
            foreach (var u in vis_um)
            {
                um_selezionate.Add(u);
            }

            AggiornaGrid();

            recordBindingSource.ResetBindings(false);
            recordAssociatiBindingSource.ResetBindings(false);

            //altrimenti non viene generato l'evento click su una cella
            gridView1.OptionsBehavior.Editable = false;
            gridView2.OptionsBehavior.Editable = false;
            //focus sul primo elemento della grid1
            
            
            //fine costruttore
        }
        public int GetPassiTest()
        {
            return passi_test;
        }
        /// <summary>
        /// ritorna tutte le configurazioni del magazzino per riferimento
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Warehouse> GetConfigMagazzini()
        {
            return config_magazzini;
        }
        /// <summary>
        /// ritorna una copia della lista delle missioni eseguite (prima di lanciare il test di interesse quindi con 
        /// le posizioni di prelievo con valori -1)
        /// </summary>
        /// <returns></returns>
        public List<List<TargetMissione>> GetMissioniEseguite()
        {
            return new List<List<TargetMissione>>(missioni_eseguite);
        }
        /// <summary>
        /// aggiorna il dizionario delle associazioni e i dettagli articoli dai dati forniti dalle tabelle magartubiasses e magartubifreqs
        /// e aggiorna le grid selezionando gli articoli con um specificate nel costruttore.
        /// </summary>
        private void AggiornaGrid()
        {
            arts.Clear();
            arts_det.Clear();
            //inizializzo arts e i dettagli leggendo i dati dalle tabelle nuove
            var dati_assoc = ergEnt.magartubiasses;

            var dati_freq = from freq in ergEnt.magartubifreqs
                            join art in ergEnt.anaarts
                            on freq.cod_art equals art.cod_art
                            select new { freq.freq_art, art.cod_art, art.cod_um, art.des_art, freq.magartubiasses };

            int count = 1;
            foreach (var df in dati_freq)
            {
                Console.WriteLine(count);
                count++;
                arts_det.Add(df.cod_art, new ArtCustom(df.cod_art, df.des_art, df.cod_um, df.freq_art));

                Dictionary<string, int> associazioni = new Dictionary<string, int>();
                foreach (var ass in df.magartubiasses)
                {
                    associazioni.Add(ass.cod_art, ass.freq_assoc);
                }
                arts.Add(df.cod_art, associazioni);
            }
            setByUmGrid(um_selezionate);
            origine_dati_grid1 = origine_dati_grid1.OrderByDescending(o => o.freq_art).ToList();
            recordBindingSource.DataSource = origine_dati_grid1;
            recordAssociatiBindingSource.DataSource = origine_dati_grid2;
            recordBindingSource.ResetBindings(false);
            recordAssociatiBindingSource.ResetBindings(false);
            
            Console.WriteLine("refresh termimato");

        }


        private void setByUmGrid(List<string> um_art_grid1)
        {
            origine_dati_grid1.Clear();
            origine_dati_grid2.Clear();
            foreach (var a in arts_det)
            {
                bool trovato = false;
                for (int i = 0; i < um_art_grid1.Count && !trovato; i++)
                {
                    if (a.Value.freq_art != 0 & a.Value.cod_um == um_art_grid1[i])
                    {
                        origine_dati_grid1.Add(new ArtCustom(a.Key, arts_det[a.Key].des_art, arts_det[a.Key].cod_um, arts_det[a.Key].freq_art));
                        trovato = true;
                    }
                }
            }
        }

        //funzione che crea il dizionario con le associazioni di frequenza in base alla lista fornita come parametro 
        private Dictionary<string, Dictionary<string, int>> CreaDizionario(List<OrdArt> ord)
        {
            Dictionary<string, Dictionary<string, int>> ris = new Dictionary<string, Dictionary<string, int>>();
            int anno_ord_prec = -1;
            int nr_ord_prec = -1;

            List<string> arts_curr = new List<string>();

            foreach (var o in ord)
            {

                if (anno_ord_prec != o.anno_ord || nr_ord_prec != o.nr_ord)
                {
                    arts_curr.Clear();
                    anno_ord_prec = o.anno_ord;
                    nr_ord_prec = o.nr_ord;
                }

                if (!ris.ContainsKey(o.cod_art))
                {
                    ris.Add(o.cod_art, new Dictionary<string, int>());
                }

                arts_curr.Add(o.cod_art);

                foreach (string a in arts_curr)
                {

                    if (o.cod_art != a)
                    {
                        if (!ris[a].ContainsKey(o.cod_art))
                        {
                            ris[a].Add(o.cod_art, 1);
                        }
                        else
                        {
                            ris[a][o.cod_art] += 1;
                        }

                        if (!ris[o.cod_art].ContainsKey(a))
                        {
                            ris[o.cod_art].Add(a, 1);
                        }
                        else
                        {
                            ris[o.cod_art][a] += 1;
                        }
                    }
                }
            }

            return ris;
        }

        private void rimuoviPrimoLivelloUm(Dictionary<string, Dictionary<string, int>> diz, List<string> listaUm)
        {
            List<string> listaK = diz.Keys.ToList();
            //ricavo le chiavi che non hanno l'unita' di misura desiderata
            for (int i = 0; i < listaK.Count; i++)
            {
                foreach (string u in listaUm)
                {
                    if (arts_det[listaK[i]].cod_um != u)
                        diz.Remove(listaK[i]);
                }
            }
        }
        //da sistemare
        private void rimuoviSecondoLivelloUm(Dictionary<string, int> diz, List<string> listaUm)
        {
            List<string> listaK = diz.Keys.ToList();
            //ricavo le chiavi che non hanno l'unita' di misura desiderata
            for (int i = 0; i < listaK.Count; i++)
            {
                foreach (string u in listaUm)
                {
                    if (arts_det[listaK[i]].cod_um != u)
                        diz.Remove(listaK[i]);
                }
            }
        }

        //funzione che rimuove dal dizionario riferimento gli articoli che non hanno um contenuta in listaUm
        private void SelezionaUmDizionarioAssociazioni(Dictionary<string, Dictionary<string, int>> dizArts, List<string> listaUm)
        {
            //sistemo il primo livello
            rimuoviPrimoLivelloUm(dizArts, listaUm);
            //sistemo il secondo livello
            List<string> listaK = dizArts.Keys.ToList();
            foreach (string k in listaK)
            {
                rimuoviSecondoLivelloUm(dizArts[k], listaUm);
            }
        }

        private void GridControl1_cell_Click(object sender, RowCellClickEventArgs e)
        {
            origine_dati_grid2.Clear();
            //recupero le info dei prodotti associati e li mostro nella grid2
            ArtCustom r = (ArtCustom)gridView1.GetRow(e.RowHandle);
            var cod_art_selezionato = r.cod_art;

            List<string> chiavi_secondo_livello = arts[r.cod_art].Keys.ToList();

            foreach (var art in arts[cod_art_selezionato])
            {
                string descrizione_articolo = arts_det[art.Key].des_art;
                int freq_articolo = art.Value;
                string codice_articolo = art.Key;
                string unita = arts_det[art.Key].cod_um;
                RecordAssociati rec = new RecordAssociati();
                rec.des_art = descrizione_articolo;
                rec.cod_art = codice_articolo;
                rec.freq = freq_articolo;
                rec.cod_um = unita;
                origine_dati_grid2.Add(rec);
            }
            origine_dati_grid2 = origine_dati_grid2.OrderByDescending(o => o.freq).ToList();
            //true se lo schema dei dati è cambiato; false se solo i valori sono cambiati.
            recordBindingSource.DataSource = origine_dati_grid1;
            recordAssociatiBindingSource.DataSource = origine_dati_grid2;
            recordBindingSource.ResetBindings(false);
            recordAssociatiBindingSource.ResetBindings(false);


        }
        /// <summary>
        /// cerca tra gli articoli presenti in grid1 quelli che hanno um uguale agli um presenti nella lista parametro formale e li ordina per frequenza desc
        /// </summary>
        /// <returns>ritorna la lista di ArtCustom per copia ordinata</returns>
        private List<ArtCustom> SelezionaGrid1ByUm(List<string> ums)
        {
            origine_dati_grid1 = (List<ArtCustom>)origine_dati_grid1.OrderBy(o => !ums.Contains(o.cod_um)).ThenByDescending(o => o.freq_art).ToList();
            List<ArtCustom> copiaOrigineDati = new List<ArtCustom>(origine_dati_grid1);
            copiaOrigineDati = copiaOrigineDati.Where(art => ums.Contains(art.cod_um)).ToList();
            return copiaOrigineDati;
        }

        //le unita' di misura da selezionare saranno passate tramite gli argomenti dell'evento (implementare control per
        //selezione unita' di misura)
        private void ButtonSimulazione_Click(object sender, System.EventArgs e)
        {
            config_magazzini.Clear();
            missioni_eseguite.Clear();


            ///////////////////////////////////////////////////////////////////////////////////////
            int init_passo_f2 = 1;
            Trenino t = new Trenino(6, null);

            var result = from ord in ergEnt.ordclidets
                         join ana in ergEnt.anaarts on ord.cod_art equals ana.cod_art
                         where ord.data_cons.Value.Year == 2022
                            && (ord.data_cons.Value.Month == 7)
                            && ana.cod_um == "FS "
                         orderby ord.data_cons, ord.anno_ord, ord.nr_ord
                         select new
                         {
                             ord.anno_ord,
                             ord.nr_ord,
                             ord.data_cons,
                             ord.cod_art
                         };

            var info_art_cliente = from a in ergEnt.anaarts
                                   join ac in ergEnt.anaartmas
                                   on a.cod_art equals ac.cod_art 
                                   where ac.cod_dep=="010"
                                   orderby ac.corsia,ac.cod_art
                                   select (new { cod_art = a.cod_art, des_art = a.des_art, cod_um = a.cod_um, freq_art = -1 });



            ///////////////////////////////////////////////////////////////////////////////////////


            var lista_result = result.ToList();

            List<string> ords = new List<string>();

            List<TargetMissione> targets_missione = new List<TargetMissione>();

            List<string> ums = new List<string>() { "FS " };
            List<ArtCustom> copiaOrigineDati = SelezionaGrid1ByUm(ums);
            Dictionary<string, Dictionary<string, int>> copia_arts = new Dictionary<string, Dictionary<string, int>>(arts);
            SelezionaUmDizionarioAssociazioni(copia_arts, ums);

            uint n = (uint)info_art_cliente.Count();
            List<ArtCustom> lista_cliente = new List<ArtCustom>();
           
            foreach (var ac in info_art_cliente)
            {
                lista_cliente.Add(new ArtCustom(ac.cod_art, ac.des_art, ac.cod_um, ac.freq_art));
            }

            Warehouse w_cliente = new Warehouse(n, t);
            w_cliente.PopolaMagazzino(lista_cliente);

            config_magazzini.Add(0, w_cliente);

            for (int i = 1; i <= passi_test; i++)
            {
                //la dimensione del magazzino deve essere uguale alla copia origine dati perche' potrei non avere tutti 
                //gli articoli all'interno di arts (articoli senza associati)
                Warehouse w = new Warehouse((uint)copiaOrigineDati.Count, t);
                w.PopolaMagazzino(i, copiaOrigineDati, copia_arts);
                config_magazzini.Add(i, w);
            }

            //ATTENZIONE!! RENDO COMPATIBILI GLI ORDINI CON IL DEPOSITO 010 PERCHE' CI SONO DEGLI ORDINI CHE NON POSSONO ESSERE SODDISFATTI 
            // IN QUANTO CI SONO ARTICOLI NEGLI ORDINI CHE NON SONO PRESENTI NEL DEPOSITO 010


            List<string> lista_cliente_codici = new List<string>();
            List<string> lista_codici_target = new List<string>();
            foreach (var item in lista_result)
            {
                lista_codici_target.Add(item.cod_art);
            }
            foreach (var item in lista_cliente)
            {
                lista_cliente_codici.Add(item.cod_art);

            }

            List<string> da_rimuovere = new List<string>();
            for (int i = 0; i < lista_codici_target.Count; i++)
            {
                bool mancante = true;
                for (int y = 0; y < lista_cliente_codici.Count && mancante; y++)
                {
                    if (lista_cliente_codici[y] == lista_codici_target[i])
                    {
                        mancante = false;
                    }

                }
                if (mancante && !da_rimuovere.Contains(lista_codici_target[i]))
                    da_rimuovere.Add(lista_codici_target[i]);
            }
            foreach (var item in da_rimuovere)
            {
                lista_result.RemoveAll(a=> a.cod_art==item);
            }

            //creo i targets missioni
            for (int i = 0; i < lista_result.Count(); i++)
            {
                bool targets_missione_terminati = false;
                if (i + 1 != lista_result.Count())
                {
                    if (lista_result[i].anno_ord != lista_result[i + 1].anno_ord || lista_result[i].nr_ord != lista_result[i + 1].nr_ord)
                        targets_missione_terminati = true;
                }
                targets_missione.Add(new TargetMissione(lista_result[i].cod_art, lista_result[i].nr_ord, lista_result[i].anno_ord));

                if (!ords.Contains($"{lista_result[i].anno_ord}|{lista_result[i].nr_ord}"))
                {
                    ords.Add($"{lista_result[i].anno_ord}|{lista_result[i].nr_ord}");
                }

                if (ords.Count() == t.n_posti && targets_missione_terminati || (lista_result.Count() - 1 - i) == 0)
                {

                    missioni_eseguite.Add(targets_missione);
                    targets_missione = new List<TargetMissione>();
                    ords.Clear();
                }
            }

            Form2 f2 = new Form2(this,init_passo_f2);
            f2.Show();
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //Console.WriteLine(origine_dati_grid1[gridView1.GetDataSourceRowIndex(e.FocusedRowHandle)]);

        }


        //aggiornamento del dizionarioi e dei dettagli

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if ((int)XtraMessageBox.Show("Premi Ok per continuare o Cancel per annullare", "Aggiorna dati", MessageBoxButtons.OKCancel) == 1)
            {
                DateTime data_inizio = DateTime.Now;
                Console.WriteLine($"inizio aggiornamento {data_inizio}");
                //try
                //{
                myApp.AppGraphics.OpenSplashForm();
                
                //calcolo il dizionario ed aggiorno  le tabelle 
                //trasformazione in OrdArt per passare il parametro a CreaDizionario()
                var ord = ergEnt.ordclidets.Select(o => new { o.anno_ord, o.nr_ord, o.cod_art }).OrderBy(o => o.anno_ord).ThenBy(o => o.nr_ord).Where(o => o.cod_art != null);
                List<OrdArt> ordArtList = new List<OrdArt>();
                foreach (var o in ord)
                {
                    ordArtList.Add(new OrdArt(o.anno_ord, o.nr_ord, o.cod_art));
                }
                Dictionary<string, Dictionary<string, int>> arts_appoggio = new Dictionary<string, Dictionary<string, int>>();
                arts_appoggio = CreaDizionario(ordArtList);
                //@
                //var arts_appoggio_ridotto = arts_appoggio.ToList().GetRange(0, 5);

                //sono tutti gli articoli che sono presenti all'interno di almeno un ordine
                //List<string> chiavi_primo_livello = arts_appoggio.Keys.ToList();

                var det = ergEnt.anaarts.Select(a => new { a.cod_art, a.des_art, a.cod_um });

                Dictionary<string, ArtCustom> arts_det_appoggio = new Dictionary<string, ArtCustom>();
                foreach (var art in det)
                {
                    if (arts_appoggio.ContainsKey(art.cod_art))
                    {
                        ArtCustom artItem = new ArtCustom(art.cod_art, art.des_art, art.cod_um);
                        arts_det_appoggio.Add(art.cod_art, artItem);
                    }
                }
                //frequenza di vendita degli articoli
                var freq =
                              from o in ergEnt.ordclidets
                              join a in ergEnt.anaarts on
                              new { cod_art = o.cod_art } equals
                              new { cod_art = a.cod_art }
                              group o by o.cod_art into g
                              orderby (g.Count()) descending
                              select new { key = g.Key, freq = g.Count() };


                //sistemo le frequenze
                foreach (var fr in freq)
                {
                    if (arts_det_appoggio.ContainsKey(fr.key))
                    {
                        arts_det_appoggio[fr.key].freq_art = fr.freq;
                    }
                }
                //arts_det_appoggio = (Dictionary<string,ArtCustom>)arts_det_appoggio.OrderByDescending(a => a.Value.freq_art);
                Console.WriteLine("");
                //salvo i dati


                Console.WriteLine("inizio operazioni");
                //richiedo i dati delle tabelle 


                //myApp.BeginTrans();

                //per eliminare
                ergEnt.Database.ExecuteSqlCommand("DELETE FROM magartubiass");
                ergEnt.Database.ExecuteSqlCommand("DELETE FROM magartubifreq");
                ergEnt.SaveChanges();

                var dati_freq = ergEnt.magartubifreqs;
                var dati_assoc = ergEnt.magartubiasses;

                int cont = 1;
                //aggiungo i valori freq
                foreach (var df in arts_det_appoggio)
                {
                    Console.WriteLine($"{cont}");
                    cont++;
                    //magartubifreq obj = new magartubifreq();
                    //obj.cod_art = df.Key;
                    //obj.freq_art = df.Value.freq_art;
                    //dati_freq.Add(obj);
                    ergEnt.Database.ExecuteSqlCommand($"INSERT INTO magartubifreq VALUES ('{df.Key}','{df.Value.freq_art}')");
                }
                ergEnt.SaveChanges();
                //aggiungo i valori assoc
                int iCounter = 0;
                //@
                foreach (var da in arts_appoggio)
                {
                    iCounter += 1;
                    Console.WriteLine(iCounter);

                    var assoc = da.Value.OrderByDescending(d => d.Value).ToList().GetRange(0, da.Value.Count < 15 ? da.Value.Count : 15);
                    foreach (var item in assoc)
                    {
                        //magartubiass obj = new magartubiass();
                        //obj.cod_art = item.Key;
                        //obj.cod_art_padre = da.Key;
                        //obj.freq_assoc = item.Value;
                        //dati_assoc.Add(obj);
                        ergEnt.Database.ExecuteSqlCommand($"INSERT INTO magartubiass VALUES ('{item.Key}','{da.Key}',{item.Value})");
                        //Console.WriteLine("aggiunto");
                    }
                }
                Console.WriteLine("prima del salvataggio");
                ergEnt.SaveChanges();
                // myApp.CommitTrans();

                myApp.AppGraphics.CloseSplashForm();

                AggiornaGrid();

                //}
                //catch (Exception)
                //{
                //    myApp.RollbackTrans();
                //    myApp.AppGraphics.CloseSplashForm();
                //    throw;
                //}
                Console.WriteLine("fine salvataggio");
                DateTime data_fine=DateTime.Now;
                Console.WriteLine($"fine aggiornamento {data_fine}");

            }
        }

        




        //fine classe
    }
    //fine namespace
}

