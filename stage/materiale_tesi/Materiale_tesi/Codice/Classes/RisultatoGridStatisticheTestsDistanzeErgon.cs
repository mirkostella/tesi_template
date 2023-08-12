using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    class RisultatoGridStatisticheTestsDistanzeErgon
    {


        //distanze
        public string tipo_test { get; set; }

        //breve descrizione del test
        public string descrizione { get; set; }
        public int dim_tren { get; set; }

        public int passo { get; set; }

        //valore + um
        public string risultato { get; set; }
        public RisultatoGridStatisticheTestsDistanzeErgon(string _tipo_test,string _descrizione,int _passo,string _risultato, int _dim_tren)
        {
            tipo_test = _tipo_test;
            descrizione = _descrizione;
            passo = _passo;
            risultato = _risultato;
            dim_tren = _dim_tren;

        }
    }
}
