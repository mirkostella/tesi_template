using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//creata per personalizzare la visualizzazione del trenino fornendo un datasource adeguato
namespace magartubi.Classes.UtiliMagazzino
{
    public class ContenutoCardTrenino
    {
        public int nr_ord { get; set; }
        public int anno_ord { get; set; }
        public string cod_arts { get; set; }

        //costruttori
        public ContenutoCardTrenino()
        {
            nr_ord = -1;
            anno_ord = -1;
            cod_arts = "";
        }
        public ContenutoCardTrenino(int _nr_ord, int _anno_ord)
        {
            nr_ord = _nr_ord;
            anno_ord = _anno_ord;
            cod_arts = "";
        }
    }
}
