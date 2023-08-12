using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    class EMagazzinoIndMaxSuperato : Exception
    {
        public EMagazzinoIndMaxSuperato(int pos, int dimMagaz) : base("Indice massimo superato: La coda del trenino si trova in posizione " + pos +
            " e l'ultima posizione del magazzino e' " + (dimMagaz-1)) {}
    }
}
