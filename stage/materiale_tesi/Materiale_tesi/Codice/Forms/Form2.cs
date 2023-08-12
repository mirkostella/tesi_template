using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using magartubi.Classes;
using magartubi.Classes.UtiliMagazzino;
using System.Data.Entity;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;
using magartubi.Forms;
using magartubi.Classes.UtiliTest;
using DevExpress.XtraGrid.Columns;

//vista che permette di visualizzare la simulazione di una missione
namespace magartubi.Forms
{
    public partial class Form2 : Form
    {

        private ArtCustom[] punti_prelievo;
        private List<ContenutoCardTrenino> carico_t;
        private List<List<TargetMissione>> missioni_eseguite;
        private List<List<int>> gruppi_missione_in_corso;
        private int step_missione_in_corso;
        private int numero_missione_in_corso;
        private List<BindingSource> l_binding_missioni_grids;
        private List<List<ContenutoCardTrenino>> contenuti_cards_trenini;
        private int dim_t;
        private int fermate_attuali;
        private int distanza_attuale;
        private Form1 vista_principale;
        //per avere il valore precedente quando l'utente seleziona un passo nuovo dalla combo
        private int passo_attuale;
        private string magaz_sel;


        public Form2(Form1 _vista_principale, int _passo)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Simulazione missione di prelievo";
            this.Resize += new System.EventHandler(this.RidimensionaForm);
           
            if (_passo > 0)
                this.ComboSelMagazz.EditValue = "Ergon";
            else
            {
                this.ComboSelMagazz.EditValue = "Cliente";
                comboBoxPasso.Enabled = false;
            }
            magaz_sel = this.ComboSelMagazz.EditValue.ToString();


            this.ComboSelMagazz.EditValueChanged += new System.EventHandler(this.ComboBoxSelMagazz_Value_Changed);

            //i punti prelievo vengono richiesti alla vista principale che e' smart e riorganizza il magazzino
            punti_prelievo = _vista_principale.GetConfigMagazzini()[_passo].GetPuntiPrelievo();
            missioni_eseguite = _vista_principale.GetMissioniEseguite();
            carico_t = new List<ContenutoCardTrenino>();
            dim_t = _vista_principale.GetConfigMagazzini()[_passo].GetTrenino().n_posti;
            l_binding_missioni_grids = new List<BindingSource>();
            contenuti_cards_trenini = new List<List<ContenutoCardTrenino>>();
            fermate_attuali = 0;
            distanza_attuale = 0;
            passo_attuale = _passo;

            comboBoxPasso.EditValue = _passo;
            //dopo assegnazione per non generare l'evento
            this.comboBoxPasso.EditValueChanged += new System.EventHandler(this.ComboBoxPasso_Value_Changed);
            vista_principale = _vista_principale;

            //card magazzino
            cardView1.MaximumCardRows = 1;
            cardView1.OptionsView.ShowQuickCustomizeButton = false;

            Warehouse w_passo_attuale = vista_principale.GetConfigMagazzini()[passo_attuale];
            List<TargetMissione> nuovi_target_missione = missioni_eseguite[passo_attuale];
            Missione m = new Missione(nuovi_target_missione);
            w_passo_attuale.GetTrenino().SetMissione(m);
            //eseguo un test (per settare le pos di prelievo)
            w_passo_attuale.TestValutazioneFermate();

            creaMissione(0);

            artCustomBindingSource.DataSource = punti_prelievo;
            artCustomBindingSource.ResetBindings(false);

            creaCardsTrenino(cardView2, carico_t);
            contenutoCardTreninoBindingSource.DataSource = carico_t;
            contenutoCardTreninoBindingSource.ResetBindings(false);



        }

        /// <summary>
        /// imposta la prossima missione da eseguire per il test e crea i gruppi missione in corso 
        /// </summary>
        /// <param name="num_missione"></param>
        private void creaMissione(int num_missione)
        {

            numero_missione_in_corso = num_missione;
            step_missione_in_corso = 0;
            label_passo_in_corso.Text = "Step:  " + $"{step_missione_in_corso}";
            label_missione_in_corso.Text = "Missione in corso:  " + $"{numero_missione_in_corso+1}";
            List<TargetMissione> tar = new List<TargetMissione>(missioni_eseguite[num_missione]);
            Missione mis = new Missione(tar);
            Warehouse w = vista_principale.GetConfigMagazzini()[int.Parse(comboBoxPasso.EditValue.ToString())];
            w.GetTrenino().SetMissione(mis);
            fermate_attuali += w.TestValutazioneFermate();
            mis.Ripristina();
            distanza_attuale += w.TestValutazioneDistanza();
            gruppi_missione_in_corso = CreaGruppiMissione(tar);
        }
        private void creaCardsTrenino(CardView cv, List<ContenutoCardTrenino> carico)
        {
            cv.MaximumCardRows = 1;
            cv.OptionsView.ShowQuickCustomizeButton = false;
            cv.CardCaptionFormat = "Sezione carrello {0}";
            cv.CardWidth = 270;
            cv.CardInterval = 4;
            
            for (int i = 0; i < dim_t; i++)
            {
                carico.Add(new ContenutoCardTrenino());
            }
        }
        /// <summary>
        /// data una lista di target missione ritorna la lista di indici di prelievo relativi ad ogni step missione
        /// </summary>
        /// <param name="l_t"></param>
        /// <returns></returns>
        private List<List<int>> CreaGruppiMissione(List<TargetMissione> l_t)
        {
            l_t=l_t.OrderBy(m => m.pos_prelievo).ToList();
            List<List<int>> ris = new List<List<int>>();
            bool primo = true;
            int indice_coda_tren = 0;
            for (int i = 0; i < l_t.Count();)
            {
                if (primo)
                {
                    indice_coda_tren = 0;
                    primo = false;
                }
                else
                    indice_coda_tren = l_t[i].pos_prelievo;

                List<int> gruppo = new List<int>();
                bool gruppo_completo = false;
                //crea gruppo
                while (i < l_t.Count() && !gruppo_completo)
                {
                    if (l_t[i].pos_prelievo - indice_coda_tren < dim_t)
                    {
                        if (!gruppo.Contains(l_t[i].pos_prelievo))
                        {
                            gruppo.Add(l_t[i].pos_prelievo);
                        }
                        i++;
                    }
                    else
                        gruppo_completo = true;
                }
                ris.Add(gruppo);
            }
            return ris;
        }

        private void AvanzaPulsante_Click(object sender, EventArgs e)
        {
            label_passo_in_corso.Text = "Step:  " + $"{step_missione_in_corso + 1}"; 
            List<int> articoli_da_prelevare = gruppi_missione_in_corso[step_missione_in_corso];
            List<TargetMissione> tar_missione_corrente = missioni_eseguite[numero_missione_in_corso];
            tar_missione_corrente = tar_missione_corrente.OrderBy(t => t.pos_prelievo).ToList();

            foreach (int art_prelievo in articoli_da_prelevare)
            {
                bool trovato = false;
                for (int i = 0; i < tar_missione_corrente.Count && !trovato; i++)
                {
                    if (tar_missione_corrente[i].pos_prelievo == art_prelievo)
                    {
                        InserisciInCarico(tar_missione_corrente[i]);
                        trovato = true;
                    }
                    if (i + 1 != tar_missione_corrente.Count && tar_missione_corrente[i + 1].pos_prelievo == art_prelievo)
                        trovato = false;

                }  
            }
            if (numero_missione_in_corso == 0)
                contenutoCardTreninoBindingSource.ResetBindings(false);
            else
                l_binding_missioni_grids[numero_missione_in_corso - 1].ResetBindings(false);

            //dopo avanzamento 
            //controllo lo stato delle missioni
            bool missioni_terminate = false;
            //ho finito tutti gli step della missione in corso
            if (step_missione_in_corso == gruppi_missione_in_corso.Count - 1)
            {
                numero_missione_in_corso++;
                step_missione_in_corso = 0;
                if (numero_missione_in_corso == missioni_eseguite.Count)
                    missioni_terminate = true;


                //ho terminato la missione e devo comunicarlo all'utente 
                XtraMessageBox.Show("Premere OK per proseguire", "Missione terminata", MessageBoxButtons.OK);
                if (missioni_terminate == true)
                {
                    XtraMessageBox.Show("Tutte le missioni sono state completate", "Missioni terminate", MessageBoxButtons.OK);
                    PulsanteAvanzaMissione.Enabled = false;
                }
                else
                {
                    //preparo la prossima missione
                    System.IO.Stream stream;
                    stream = new System.IO.MemoryStream();
                    cardView2.SaveLayoutToStream(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    GridControl nuova_grid = new GridControl();
                    CardView nuova_card_view = new CardView();
                    nuova_card_view.GridControl = nuova_grid;

                    nuova_card_view.RestoreLayoutFromStream(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    nuova_grid.MainView = nuova_card_view;
                    List<ContenutoCardTrenino> nuovo_contenuto_trenino = new List<ContenutoCardTrenino>();
                    //contiene i carichi dalla seconda missione 
                    contenuti_cards_trenini.Add(nuovo_contenuto_trenino);
                    creaCardsTrenino(nuova_card_view, nuovo_contenuto_trenino);

                    BindingSource binding_source_nuova_view = new BindingSource();
                    binding_source_nuova_view.DataSource = nuovo_contenuto_trenino;
                    nuova_grid.DataSource = binding_source_nuova_view;
                    l_binding_missioni_grids.Add(binding_source_nuova_view);
                    LayoutControlItem item_nuova_grid = new LayoutControlItem();
                    item_nuova_grid.Control = nuova_grid;

                    tablePanel1.SuspendLayout();

                    tablePanel1.Controls.Add(nuova_grid);
                    TablePanelRow row1 = new TablePanelRow(TablePanelEntityStyle.AutoSize, 1);
                    TablePanelRow row2 = new TablePanelRow(TablePanelEntityStyle.AutoSize, 1);
                    tablePanel1.Rows.Add(row1);
                    tablePanel1.Rows.Add(row2);

                    tablePanel1.SetCell(layoutPulsantiBottom, tablePanel1.Rows.Count() - 1, 0);
                    tablePanel1.SetCell(nuova_grid, tablePanel1.Rows.Count() - 2, 0);

                    tablePanel1.Rows.RemoveAt(tablePanel1.Rows.Count() - 3);

                    tablePanel1.ResumeLayout();

                    tablePanel1.Height += nuova_grid.Height;
                    nuova_grid.Size = gridControl2.Size;

                    //gestisco la nuova missione
                    nuova_card_view.Columns.Add(new GridColumn()
                    {
                        Caption = "Numero ordine",
                        FieldName = "nr_ord",
                        Visible = true
                    });
                    nuova_card_view.Columns.Add(new GridColumn()
                    {
                        Caption = "Anno ordine",
                        FieldName = "anno_ord",
                        Visible = true
                    });
                    nuova_card_view.Columns.Add(new GridColumn()
                    {
                        Caption = "Articoli prelevati",
                        FieldName = "cod_arts",
                        Visible = true
                    });
                    binding_source_nuova_view.ResetBindings(false);
                    
                    creaMissione(numero_missione_in_corso);
                }
            }
            else
                step_missione_in_corso++;
        }
        private void InserisciInCarico(TargetMissione tar_da_inserire)
        {
            List<ContenutoCardTrenino> alias_contenuto;

            //sono alla prima missione
            if (numero_missione_in_corso == 0)
            {
                alias_contenuto = carico_t;
            }
            else
            {
                //se sono alla missione 1 il primo contenuto dalle missioni successive alla prima e' lo zero
                alias_contenuto = contenuti_cards_trenini[numero_missione_in_corso - 1];
            }
            bool trovato = false;
            for (int i = 0; i < dim_t && !trovato; i++)
            {

                if (alias_contenuto[i].nr_ord == tar_da_inserire.nr_ord && alias_contenuto[i].anno_ord == tar_da_inserire.anno_ord)
                {
                    //aggiungo solo l'articolo alla lista articoli della scheda
                    alias_contenuto[i].cod_arts += $" {tar_da_inserire.cod_art}";
                    trovato = true;
                }
                if (alias_contenuto[i].nr_ord == -1)
                {
                    //aggiorno anche anno e nr_ord della scheda
                    alias_contenuto[i].nr_ord = tar_da_inserire.nr_ord;
                    alias_contenuto[i].anno_ord = tar_da_inserire.anno_ord;
                    alias_contenuto[i].cod_arts = tar_da_inserire.cod_art;
                    trovato = true;
                }
            }

        }


        private void ComboBoxPasso_Value_Changed(object sender, EventArgs e)
        {
            if (int.Parse(comboBoxPasso.EditValue.ToString()) != passo_attuale)
            {

                //messaggio di conferma
                DialogResult decisione = XtraMessageBox.Show($"Cambiare la configurazione del magazzino da passo {passo_attuale} al passo {comboBoxPasso.EditValue}?" +
                    $"\r\n Attenzione: l'avanzamento della missione in corso andrà perduto", "Cambio passo", MessageBoxButtons.OKCancel);
                //l'utente ha premuto ok
                if (((int)decisione) == 1)
                {
                    AggiornaPassoMagazz(int.Parse(comboBoxPasso.EditValue.ToString()));
                }
                else
                {
                    comboBoxPasso.EditValue = passo_attuale;
                }

            }
        }

        private void AggiornaPassoMagazz(int _passo)
        {
            passo_attuale = _passo;
            fermate_attuali = 0;
            distanza_attuale = 0;
            punti_prelievo = vista_principale.GetConfigMagazzini()[passo_attuale].GetPuntiPrelievo();
            //eseguo il test per la missione di interesse (altrimenti pos di prelievo a -1 per ogni target)
            Warehouse w_passo_attuale = vista_principale.GetConfigMagazzini()[passo_attuale];
            List<TargetMissione> nuovi_target_missione= missioni_eseguite[passo_attuale];
            Missione m = new Missione(nuovi_target_missione);
            w_passo_attuale.GetTrenino().SetMissione(m);
            //eseguo un test (per settare le pos di prelievo)
            w_passo_attuale.TestValutazioneFermate();
            dim_t = w_passo_attuale.GetTrenino().n_posti;
            // aggiorno card magazzino
            artCustomBindingSource.DataSource = punti_prelievo;
            artCustomBindingSource.ResetBindings(false);

            //aggiorno card trenino
            carico_t.Clear();
            creaCardsTrenino(cardView2, carico_t);
            contenutoCardTreninoBindingSource.DataSource = carico_t;
            contenutoCardTreninoBindingSource.ResetBindings(false);

            contenuti_cards_trenini.Clear();
            foreach (var bind in l_binding_missioni_grids)
            {
                //bind.DataSource = new List<ContenutoCardTrenino>();
                bind.DataSource = null;

                bind.ResetBindings(true);
            }
            
            l_binding_missioni_grids.Clear();

            //pulisco la form2
            tablePanel1.Rows.Clear();
            TablePanelRow row_mag = new TablePanelRow(TablePanelEntityStyle.AutoSize, 1);
            TablePanelRow row_tren = new TablePanelRow(TablePanelEntityStyle.AutoSize, 1);
            TablePanelRow row_butt = new TablePanelRow(TablePanelEntityStyle.AutoSize, 1);


            tablePanel1.Rows.Add(row_mag);
            tablePanel1.Rows.Add(row_tren);
            tablePanel1.Rows.Add(row_butt);
            tablePanel1.SetRow(gridControl1, 0);
            tablePanel1.SetRow(gridControl2, 1);
            tablePanel1.SetRow(layoutPulsantiBottom, 2);

            PulsanteAvanzaMissione.Enabled = true;
            creaMissione(0);

        }

        private void RidimensionaForm(object sender, EventArgs e)
        {
            tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            PulsanteStatistiche.Dock = System.Windows.Forms.DockStyle.Right;
        }
        private void ComboBoxSelMagazz_Value_Changed(object sender, EventArgs e)
        {
            if (ComboSelMagazz.EditValue.ToString() == "Ergon")
            {
                this.comboBoxPasso.Enabled = true;
                AggiornaPassoMagazz(int.Parse(comboBoxPasso.EditValue.ToString()));
            }
            else
            {
                comboBoxPasso.Enabled = false;
                AggiornaPassoMagazz(0);
            }
        }
        private void PulsanteStatistiche_Click(object sender, EventArgs e)
        {
            Erg.Program.myApp.AppGraphics.OpenSplashForm();
            
            //test magazzino cliente
            List<RisultatoTestCliente> ris_tests_cliente = new List<RisultatoTestCliente>();
            Warehouse w_cliente = vista_principale.GetConfigMagazzini()[0];
            //test distanza cliente
            int tot_distanza_cliente = 0;
            foreach (var list_targets in missioni_eseguite)
            {
                Missione m = new Missione(list_targets);
                w_cliente.GetTrenino().SetMissione(m);
                w_cliente.GetTrenino().Reset();

                tot_distanza_cliente += w_cliente.TestValutazioneDistanza();
            }
            RisultatoTestDistanzaCliente ris_test_distanza_cliente = new RisultatoTestDistanzaCliente("EuroBevande", "010", tot_distanza_cliente, "ubicazioni",dim_t);
            ris_tests_cliente.Add(ris_test_distanza_cliente);
            //ripristino le posizioni di prelievo
            foreach (var list_targets in missioni_eseguite)
            {
                foreach (var t in list_targets)
                {
                    t.pos_prelievo = -1;
                }
            }

            //test fermate cliente
            int tot_fermate_cliente = 0;
            foreach (var list_targets in missioni_eseguite)
            {
                Missione m = new Missione(list_targets);
                w_cliente.GetTrenino().SetMissione(m);
                tot_fermate_cliente += w_cliente.TestValutazioneFermate();
            }
            RisultatoTestFermateCliente ris_test_fermate_cliente = new RisultatoTestFermateCliente("EuroBevande", "010", tot_fermate_cliente, dim_t);
            ris_tests_cliente.Add(ris_test_fermate_cliente);
            //ripristino le posizioni di prelievo
            foreach (var list_targets in missioni_eseguite)
            {
                foreach (var t in list_targets)
                {
                    t.pos_prelievo = -1;
                }
            }
            ResocontoTestCliente resoconto = new ResocontoTestCliente(ris_tests_cliente, "Tests Cliente", "Tests fatti simulando il comportamento del trenino sulla configurazione del cliente");

            //tests Ergon

            List<RisultatoTestErgon> ris_tests_ergon = new List<RisultatoTestErgon>();
            int n_passi = vista_principale.GetPassiTest();
            //per ogni passo faccio tutti i test possibili (i>=1 perche' all'indice 0 delle configurazioni magazzino c'e' quella del cliente)
            for (int i = 1; i <= n_passi; i++)
            {
                Warehouse w_ergon = vista_principale.GetConfigMagazzini()[i];
                int tot_distanza = 0;
                foreach (var list_targets in missioni_eseguite)
                {
                    Missione m = new Missione(list_targets);
                    w_cliente.GetTrenino().SetMissione(m);
                    tot_distanza += w_ergon.TestValutazioneDistanza();
                }
                RisultatoTestErgon ris_test_distanza = new RisultatoTestDistanzaErgon(i, tot_distanza, "ubicazioni", dim_t);
                ris_tests_ergon.Add(ris_test_distanza);
                //ripristino le posizioni di prelievo
                foreach (var list_targets in missioni_eseguite)
                {
                    foreach (var t in list_targets)
                    {
                        t.pos_prelievo = -1;
                    }
                }

                int tot_fermate = 0;
                foreach (var list_targets in missioni_eseguite)
                {
                    Missione m = new Missione(list_targets);
                    w_ergon.GetTrenino().SetMissione(m);
                    tot_fermate += w_ergon.TestValutazioneFermate();
                }
                RisultatoTestErgon ris_test_fermate = new RisultatoTestFermateErgon(i, tot_fermate, dim_t);
                ris_tests_ergon.Add(ris_test_fermate);
                //ripristino le posizioni di prelievo
                foreach (var list_targets in missioni_eseguite)
                {
                    foreach (var t in list_targets)
                    {
                        t.pos_prelievo = -1;
                    }
                }
            }
            ResocontoTestErgon resoconto_ergon = new ResocontoTestErgon(ris_tests_ergon, "Tests Ergon", "Tests fatti simulando il comportamento del trenino su magazzino Ergon con associazioni");

            //stampe
            Console.WriteLine(resoconto);
            Console.WriteLine(resoconto_ergon);
            
            Erg.Program.myApp.AppGraphics.CloseSplashForm();

            Form3 f3 = new Form3(resoconto, resoconto_ergon,this);
            f3.Show();

        }
        /// <summary>
        /// confronta i passi dei magazzini nel range fornito come parametro formale
        /// </summary>
        /// <returns>ritorna la lista dei comparatori. Se non ci sono confronti da fare ritorna la lista vuota</returns>
        public List<ComparatoreDiffPassiWarehouse> GetComparatori(int w_ind_inizio_confronto,int w_ind_fine_confronto)
        {
            List<ComparatoreDiffPassiWarehouse> l_comp = new List<ComparatoreDiffPassiWarehouse>();
            
            //confronto configurazioni passi magazzini
            //se non ci sono confronti da fare
            if (vista_principale.GetPassiTest() > 1)
            {
                //confronto la prima config con le altre
                for (int i = 1; i <= vista_principale.GetPassiTest() - 1; i++)
                {
                    //configurazioni di confronto
                    for (int y = i + 1; y <= vista_principale.GetPassiTest(); y++)
                    {
                        int w_ind_fine = vista_principale.GetConfigMagazzini()[i].GetPuntiPrelievo().Length - 1;
                        
                        ComparatoreDiffPassiWarehouse comparatore = new ComparatoreDiffPassiWarehouse(vista_principale.GetConfigMagazzini()[i], vista_principale.GetConfigMagazzini()[y],
                                                                        i, y, w_ind_inizio_confronto, w_ind_fine_confronto);
                        l_comp.Add(comparatore);
                    }

                }
            }
            else
                Console.WriteLine("Non ci sono configurazioni di magazzino da confrontare");

            return l_comp;
        }
        /// <summary>
        /// ritorna l'indice dell'ultima posizione dei magazzini (i magazzini hanno la stessa dimensione perche' e' lo stesso a passi differenti)
        /// </summary>
        /// <returns></returns>
        public int GetIndiceFinePrimaConfigMagazzino()
        {
            return vista_principale.GetConfigMagazzini()[1].GetPuntiPrelievo().Length - 1;
        }
        /// <summary>
        /// ritorna la dimensione del trenino a cui le simulazioni fanno riferimento
        /// </summary>
        /// <returns></returns>
        public int GetDimTren()
        {
            return dim_t;
        }













        //fine classe
    }
    //fine namespace 
}
