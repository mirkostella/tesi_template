using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magartubi.Classes.UtiliMagazzino;


namespace magartubi.Classes
{
    public class Warehouse
    {
        private ArtCustom[] punti_prelievo;
        private Trenino tren;
        //indica la posizione (indice) della coda del trenino
        //attenzione rimettere privato...
        public int posT;

        public Warehouse()
        {
            punti_prelievo = new ArtCustom[1] { null };
            tren = null;
            posT = -1;
        }
        public Warehouse(uint dim, Trenino t)
        {
            punti_prelievo = new ArtCustom[dim];
            for (int i = 0; i < punti_prelievo.Length; i++)
                punti_prelievo[i] = null;
            posT = 0;
            tren = t;
        }
        /// <summary>
        /// ritorna la dimensione del magazzino
        /// </summary>
        /// <returns></returns>
        public int GetDimWarehouse()
        {
            return punti_prelievo.Length;
        }
        /// <summary>
        /// ritorna il trenino per riferimento
        /// </summary>
        /// <returns></returns>
        public Trenino GetTrenino()
        {
            return tren;
        }

        //attenzione che tren deve essere settato con una missione valida prima di chiamare il metodo
        private bool ControlloArticoliMissionePresenti()
        {
            bool art_miss_pres_a_magaz = true;
            List<TargetMissione> tar = tren.GetMissione().targets;
            for (int a = 0; a < tar.Count && art_miss_pres_a_magaz; a++)
            {
                bool trovato = false;
                for (int i = 0; i < punti_prelievo.Length && !trovato; i++)
                {
                    //articolo presente
                    if (punti_prelievo[i] != null && punti_prelievo[i].cod_art == tar[a].cod_art)
                    {
                        trovato = true;
                    }
                }
                
                if (!trovato)
                    art_miss_pres_a_magaz = false;
            }
            return art_miss_pres_a_magaz;
        }
        /// <summary>
        /// calcola il numero di fermate che il trenino ha compiuto per completare la missione 
        /// </summary>
        /// <returns>ritorna il numero di fermate</returns>
        public int TestValutazioneFermate()
        {
            if (tren == null)
                throw new ETestMissioneNonEseguibile("Trenino non presente");
            if (!tren.MissioneIsSet())
                throw new ETestMissioneNonEseguibile("Il trenino non ha una missione da compiere");
            if (!ControlloArticoliMissionePresenti())
                throw new ETestMissioneNonEseguibile("Il magazzino non contiene tutti gli articoli necessari per portare a termine la missione");

            //pre: il trenino puo' portare a termine la missione

            MuoviCodaT(0);
            tren.Svuota();
            tren.n_fermate = 0;
            Console.WriteLine("inizio test valutazione fermate");

            //inizio del test

            while (!tren.GetMissione().MissioneIsCompletata())
            {
                Console.WriteLine("numero di fermate: ");
                Console.WriteLine(tren.n_fermate);
                CaricaCoperturaTren();
                if (tren.IsPieno())
                {
                    //scarico e riparto dall'indindice prossimo articolo da prelevare guardando dalla posizione 0
                    // e incremento il numero di fermate (ricalcolare la pos prossimo articolo da prelevare)
                    tren.Svuota();
                    MuoviCodaT(0);
                }
                int prossimo_articolo_prelievo = GetIndiceProssimoArtInMissioni();
                if (prossimo_articolo_prelievo != -1)
                {
                    MuoviCodaT(prossimo_articolo_prelievo);
                    tren.n_fermate++;
                }

                //controlli sullo stato attuale
                if (prossimo_articolo_prelievo == -1 && !tren.GetMissione().MissioneIsCompletata())
                {
                    //non ho trovato l'articolo in avanti ma la missione non e' completata.. svuoto anche se non
                    //sono pieno, ricalcolo 
                    //la posizione del prossimo articolo e mi sposto alla posizione con incremento del numero di fermate
                    tren.Svuota();
                    MuoviCodaT(0);
                    prossimo_articolo_prelievo = GetIndiceProssimoArtInMissioni();
                    MuoviCodaT(prossimo_articolo_prelievo);
                    tren.n_fermate++;
                }

            }

            return tren.n_fermate;
        }

        //FUNZIONI DI UTILITA
        private void CaricaCoperturaTren()
        {
            List<int> cop = GetCoperturaTren();
            foreach (int pick in cop)
            {
                bool fine = false;
                while (!fine)
                {
                    int indice_missione_tren = tren.GetIndiceMissioneArticolo(punti_prelievo[pick].cod_art);
                    if (indice_missione_tren != -1)
                        //ho trovato la missione che contiene l'articolo al pick
                        tren.Carica(punti_prelievo[pick].cod_art, pick);
                    else
                        fine = true;
                }


            }
        }
        /// <summary>
        /// calcola le posizioni del magazzino che si trovano alla portata del trenino
        /// </summary>
        /// <returns>una lista con gli indici di magazzino alla portata del trenino</returns>
        //va rimesso privato(pubblico solo per fare i test)
        public List<int> GetCoperturaTren()
        {
            List<int> ris = new List<int>();

            int ind_testa = posT + tren.n_posti - 1;
            int ind_ultimo = GetDimWarehouse() - 1;

            ris.Add(posT);
            for (int i = posT + 1; i < ind_ultimo && ris.Count < tren.n_posti && punti_prelievo[i] != null; i++)
                ris.Add(i);
            return ris;
        }
        /// <summary>
        /// posiziona la coda del trenino alla posizione di magazzino fornita dal parametro formale
        /// </summary>
        /// <param name="pos"></param>
        //posiziona la coda del trenino nella posizione indicata dal parametro
        private void MuoviCodaT(int pos)
        {

            if (pos > GetDimWarehouse() - 1 || pos < 0)
                throw new EMagazzinoIndMaxSuperato(pos, GetDimWarehouse());
            else
                posT = pos;
        }

        /// <summary>
        /// ritorna la posizione della coda del trenino 
        /// </summary>
        /// <returns></returns>

        public int GetPosT()
        {
            return posT;
        }

        public override string ToString()
        {
            string ris = "";
            foreach (ArtCustom p in punti_prelievo)
            {
                ris = ris + p.ToString() + "\r\n";
            }
            return ris;
        }
        //da rimettere privato
        public int GetIndiceProssimoArtInMissioni()
        {
            List<int> copertura = GetCoperturaTren();
            int indice_da_esaminare = copertura.Last() + 1;
            int ris = -1;
            bool trovato = false;
            List<TargetMissione> l_targets = tren.GetMissione().GetTargets();
            while (indice_da_esaminare < GetDimWarehouse() && !trovato)
            {
                //scorro la lista delle missioni del treno 
                for (int i = 0; i < l_targets.Count && !trovato; i++)
                {
                    //se trovo il codice articolo di una missione non importa se e' o non e' contenuto nel carrello
                    //controllo su pos_prelievo superflua ma fatta per considerazioni future
                    if (punti_prelievo[indice_da_esaminare] != null &&
                        punti_prelievo[indice_da_esaminare].cod_art == l_targets[i].cod_art &&
                        l_targets[i].pos_prelievo == -1)
                    {
                        trovato = true;
                        ris = indice_da_esaminare;
                    }
                }
                indice_da_esaminare++;
            }
            return ris;
        }
        /// <summary>
        /// ritorna una copia dei punti prelievo
        /// </summary>
        /// <returns></returns>
        public ArtCustom[] GetPuntiPrelievo()
        {
            ArtCustom[] _punti_prelievo = new ArtCustom[punti_prelievo.Length];
            for (int i = 0; i < _punti_prelievo.Length; i++)
            {
                if (punti_prelievo[i] != null)
                    _punti_prelievo[i] = new ArtCustom(punti_prelievo[i]);
                else
                    _punti_prelievo[i] = null;

            }
            return _punti_prelievo;
        }
        //ritorna una copia dei punti prelievo visti dal trenino (se il punto non contiene un ArtCustom insereisce nella posizione corrispondente null)
        public ArtCustom[] GetPuntiPrelievoTrenino()
        {

            ArtCustom[] _punti_prelievo = new ArtCustom[tren.n_posti];
            List<int> cop_tren = GetCoperturaTren();
            foreach (int i in cop_tren)
            {
                if (punti_prelievo[i] == null)
                    _punti_prelievo[i] = null;
                else
                    _punti_prelievo[i] = new ArtCustom(punti_prelievo[i]);

            }
            return _punti_prelievo;
        }
        /// <summary>
        /// inserisce nel punto di prelievo _i l'ArtCustom _art riferimento
        /// </summary>
        /// <param name="_i"></param>
        /// <param name="_art"></param>
        public void SetPuntiPrelievoI(int _i, ArtCustom _art)
        {
            punti_prelievo[_i] = _art;
        }
        /// <summary>
        /// calcola la somma delle distanze percorse dal trenino ad ogni fermata per completare la missione
        /// </summary>
        /// <returns>ritorna la distanza totale percorsa</returns>
        public int TestValutazioneDistanza()
        {
            if (tren == null)
                throw new ETestMissioneNonEseguibile("Trenino non presente");
            if (!tren.MissioneIsSet())
                throw new ETestMissioneNonEseguibile("Il trenino non ha una missione da compiere");
            if (!ControlloArticoliMissionePresenti())
                throw new ETestMissioneNonEseguibile("Il magazzino non contiene tutti gli articoli necessari per portare a termine la missione");

            //pre: il trenino puo' portare a termine la missione

            MuoviCodaT(0);
            tren.Svuota();
            tren.distanza_percorsa = 0;
            Console.WriteLine("inizio test valutazione distanza");

            //inizio del test


            while (!tren.GetMissione().MissioneIsCompletata())
            {
                Console.WriteLine("distanza percorsa: ");
                Console.WriteLine(tren.distanza_percorsa);
                CaricaCoperturaTren();
                if (tren.IsPieno())
                {
                    //scarico e riparto dall'indindice prossimo articolo da prelevare guardando dalla posizione 0
                    // e incremento il numero di fermate (ricalcolare la pos prossimo articolo da prelevare)
                    tren.Svuota();
                    MuoviCodaT(0);
                }
                int prossimo_articolo_prelievo = GetIndiceProssimoArtInMissioni();
                if (prossimo_articolo_prelievo != -1)
                {
                    tren.distanza_percorsa += prossimo_articolo_prelievo - posT;
                    MuoviCodaT(prossimo_articolo_prelievo);
                }

                //controlli sullo stato attuale
                if (prossimo_articolo_prelievo == -1 && !tren.GetMissione().MissioneIsCompletata())
                {
                    //non ho trovato l'articolo in avanti ma la missione non e' completata.. svuoto anche se non
                    //sono pieno, ricalcolo 
                    //la posizione del prossimo articolo e mi sposto alla posizione con incremento del numero di fermate
                    tren.Svuota();
                    MuoviCodaT(0);
                    prossimo_articolo_prelievo = GetIndiceProssimoArtInMissioni();
                    tren.distanza_percorsa += prossimo_articolo_prelievo - posT;
                    MuoviCodaT(prossimo_articolo_prelievo);
                }

            }

            return tren.distanza_percorsa;
        }
        /// <summary>
        /// stringa vuota se non sono presenti associati altrimenti codice_articolo dell'associato che ha frequenza maggiore
        /// </summary>
        /// <param name="cod"></param>
        /// <param name="strut"></param>
        /// <returns></returns>
        private string GetMigliorAssociato(string cod, Dictionary<string, Dictionary<string, int>> strut)
        {
            string cod_best = "";
            if (strut.Count != 0)
            {
                if (strut[cod].Count == 0)
                    return cod_best;

                var chiavi = strut[cod].Keys.ToList();
                cod_best = chiavi.First();
                foreach (var c in chiavi)
                    if (strut[cod][cod_best] < strut[cod][c])
                        cod_best = c;
            }

            return cod_best;
        }
        /// <summary>
        /// setta a null tutte le posizioni di prelievo
        /// </summary>
        public void SvuotaMagazzino()
        {
            for (int i = 0; i < punti_prelievo.Length; i++)
            {
                punti_prelievo[i] = null;
            }
        }
        /// <summary>
        /// popola il magazzino inserendo dalla prima posizione gli ArtCustom che gli vengono passati all'interno della lista parametro formale
        /// </summary>
        /// <param name="w_cliente"></param>
        public void PopolaMagazzino(List<ArtCustom> w_cliente)
        {
            SvuotaMagazzino();
            for (int i = 0; i < GetDimWarehouse() && i < w_cliente.Count; i++)
            {
                punti_prelievo[i] = w_cliente[i];
            }
        }
        /// <summary>
        /// popola il magazzino con gli ArtCustom (ordinati per freq desc) forniti seguendo il passo e il dizionario delle associazioni
        /// (senza ripetizioni di inserimento)
        /// </summary>
        /// <param name="_passo"></param>
        /// <param name="_arts"></param>
        /// <param name="assoc_arts"></param>
        public void PopolaMagazzino(int _passo, List<ArtCustom> _arts, Dictionary<string, Dictionary<string, int>> _assoc_arts)
        {
            SvuotaMagazzino();

            //copia dei parametri
            List<ArtCustom> copia_arts = new List<ArtCustom>(_arts);
            Dictionary<string, Dictionary<string, int>> copia_assoc_arts = new Dictionary<string, Dictionary<string, int>>();
            foreach (var item in _assoc_arts)
            {
                copia_assoc_arts.Add(item.Key,new Dictionary<string, int>());
                foreach (var i in item.Value)
                {
                    copia_assoc_arts[item.Key].Add(i.Key, i.Value);
                }
            }
            List<string> presenti = new List<string>();
            
            //indice di popolamento
            int p = 0;
            //finche' devo inserire articoli e se non ho superato la dimensione del magazzino 
            while (copia_arts.Count != 0 && p < GetDimWarehouse())
            {
                SetPuntiPrelievoI(p, copia_arts[0]);
                p++;
                presenti.Add(copia_arts[0].cod_art);
                string cod_art_corrente = copia_arts[0].cod_art;
                copia_arts.RemoveAt(0);
                bool assoc_non_presente = false;
                //passo
                int i = 0;
                while (i < _passo && !assoc_non_presente && p < GetDimWarehouse() && copia_arts.Count != 0)
                {
                    //trovo l'articolo con maggior frequenza di associazione per l'articolo corrente
                    string best = GetMigliorAssociato(cod_art_corrente, copia_assoc_arts);

                    if (best != "")
                    {
                        //rimuove il miglior associato
                        copia_assoc_arts[cod_art_corrente].Remove(best);
                        //Attenzione: non inserisce un articolo se e' gia' stato inserito in precedenza
                        //se best appartiene alla lista dei presenti lo ignoro altrimenti lo aggiungo al magazzino e alla lista dei presenti
                        if (!presenti.Contains(best))
                        {
                            bool trovato = false;
                            for (int b = 0; b < copia_arts.Count && !trovato; b++)
                            {
                                if (copia_arts[b].cod_art == best)
                                {
                                    trovato = true;
                                    SetPuntiPrelievoI(p, copia_arts[b]);
                                    p++;
                                    i++;
                                    presenti.Add(best);
                                    copia_arts.RemoveAt(b);
                                }
                            }
                        }
                    }
                    else
                        assoc_non_presente = true;
                }

            }
        }

        



        //fine classe
    }
    //fine namespace
}

