using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes.UtiliMagazzino
{
    public class Trenino
    {
        public int n_posti { get; private set; }

        public int n_posti_occupati { get; private set; }
        public int n_fermate { get; set; }
        public int distanza_percorsa { get; set; }
        private Missione m_principale;
        
        //COSTRUTTORI
        public Trenino()
        {
            n_posti = 1;
            n_posti_occupati = 0;
            n_fermate = 0;
            m_principale = null;
            distanza_percorsa = 0;
        }
        public Trenino(int _n_posti,Missione _m_principale)
        {
            n_posti = _n_posti;
            n_posti_occupati = 0;
            n_fermate = 0;
            m_principale = _m_principale;
            distanza_percorsa = 0;
        }

        //METODI GETTER
      
        public bool MissioneIsSet()
        {
            if (m_principale!=null)
                return true;
            return false;
        }
        /// <summary>
        /// ritorna una copia della missione assegnata al trenino
        /// </summary>
        /// <returns></returns>
        public Missione GetMissione()
        {
            if (MissioneIsSet())
                return new Missione(m_principale);
            return null;
        }

        //restituisce l'indice della prima missione che ha come cod_art il parametro formale fornito altrimenti -1
        public int GetIndiceMissioneArticolo(string cod_articolo)
        {
            bool trovato = false;
            int ris = -1;
            List<TargetMissione> lista_target = m_principale.GetTargets();
            for (int i = 0; i < lista_target.Count && !trovato; i++)
            {
                if (lista_target[i].cod_art == cod_articolo && lista_target[i].pos_prelievo == -1)
                {
                    ris = i;
                    trovato = true;
                }                
            }
            return ris;
        }
        //METODI DI CONTROLLO
        public bool IsPieno()
        {
            return n_posti_occupati==n_posti? true : false;
        }
        
        //METODI CHE OPERANO SUI DATI

        public void SetMissione(Missione _m_principale)
        {
            m_principale = _m_principale;
        }
        //carica l'articolo nella prima posizione libera partendo dalla coda (pre:c'e' almeno una posizione libera)
        //in futuro potrebbe essere utile implementare un pattern strategy per caricare il trenino in modi diversi
        public void Carica(string cod_art,int pos_prelievo)
        {
            if (IsPieno())
                throw new ECapienzaTreninoSuperata();
            else
            {
                //attenzione eccezione EMissioneCodArtNonPresente non gestita
                m_principale.SetPosPrelievoTarget(cod_art, pos_prelievo);
            }
        }
        //svuota il trenino e ripristina le variabili di interesse nel modo opportuno
        public void Svuota()
        {
            n_posti_occupati = 0;
        }
        //imposta n_posti_iccupati,n_fermate e distanza_percorsa a zero lasciando la missione assegnata inalterata
        public void Reset()
        {
            n_posti_occupati = 0;
            n_fermate = 0;
            distanza_percorsa = 0;
        }
      
        //fine classe
    }
    //fine namespace
}
