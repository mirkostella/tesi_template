using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    class ArtCustomFactory
    {
        private string cod_art;
        private string des_art;
        private string cod_um;
        private int freq_art;

        private ArtCustomFactory(string cod, string des, string um, int freq) {
            cod_art = cod;
            des_art = des;
            freq_art = freq;
            cod_um = um;
        }
        public class ArtCustomBuilder{
            private string cod_art;
            private string des_art;
            private string cod_um;
            private int freq_art;

            public string codart { 
                set { 
                    cod_art = value; 
                } 
                
                get { 
                    return cod_art; 
                } 
            }

            public ArtCustomBuilder() {
                    cod_art = "";
                    des_art = "";
                    freq_art = 0;
                    cod_um = "";
            }
            public ArtCustomBuilder setCodArt(string cod)
            {
                cod_art = cod;
                return this;
            }
            public ArtCustomBuilder setDesArt(string des)
            {
                des_art = des;
                return this;
            }
            public ArtCustomBuilder setFreqArt(int f)
            {
                freq_art = f;
                return this;
            }
            public ArtCustomBuilder setCodUm(string u)
            {
                cod_um = u;
                return this;
            }
            public ArtCustomFactory Build()
            {
                return new ArtCustomFactory(cod_art, des_art, cod_um, freq_art);
            }

        }
        public ArtCustom creaArtCustom()
        {
            ArtCustom art= new ArtCustom();
            
            art.cod_art = cod_art;
            art.des_art = des_art;
            art.freq_art = freq_art;
            art.cod_um = cod_um;

            return art;
        }


    }
}
