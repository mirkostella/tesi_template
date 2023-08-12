using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes
{
    class ETestMissioneNonEseguibile : Exception
    {
        public ETestMissioneNonEseguibile(string testo_ecc) : base(testo_ecc) { }
    }
}
