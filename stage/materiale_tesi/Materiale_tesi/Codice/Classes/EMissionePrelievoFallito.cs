using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes.UtiliMagazzino
{
    public class EMissionePrelievoFallito: Exception
    {
        public EMissionePrelievoFallito(string err_mess): base(err_mess) {}
    }
}
