using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magartubi.Classes.UtiliMagazzino;

namespace magartubi.Classes.UtiliTest
{
    public abstract class ResocontoTest
    {
        public string nome_test { get; set; }
        public string descrizione { get; set; }

        public ResocontoTest(string _nome_test = "Test generico", string _descrizione = "")
        {
            nome_test = _nome_test;
            descrizione = _descrizione;

        }
    }
    public class ResocontoTestCliente : ResocontoTest
    {
        private List<RisultatoTestCliente> ris_test;
        public ResocontoTestCliente(List<RisultatoTestCliente> _ris_test, string _nome_test, string _descrizione) : base(_nome_test, _descrizione)
        {
            ris_test = _ris_test;
            
        }
        
        public List<RisultatoTestCliente> GetRisultatiTest()
        {
            return new List<RisultatoTestCliente>(ris_test);
        }
    }
    public class ResocontoTestErgon : ResocontoTest
    {
        private List<RisultatoTestErgon> ris_test;
        
        public ResocontoTestErgon(List<RisultatoTestErgon> _ris_test, string _nome_test, string _descrizione) : base(_nome_test, _descrizione)
        {
            ris_test = _ris_test;
        }
        /// <summary>
        /// ritorna di copia la lista dei risultati dei test sulle fermate
        /// </summary>
        /// <returns></returns>
        public List<RisultatoTestFermateErgon> GetRisultatiFermate()
        {
            List<RisultatoTestFermateErgon> r_fermate = new List<RisultatoTestFermateErgon>();
            foreach (RisultatoTestErgon item in ris_test)
            {
                if (item is RisultatoTestFermateErgon)
                    r_fermate.Add(new RisultatoTestFermateErgon(item as RisultatoTestFermateErgon));
            }
            return r_fermate;
        }
        /// <summary>
        /// ritorna di copia la lista dei risultati dei test sulle distanze
        /// </summary>
        /// <returns></returns>
        public List<RisultatoTestDistanzaErgon> GetRisultatiDistanza()
        {
            List<RisultatoTestDistanzaErgon> r_distanze = new List<RisultatoTestDistanzaErgon>();
            foreach (RisultatoTestErgon item in ris_test)
            {
                if (item is RisultatoTestDistanzaErgon)
                    r_distanze.Add(new RisultatoTestDistanzaErgon(item as RisultatoTestDistanzaErgon));
            }
            return r_distanze;
        }
        /// <summary>
        /// cerca tra la lista dei risultati dei test sulle fermate quello che ha il numero di fermate minore. In caso di parita'
        /// di fermate ritorna quello che produce una distanza minore tra gli altri.
        /// </summary>
        /// <returns></returns>
        public virtual RisultatoTestFermateErgon GetMiglioreFermate()
        {
            //ricavo i risultati delle fermate
            List<RisultatoTestFermateErgon> r_fermate = GetRisultatiFermate();
            if (r_fermate.Count == 0)
                return null;
            r_fermate=r_fermate.OrderBy(r => r.fermate_effettuate).ToList();
            //ricavo i risultati con il minor numero di fermate
            List<RisultatoTestFermateErgon> r_fermate_migliori = new List<RisultatoTestFermateErgon>();
            bool finito = false;
            for (int i = 0; i < r_fermate.Count && !finito; i++)
            {
                if (i != 0)
                {
                    if (r_fermate[i].fermate_effettuate == r_fermate[i - 1].fermate_effettuate)
                        r_fermate_migliori.Add(r_fermate[i]);
                    else
                        finito = true;
                }
                else
                    r_fermate_migliori.Add(r_fermate[i]);
            }
            if (r_fermate_migliori.Count == 1)
                return r_fermate_migliori[0];

            //seleziono quello che a parita' di passo mi da la distanza minore
            //ricavo i risultati migliori delle distanze
            List<RisultatoTestDistanzaErgon> r_distanze = GetRisultatiDistanza();
            if (r_distanze.Count == 0)
                return r_fermate[0];

            int valore_migliore_distanza = GetValoreMiglioreDistanza();
            //ricavo i passi dei risultati che hanno la distanza migliore
            List<int> passi_distanze_migliori = new List<int>();
            foreach (RisultatoTestDistanzaErgon item in r_distanze)
            {
                if (item.distanza_percorsa == valore_migliore_distanza)
                    passi_distanze_migliori.Add(item.passo);
            }
            bool trovato = false;
            int indice_migliore_fermata = 0;
            for (int i = 0; i < r_fermate_migliori.Count && !trovato; i++)
            {
                for (int y = 0; y < passi_distanze_migliori.Count && !trovato; y++)
                {
                    if (r_fermate_migliori[i].passo == passi_distanze_migliori[y])
                    {
                        trovato = true;
                        indice_migliore_fermata = i;
                    }
                }
            }
            if (!trovato)
                return r_fermate_migliori[0];

            return r_fermate_migliori[indice_migliore_fermata];
        }
        /// <summary>
        /// ritorna il valore del risultato migliore (numero di fermate minore). Se non sono presenti risultati ritorna -1
        /// </summary>
        /// <returns></returns>
        public int GetValoreMiglioreFermate()
        {
            List<RisultatoTestFermateErgon> r_fermate = GetRisultatiFermate();
            if (r_fermate.Count == 0)
                return -1;
            r_fermate.OrderBy(r => r.fermate_effettuate);
            return r_fermate[0].fermate_effettuate;
        }
        /// <summary>
        /// ritorna il valore del risultato peggiore (numero di fermate maggiore). Se non sono presenti risultati ritorna -1
        /// </summary>
        /// <returns></returns>
        public int GetValorePeggioreFermate()
        {
            List<RisultatoTestFermateErgon> r_fermate = GetRisultatiFermate();
            if (r_fermate.Count == 0)
                return -1;
            r_fermate.OrderByDescending(r => r.fermate_effettuate);
            return r_fermate[0].fermate_effettuate;
        }
        /// <summary>
        /// ritorna il valore del risultato migliore (distanza percorsa minore). Se non sono presenti risultati ritorna -1
        /// </summary>
        /// <returns></returns>
        public int GetValoreMiglioreDistanza()
        {
            List<RisultatoTestDistanzaErgon> r_distanze = GetRisultatiDistanza();
            if (r_distanze.Count == 0)
                return -1;
            r_distanze.OrderBy(r => r.distanza_percorsa);
            return r_distanze[0].distanza_percorsa;
        }
        /// <summary>
        /// ritorna il valore del risultato peggiore (distanza percorsa maggiore). Se non sono presenti risultati ritorna -1
        /// </summary>
        /// <returns></returns>
        public int GetValorePeggioreDistanza()
        {
            List<RisultatoTestDistanzaErgon> r_distanze = GetRisultatiDistanza();
            if (r_distanze.Count == 0)
                return -1;
            r_distanze.OrderByDescending(r => r.distanza_percorsa);
            return r_distanze[0].distanza_percorsa;
        }

        public virtual RisultatoTestDistanzaErgon GetMiglioreDistanza()
        {
            //ricavo i risultati delle distanze
            List<RisultatoTestDistanzaErgon> r_distanze = GetRisultatiDistanza();
            if (r_distanze.Count == 0)
                return null;
            r_distanze=r_distanze.OrderBy(r => r.distanza_percorsa).ToList();
            //ricavo i risultati con la distanza minore
            List<RisultatoTestDistanzaErgon> r_distanza_migliori = new List<RisultatoTestDistanzaErgon>();
            bool finito = false;
            for (int i = 0; i < r_distanze.Count && !finito; i++)
            {
                if (i != 0)
                {
                    if (r_distanze[i].distanza_percorsa == r_distanze[i - 1].distanza_percorsa)
                        r_distanza_migliori.Add(r_distanze[i]);
                    else
                        finito = true;
                }
                else
                    r_distanza_migliori.Add(r_distanze[i]);
            }
            if (r_distanza_migliori.Count == 1)
                return r_distanza_migliori[0];

            //seleziono quello che a parita' di passo mi da la distanza minore
            //ricavo i risultati migliori delle distanze
            List<RisultatoTestFermateErgon> r_fermate = GetRisultatiFermate();
            if (r_fermate.Count == 0)
                return r_distanze[0];

            int valore_migliore_fermate = GetValoreMiglioreFermate();
            //ricavo i passi dei risultati che hanno la distanza migliore
            List<int> passi_fermate_migliori= new List<int>();
            foreach (RisultatoTestFermateErgon item in r_fermate)
            {
                if (item.fermate_effettuate == valore_migliore_fermate)
                    passi_fermate_migliori.Add(item.passo);
            }
            bool trovato = false;
            int indice_migliore_distanza = 0;
            for (int i = 0; i < r_distanza_migliori.Count && !trovato; i++)
            {
                for (int y = 0; y < passi_fermate_migliori.Count && !trovato; y++)
                {
                    if (r_distanza_migliori[i].passo == passi_fermate_migliori[y])
                    {
                        trovato = true;
                        indice_migliore_distanza = i;
                    }
                }
            }
            if (!trovato)
                return r_distanza_migliori[0];

            return r_distanza_migliori[indice_migliore_distanza];
        }
        public virtual RisultatoTestFermateErgon GetPeggioreFermate()
        {
            //ricavo i risultati delle fermate
            List<RisultatoTestFermateErgon> r_fermate = GetRisultatiFermate();
            if (r_fermate.Count == 0)
                return null;
            r_fermate = r_fermate.OrderByDescending(r => r.fermate_effettuate).ToList();
            //ricavo i risultati con il minor numero di fermate
            List<RisultatoTestFermateErgon> r_fermate_peggiori = new List<RisultatoTestFermateErgon>();
            bool finito = false;
            for (int i = 0; i < r_fermate.Count && !finito; i++)
            {
                if (i != 0)
                {
                    if (r_fermate[i].fermate_effettuate == r_fermate[i - 1].fermate_effettuate)
                        r_fermate_peggiori.Add(r_fermate[i]);
                    else
                        finito = true;
                }
                else
                    r_fermate_peggiori.Add(r_fermate[i]);
            }
            if (r_fermate_peggiori.Count == 1)
                return r_fermate_peggiori[0];

            //seleziono quello che a parita' di passo mi da la distanza minore
            //ricavo i risultati migliori delle distanze
            List<RisultatoTestDistanzaErgon> r_distanze = GetRisultatiDistanza();
            if (r_distanze.Count == 0)
                return r_fermate[0];

            int valore_peggiore_distanza = GetValorePeggioreDistanza();
            //ricavo i passi dei risultati che hanno la distanza migliore
            List<int> passi_distanze_peggiori = new List<int>();
            foreach (RisultatoTestDistanzaErgon item in r_distanze)
            {
                if (item.distanza_percorsa == valore_peggiore_distanza)
                    passi_distanze_peggiori.Add(item.passo);
            }
            bool trovato = false;
            int indice_peggiore_fermata = 0;
            for (int i = 0; i < r_fermate_peggiori.Count && !trovato; i++)
            {
                for (int y = 0; y < passi_distanze_peggiori.Count && !trovato; y++)
                {
                    if (r_fermate_peggiori[i].passo == passi_distanze_peggiori[y])
                    {
                        trovato = true;
                        indice_peggiore_fermata = i;
                    }
                }
            }
            if (!trovato)
                return r_fermate_peggiori[0];

            return r_fermate_peggiori[indice_peggiore_fermata];
        }
        public virtual RisultatoTestDistanzaErgon GetPeggioreDistanza()
        {
            //ricavo i risultati delle distanze
            List<RisultatoTestDistanzaErgon> r_distanze = GetRisultatiDistanza();
            if (r_distanze.Count == 0)
                return null;
            r_distanze = r_distanze.OrderByDescending(r => r.distanza_percorsa).ToList();
            //ricavo i risultati con la distanza minore
            List<RisultatoTestDistanzaErgon> r_distanza_peggiori = new List<RisultatoTestDistanzaErgon>();
            bool finito = false;
            for (int i = 0; i < r_distanze.Count && !finito; i++)
            {
                if (i != 0)
                {
                    if (r_distanze[i].distanza_percorsa == r_distanze[i - 1].distanza_percorsa)
                        r_distanza_peggiori.Add(r_distanze[i]);
                    else
                        finito = true;
                }
                else
                    r_distanza_peggiori.Add(r_distanze[i]);
            }
            if (r_distanza_peggiori.Count == 1)
                return r_distanza_peggiori[0];

            //seleziono quello che a parita' di passo mi da la distanza minore
            //ricavo i risultati migliori delle distanze
            List<RisultatoTestFermateErgon> r_fermate = GetRisultatiFermate();
            if (r_fermate.Count == 0)
                return r_distanze[0];

            int valore_peggiori_fermate = GetValoreMiglioreFermate();
            //ricavo i passi dei risultati che hanno la distanza migliore
            List<int> passi_fermate_peggiori = new List<int>();
            foreach (RisultatoTestFermateErgon item in r_fermate)
            {
                if (item.fermate_effettuate == valore_peggiori_fermate)
                    passi_fermate_peggiori.Add(item.passo);
            }
            bool trovato = false;
            int indice_peggiore_distanza = 0;
            for (int i = 0; i < r_distanza_peggiori.Count && !trovato; i++)
            {
                for (int y = 0; y < passi_fermate_peggiori.Count && !trovato; y++)
                {
                    if (r_distanza_peggiori[i].passo == passi_fermate_peggiori[y])
                    {
                        trovato = true;
                        indice_peggiore_distanza = i;
                    }
                }
            }
            if (!trovato)
                return r_distanza_peggiori[0];

            return r_distanza_peggiori[indice_peggiore_distanza];
        }

        public List<RisultatoTestErgon> GetRisultatiTest()
        {
            return new List<RisultatoTestErgon>(ris_test);
        }
    }
    //fine namespace
}
   

    

