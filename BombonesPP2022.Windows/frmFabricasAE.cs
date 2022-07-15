using BombonesPP2022.Entidades;
using BombonesPP2022.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombonesPP2022.Windows
{
    public partial class frmFabricasAE : Form
    {       
        public frmFabricasAE()
        {
            InitializeComponent();
        }
        private Fabrica fabrica;
        private FabricaServicios servicioFabrica;
        private PaisServicios servicioPaises;
        public void SetFabrica(Fabrica fabrica)
        {
            this.fabrica = fabrica;
        }
        public Fabrica GetFabrica()
        {
            return fabrica;
        }

        private void frmFabricasAE_Load(object sender, EventArgs e)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            servicioFabrica = new FabricaServicios();
            servicioPaises = new PaisServicios();
            CargarDatosComboPais(ref PaisesComboBox);
            if (fabrica != null)
            {
                FabricaTextBox.Text = fabrica.NombreFabrica;
                DireccionTextBox.Text = fabrica.Direccion;
                GerenteTextBox.Text = fabrica.GerenteDeVentas;
                PaisesComboBox.SelectedValue = fabrica.PaisId;
            }
        }
        private void CargarDatosComboPais(ref ComboBox combo)
        {
            var lista = servicioPaises.GetLista();
            var defaultPais = new Pais()
            {
                PaisId = 0,
                NombrePais = "Seleccione Pais"
            };
            lista.Insert(0, defaultPais);
            combo.DataSource = lista;
            combo.DisplayMember = "NombrePais";
            combo.ValueMember = "PaisId";
            combo.SelectedIndex = 0;
        }

        private void OKIconButton_Click(object sender, EventArgs e)
        {
            if (fabrica == null)
            {
                fabrica = new Fabrica();
            }

            fabrica.NombreFabrica = FabricaTextBox.Text;
            fabrica.Direccion = DireccionTextBox.Text;
            fabrica.GerenteDeVentas = GerenteTextBox.Text;
            fabrica.PaisId = ((Pais)PaisesComboBox.SelectedItem).PaisId;
            fabrica.Pais = (Pais)PaisesComboBox.SelectedItem;
            DialogResult = DialogResult.OK;
        }

        private void CancelarIconButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
