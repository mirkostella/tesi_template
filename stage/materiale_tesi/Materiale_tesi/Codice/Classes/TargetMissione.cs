using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes.UtiliMagazzino
{
    //la classe rappresenta gli articoli che vanno a formare la missione di prelievo
    public class TargetMissione
    {
        public int nr_ord { get; private set; }
        public int anno_ord { get; private set; }
        public string cod_art { get; private set; }
        public int pos_prelievo { get; set; }

        public TargetMissione(string _cod_art="",int _nr_ord=-1,int _anno_ord=-1)
        {
            nr_ord = _nr_ord;
            anno_ord = _anno_ord;
            cod_art = _cod_art;
            pos_prelievo = -1;
        }
        public TargetMissione(TargetMissione t)
        {
            nr_ord = t.nr_ord;
            anno_ord = t.anno_ord;
            cod_art = t.cod_art;
            pos_prelievo = t.pos_prelievo;
        }
        public string GetStringaOrdine()
        {
            return $"{cod_art}%{nr_ord}%{anno_ord}";
        }
        public override string ToString()
        {
            return GetStringaOrdine() + "  " + pos_prelievo;
        }
        
    }
}
