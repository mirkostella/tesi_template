using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    class OrdArt
    {
        public int anno_ord { get; set; }
        public int nr_ord { get; set; }
        public string cod_art { get; set; }

        public OrdArt() { }
        public OrdArt(int anno, int num, string cod) {
            anno_ord = anno;
            nr_ord = num;
            cod_art = cod;
        }
    }
}
