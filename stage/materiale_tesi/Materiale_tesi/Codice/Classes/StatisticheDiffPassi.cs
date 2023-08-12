using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    class StatisticheDiffPassi
    {
        public int passo1 { get; set; }
        public int passo2 { get; set; }
        public string descrizione { get; set; }
        public string range { get; set; }
        public int dim_w { get; set; }
        public int diff_arts { get; set; }
        public int diff_pos { get; set; }

        public StatisticheDiffPassi(int _passo1, int _passo2, string _descrizione, string _range, int _dim_w, int _diff_arts, int _diff_pos)
        {
            passo1 = _passo1;
            passo2 = _passo2;
            descrizione = _descrizione;
            range = _range;
            dim_w =_dim_w;
            diff_arts = _diff_arts;
            diff_pos = _diff_pos;
        }

    }
}
