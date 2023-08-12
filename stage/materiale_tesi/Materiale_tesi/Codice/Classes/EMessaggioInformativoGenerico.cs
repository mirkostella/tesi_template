using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace magartubi.Classes
{
    class EMessaggioInformativoGenerico: Exception
    {
        public EMessaggioInformativoGenerico(string mess,string cap): base(mess)
        {
            MessageBoxButtons pulsanti = MessageBoxButtons.OK;
            DialogResult ris;

            // Displays the MessageBox.
            ris = MessageBox.Show(mess, cap, pulsanti);
        }
    }
}
