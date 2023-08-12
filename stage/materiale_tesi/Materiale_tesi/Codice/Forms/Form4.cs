using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using magartubi.Classes;
using magartubi.Classes.UtiliTest;
using DevExpress.XtraEditors;
using DevExpress.LookAndFeel;

namespace magartubi.Forms
{
    public partial class Form4 : Form
    {
        Form2 vista_principale;

        List<StatisticheDiffPassi> ris_diff_passi;
        int prec_inizio_range;
        int prec_fine_range;
        public Form4(Form2 _vista_principale)
        {
            vista_principale = _vista_principale;
            InitializeComponent();
            this.Text = "Differenze passi";
            inizio_range.EditValue = 1;
            // attenzione: il trenino potrebbe essere piu' grande del magazzino 
            fine_range.EditValue = vista_principale.GetDimTren();
            prec_inizio_range = (int)inizio_range.EditValue;
            prec_fine_range = (int)fine_range.EditValue;
            int w_indice_ultima_posizione = vista_principale.GetIndiceFinePrimaConfigMagazzino();
            Console.WriteLine($"indice ultima posizione di magazzino {w_indice_ultima_posizione}");
            ris_diff_passi = GetStatisticheDiffPassi(vista_principale.GetComparatori((int)inizio_range.EditValue-1, (int)fine_range.EditValue-1));

            statisticheDiffPassiBindingSource.DataSource = ris_diff_passi;
            statisticheDiffPassiBindingSource.ResetBindings(false);

        }

        private void pulsante_calcola_range_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //ricalcola il range e aggiorna la grid
            List<string> msg_err = new List<string>();
            if (int.Parse(inizio_range.EditValue.ToString()) < 1)
                msg_err.Add("-Il valore di inizio range deve essere maggiore di 0\r\n");
            if (int.Parse(fine_range.EditValue.ToString()) > vista_principale.GetIndiceFinePrimaConfigMagazzino() + 1)
                msg_err.Add($"-Il valore di fine range deve essere contenuto all'interno della dimensione del magazzino ({vista_principale.GetIndiceFinePrimaConfigMagazzino() + 1})");
            //non ci sono errori di range
            if (msg_err.Count == 0)
            {
                prec_inizio_range = int.Parse(inizio_range.EditValue.ToString());
                prec_fine_range = int.Parse(fine_range.EditValue.ToString());
                int indice_inizio = int.Parse(inizio_range.EditValue.ToString()) - 1;
                int indice_fine = int.Parse(fine_range.EditValue.ToString()) - 1;
                ris_diff_passi = GetStatisticheDiffPassi(vista_principale.GetComparatori(indice_inizio, indice_fine));
                statisticheDiffPassiBindingSource.DataSource = ris_diff_passi;
                statisticheDiffPassiBindingSource.ResetBindings(false);
            }
            else
            {
                string testo_errore = "";
                foreach (string err in msg_err)
                    testo_errore += err;
                //messagebox con la lista degli errori

                XtraMessageBoxArgs args = new XtraMessageBoxArgs();
                args.Caption = "Range inserito non valido";
                args.Text = string.Format(testo_errore);
                args.Buttons = new DialogResult[] { DialogResult.OK };
                args.LookAndFeel= new UserLookAndFeel(this);
                args.LookAndFeel.UseDefaultLookAndFeel = false;
                args.LookAndFeel.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.Caramel); // set a required skin here
                
                XtraMessageBox.Show(args);
                //ripristino ai valori precedenti
                inizio_range.EditValue = prec_inizio_range;
                fine_range.EditValue = prec_fine_range;
            }

        }
        /// <summary>
        /// estrae la List<StatisticheDiffPassi> da una List<ComparatoeDiffPassiWarehouse>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private List<StatisticheDiffPassi> GetStatisticheDiffPassi(List<ComparatoreDiffPassiWarehouse> l_comparatori)
        {
            List<StatisticheDiffPassi> l_stat = new List<StatisticheDiffPassi>();


            //dati grid passi
            l_comparatori = l_comparatori.OrderBy(c => c.passo_w1).ThenBy(c => c.passo_w2).ToList();
            foreach (ComparatoreDiffPassiWarehouse c in l_comparatori)
            {
                string r = $"da {c.inizio_range + 1} a {c.fine_range + 1}";
                StatisticheDiffPassi s = new StatisticheDiffPassi(c.passo_w1, c.passo_w2, "descriz", r, c.w1.GetDimWarehouse(), c.diff_arts, c.diff_pos);
                l_stat.Add(s);
            }

            return l_stat;
        }
    }
}
