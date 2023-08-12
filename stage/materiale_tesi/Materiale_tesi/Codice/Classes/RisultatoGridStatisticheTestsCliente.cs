using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//classe con lo scopo di visualizzare i risultati per i test cliente
namespace magartubi.Classes
{
    class RisultatoGridStatisticheTestsCliente
    {

        public string nome_cliente { get; set; }
        //010
        public string codice_deposito { get; set; }

        //fermate o distanza
        public string tipo_test { get; set; }

        public int dim_tren { get; set; }
        //breve descrizione del test
        public string descrizione { get; set; }

        //valore + um
        public string risultato { get; set; }
        public RisultatoGridStatisticheTestsCliente(string _nome_cliente,string _codice_deposito, string _tipo_test,string _descrizione,string _risultato, int _dim_tren)
        {
            nome_cliente = _nome_cliente;
            codice_deposito = _codice_deposito;
            tipo_test = _tipo_test;
            descrizione = _descrizione;
            risultato = _risultato;
            dim_tren = _dim_tren;

        }
    }
}
