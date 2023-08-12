using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes.UtiliTest
{
    public abstract class RisultatoTest
    {
        public abstract string GetNomeRisultato();
        public abstract string GetTipoRisultato();
        public abstract string GetRisultato();
        public abstract int GetDimCarrello();
        public abstract string GetDescrizioneRisultato();
    }

    public abstract class RisultatoTestCliente : RisultatoTest
    {
        public int dim_carrello { get; set; }
        public string nome_cliente { get; set; }
        public string cod_deposito { get; set; }
        public RisultatoTestCliente(string _nome_cliente, string _cod_deposito, int _dim_carrello)
        {
            nome_cliente = _nome_cliente;
            cod_deposito = _cod_deposito;
            dim_carrello = _dim_carrello;
        }
        public override string GetNomeRisultato()
        {
            return "Risultato test cliente";
        }
        public override string GetTipoRisultato()
        {
            return "Generico";
        }
        public override string GetRisultato()
        {
            return "Punti";
        }
        public override string GetDescrizioneRisultato()
        {
            return "";
        }

        public string GetNomeCliente()
        {
            return nome_cliente;
        }
        public string GetCodiceDeposito()
        {
            return cod_deposito;
        }
        public override int GetDimCarrello()
        {
            return dim_carrello;
        }

    }


    public abstract class RisultatoTestErgon : RisultatoTest
    {

        public int dim_carrello { get; set; }
        public int passo { get; set; }
        public RisultatoTestErgon(int _passo, int _dim_carrello)
        {
            dim_carrello = _dim_carrello;
            passo = _passo;
        }
        public override string GetNomeRisultato()
        {
            return "Risultato test Ergon con associazioni";
        }
        public override string GetTipoRisultato()
        {
            return "Generico";
        }
        public override string GetRisultato()
        {
            return "Punti";
        }
        public override string GetDescrizioneRisultato()
        {
            return "";
        }
        public override int GetDimCarrello()
        {
            return dim_carrello;
        }
    }

    class RisultatoTestDistanzaCliente : RisultatoTestCliente
    {
        public int distanza_percorsa { get; set; }
        public string um_distanza { get; set; }
        public RisultatoTestDistanzaCliente(string _nome_cliente, string _cod_deposito, int _distanza_percorsa, string _um_distanza, int _dim_carrello) : base(_nome_cliente, _cod_deposito, _dim_carrello)
        {
            distanza_percorsa = _distanza_percorsa;
            um_distanza = _um_distanza;
        }
        public override string GetRisultato()
        {
            return $"{distanza_percorsa} {um_distanza}";
        }

        public override string GetTipoRisultato()
        {
            return "Distanza";
        }
        public override string GetNomeRisultato()
        {
            return "Risultato test distanza cliente";
        }
        public override string GetDescrizioneRisultato()
        {
            return $"Test distanza percorsa per il cliente {nome_cliente} effettuato sul deposito {cod_deposito} utilizzando un carrello avente {GetDimCarrello()} sezioni.\r\n" +
                $"Il test consiste nel calcolare la distanza percorsa dal mezzo per completare la missione che gli è stata assegnata";
        }
        public override string ToString()
        {
            return $"{GetNomeRisultato()}\r\n{GetTipoRisultato()}: {GetRisultato()}";
        }


    }
    class RisultatoTestFermateCliente : RisultatoTestCliente
    {
        public int fermate_effettuate
        {
            get; set;
        }
        public RisultatoTestFermateCliente(string _nome_cliente, string _cod_deposito, int _fermate_effettuate, int _dim_carrello) : base(_nome_cliente, _cod_deposito,_dim_carrello)
        {
            fermate_effettuate = _fermate_effettuate;
        }
        public override string GetRisultato()
        {
            return $"{fermate_effettuate}";
        }

        public override string GetTipoRisultato()
        {
            return "Fermate";
        }
        public override string GetNomeRisultato()
        {
            return "Risultato test fermate cliente";
        }
        public override string ToString()
        {
            return $"{GetNomeRisultato()}\r\n{GetTipoRisultato()}: {GetRisultato()}";
        }
        public override string GetDescrizioneRisultato()
        {
            return $"Test fermate per il cliente {nome_cliente} effettuato sul deposito {cod_deposito} utilizzando un carrello avente {GetDimCarrello()} sezioni.\r\n" +
                $"Il test consiste nel calcolare il numero di fermate impiegate dal mezzo per completare la missione che gli è stata assegnata";
        }
    }
    public class RisultatoTestDistanzaErgon : RisultatoTestErgon
    {

        public int distanza_percorsa { get; set; }
        public string um_distanza { get; set; }
        public RisultatoTestDistanzaErgon(int _passo, int _distanza_percorsa, string _um_distanza,int _dim_carrello) : base(_passo, _dim_carrello)
        {
            distanza_percorsa = _distanza_percorsa;
            um_distanza = _um_distanza;
        }
        //costruttore di copia
        public RisultatoTestDistanzaErgon(RisultatoTestDistanzaErgon r) : base(r.passo,r.dim_carrello)
        {
            distanza_percorsa = r.distanza_percorsa;
            um_distanza = r.um_distanza;
            
        }
        public override string GetRisultato()
        {
            return $"{distanza_percorsa} {um_distanza}";
        }

        public override string GetTipoRisultato()
        {
            return "Distanza";
        }
        public override string GetNomeRisultato()
        {
            return "Risultato test distanza Ergon";
        }

        public override string ToString()
        {
            return $"{GetNomeRisultato()}\r\ntest a passo: {passo} \r\n{GetTipoRisultato()}: {GetRisultato()}";
        }
        public override string GetDescrizioneRisultato()
        {
            return $"Test distanza Ergon effettuato con passo associazioni {passo} utilizzando un carrello avente {GetDimCarrello()} sezioni.\r\n" +
                $"Il test consiste nel calcolare la distanza percorsa dal mezzo per completare la missione che gli è stata assegnata";
        }
    }
    public class RisultatoTestFermateErgon : RisultatoTestErgon
    {
        public int fermate_effettuate
        {
            get; set;
        }
        public RisultatoTestFermateErgon(int _passo, int _fermate_effettuate,int _dim_carrello) : base(_passo, _dim_carrello)
        {
            fermate_effettuate = _fermate_effettuate;
        }
        //costruttore di copia
        public RisultatoTestFermateErgon(RisultatoTestFermateErgon r) : base(r.passo,r.dim_carrello)
        {
            fermate_effettuate = r.fermate_effettuate;
        }
        public override string GetRisultato()
        {
            return $"{fermate_effettuate}";
        }

        public override string GetTipoRisultato()
        {
            return "Fermate";
        }
        public override string GetNomeRisultato()
        {
            return "Risultato test fermate Ergon";
        }
        public override string GetDescrizioneRisultato()
        {
            return $"Test fermate Ergon effettuato con passo associazioni {passo} utilizzando un carrello avente {GetDimCarrello()} sezioni.\r\n" +
                $"Il test consiste nel calcolare il numero di fermate impiegate dal mezzo per completare la missione che gli è stata assegnata";
        }

    }
    public class ComparatoreDiffPassiWarehouse
    {
        public Warehouse w1 { get; set; }
        public Warehouse w2 { get; set; }
        public int passo_w1 { get; set; }
        public int passo_w2 { get; set; }
        public int inizio_range { get; set; }
        public int fine_range { get; set; }

        //contiene il numero di articoli diversi presenti all'interno del range
        public int diff_arts { get; }

        public int diff_pos { get; }
        public ComparatoreDiffPassiWarehouse(Warehouse _w1, Warehouse _w2,int _passo_w1, int _passo_w2,int _inizio_range,int _fine_range)
        {

            w1 = _w1;
            w2 = _w2;
            passo_w1 = _passo_w1;
            passo_w2 = _passo_w2;
            inizio_range = _inizio_range;
            fine_range = _fine_range;
            diff_arts = GetDiffArticoliConfig(w1,w2,inizio_range,fine_range);
            diff_pos = GetDiffPosizioniConfig(w1,w2,inizio_range,fine_range);

        }
        private int GetDiffPosizioniConfig(Warehouse w1, Warehouse w2, int inizio, int fine)
        {
            int n_diff = 0;
            //controllo che i magazzini abbiano la stessa dimensione
            if (w1.GetPuntiPrelievo().Length != w2.GetPuntiPrelievo().Length ||
                inizio < 0 ||
                inizio > fine ||
                fine > w1.GetPuntiPrelievo().Length - 1)
                return -1;
            if (w1 == w2)
                return 0;

            List<string> w1_range = GetCodArtRange(w1, inizio, fine);
            List<string> w2_range = GetCodArtRange(w2, inizio, fine);

            for (int i = 0; i < w1_range.Count; i++)
                if (w1_range[i] != w2_range[i])
                    n_diff++;

            return n_diff;
        }
        /// <summary>
        /// confronta i punti di prelievo all'interno del range fornito di due magazzini aventi la stessa dimensione
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <param name="inizio"></param>
        /// <param name="fine"></param>
        /// <returns>il numero di articoli all'interno del range che non sono presenti nel primo magazzino</returns>
        private int GetDiffArticoliConfig(Warehouse w1, Warehouse w2, int inizio, int fine)
        {
            int n_diff = 0;
            //controllo che i magazzini abbiano la stessa dimensione
            if (w1.GetPuntiPrelievo().Length != w2.GetPuntiPrelievo().Length ||
                inizio < 0 ||
                inizio > fine ||
                fine > w1.GetPuntiPrelievo().Length - 1 ||
                w1.GetPuntiPrelievo().Length == 0)
                return -1;
            if (w1 == w2)
                return 0;

            List<string> w1_range = GetCodArtRange(w1, inizio, fine);
            List<string> w2_range = GetCodArtRange(w2, inizio, fine);

            //trovo le differenze
            foreach (string item in w1_range)
            {
                if (!w2_range.Contains(item))
                    n_diff++;
            }

            return n_diff;
        }
        /// <summary>
        /// preleva i codici articoli dei prodotti all'interno del range.
        /// </summary>
        /// <returns>lista contenente i codici articoli all'interno del range</returns>
        private List<string> GetCodArtRange(Warehouse w, int inizio, int fine)
        {
            ArtCustom[] w_punti_prelievo = w.GetPuntiPrelievo();
            //seleziono i punti compresi all'interno del range (attenzione che i punti potrebbero essere null se non contengono articoli)
            List<string> w_range = new List<string>();
            int indice = 0;
            for (int i = inizio; i <= fine; i++)
            {
                w_range.Add(w_punti_prelievo[i].cod_art);
                indice++;
            }
            return w_range;
        }

    }
}
