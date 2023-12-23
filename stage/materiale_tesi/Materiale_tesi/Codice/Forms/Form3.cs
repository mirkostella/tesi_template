 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using magartubi.Classes.UtiliTest;
using magartubi.Classes;
using DevExpress.XtraGrid.Views.Grid;

namespace magartubi.Forms
{
    public partial class Form3 : Form
    {
        Form2 vista_principale_f4;
        List<RisultatoGridStatisticheTestsCliente> ris_grid_cliente;

        List<RisultatoGridStatisticheTestsFermateErgon> ris_grid_ergon_fermate;

        List<RisultatoGridStatisticheTestsDistanzeErgon> ris_grid_ergon_distanza;

        RisultatoTestFermateErgon ris_fermate_migliore;
        RisultatoTestDistanzaErgon ris_distanza_migliore;
        RisultatoTestFermateErgon ris_fermate_peggiore;
        RisultatoTestDistanzaErgon ris_distanza_peggiore;
        public Form3(ResocontoTestCliente test_cliente, ResocontoTestErgon test_ergon, Form2 _vista_principale_f4)

        {
            InitializeComponent();
            vista_principale_f4 = _vista_principale_f4;
            viewFermate.RowStyle += new RowStyleEventHandler(gridViewFermate_RowStyle);
            viewDistanze.RowStyle += new RowStyleEventHandler(gridViewDistanze_RowStyle);
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Statistiche generali";
            ris_grid_cliente = new List<RisultatoGridStatisticheTestsCliente>();
            ris_grid_ergon_fermate = new List<RisultatoGridStatisticheTestsFermateErgon>();
            ris_grid_ergon_distanza = new List<RisultatoGridStatisticheTestsDistanzeErgon>();

            //dai resoconti ricavare i dati da visualizzare settando i risultati statistiche e aggiungendoli alle liste
            List<RisultatoTestCliente> ris_cliente = test_cliente.GetRisultatiTest();
            List<RisultatoTestErgon> ris_ergon = test_ergon.GetRisultatiTest();

            //dati grid cliente
            foreach (var item in ris_cliente)
                ris_grid_cliente.Add(new RisultatoGridStatisticheTestsCliente(item.nome_cliente, item.cod_deposito, item.GetTipoRisultato(),item.GetDescrizioneRisultato(),  item.GetRisultato(), item.GetDimCarrello()));


            //dati grid ergon
            foreach (var item in ris_ergon)
            {
                if (item is RisultatoTestFermateErgon)
                    ris_grid_ergon_fermate.Add(new RisultatoGridStatisticheTestsFermateErgon(item.GetTipoRisultato(), item.GetDescrizioneRisultato(), item.passo, item.GetRisultato(), item.GetDimCarrello()));
                if (item is RisultatoTestDistanzaErgon)
                    ris_grid_ergon_distanza.Add(new RisultatoGridStatisticheTestsDistanzeErgon(item.GetTipoRisultato(), item.GetDescrizioneRisultato(), item.passo, item.GetRisultato(), item.GetDimCarrello()));
            }

           

            //seleziono i risultati migliori e li coloro di verde e i peggiori di rosso
            ris_fermate_migliore = test_ergon.GetMiglioreFermate();
            ris_distanza_migliore = test_ergon.GetMiglioreDistanza();
            ris_fermate_peggiore = test_ergon.GetPeggioreFermate();
            ris_distanza_peggiore = test_ergon.GetPeggioreDistanza();


            //asssegno i databinding e li aggiorno per visualizzare i dati
            risultatoGridStatisticheTestsClienteBindingSource.DataSource = ris_grid_cliente;
            risultatoGridStatisticheTestsDistanzeErgonBindingSource.DataSource = ris_grid_ergon_distanza;
            risultatoGridStatisticheTestsFermateErgonBindingSource.DataSource = ris_grid_ergon_fermate;


            viewFermate.BestFitColumns();
            viewDistanze.BestFitColumns();
            viewCliente.BestFitColumns();



            risultatoGridStatisticheTestsClienteBindingSource.ResetBindings(false);
            risultatoGridStatisticheTestsDistanzeErgonBindingSource.ResetBindings(false);
            risultatoGridStatisticheTestsFermateErgonBindingSource.ResetBindings(false);
        }
        
        private void gridViewFermate_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string priority = View.GetRowCellDisplayText(e.RowHandle, View.Columns["passo"]);
                //il migliore e' anche il peggiore
                if (priority == ris_fermate_migliore.passo.ToString())
                    e.Appearance.BackColor = Color.FromArgb(150, Color.GreenYellow);
                if (priority == ris_fermate_peggiore.passo.ToString())
                    e.Appearance.BackColor = Color.FromArgb(150, Color.IndianRed);
                if(ris_fermate_migliore.passo.ToString() == ris_fermate_peggiore.passo.ToString())
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Aquamarine);


            }
        }
        private void gridViewDistanze_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string priority = View.GetRowCellDisplayText(e.RowHandle, View.Columns["passo"]);
                if (priority == ris_distanza_migliore.passo.ToString())
                    e.Appearance.BackColor = Color.FromArgb(150, Color.GreenYellow);
                if (priority == ris_distanza_peggiore.passo.ToString())
                    e.Appearance.BackColor = Color.FromArgb(150, Color.IndianRed);
                if (ris_distanza_migliore.passo.ToString() == ris_distanza_peggiore.passo.ToString())
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Aquamarine);
            }
        }

        private void labelControl4_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Form4 f4=new Form4(vista_principale_f4);
            f4.Show();
        }
    }
}

