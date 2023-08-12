using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    //classe che contiene i dati di interesse di un articolo
    public class ArtCustom
    {
        public string cod_art { get; set; }
        public string des_art { get; set; }
        public string cod_um { get; set; }
        public int freq_art { get; set; }

        public ArtCustom(string _cod_art = "", string _des_art = "", string _cod_um = "", int _freq_art = 0)
        {
            cod_art = _cod_art;
            des_art = _des_art;
            cod_um = _cod_um;
            freq_art = _freq_art;
        }
        //costruttore di copia
        public ArtCustom(ArtCustom _art)
        {
            cod_art = _art.cod_art;
            des_art = _art.des_art;
            cod_um = _art.cod_um;
            freq_art = _art.freq_art;
        }

        public override string ToString()
        {
            return cod_art + "   "  + des_art + "   " + cod_um + "   " + freq_art.ToString();
        }

    }
}
