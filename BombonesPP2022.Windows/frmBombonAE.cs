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
    public partial class frmBombonAE : Form
    {
        public frmBombonAE()
        {
            InitializeComponent();
        }

        private Bombon bombon;
        private TipoChocolateServicios servicioChocolate;
        private TipoNuezServicios servicioNuez;
        private TipoRellenoServicios servicioRelleno;
        private FabricaServicios servicioFabrica;
        private void frmBombonAE_Load(object sender, EventArgs e)
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            servicioChocolate = new TipoChocolateServicios();
            servicioNuez = new TipoNuezServicios();
            servicioRelleno = new TipoRellenoServicios();
            servicioFabrica = new FabricaServicios();
            CargarDatosComboChocolates(ref ChocolateComboBox);
            CargarDatosComboNuez(ref NuezComboBox);
            CargarDatosComboRelleno(ref RellenoComboBox);
            CargarDatosComboFabrica(ref FabricaComboBox);
            if (bombon != null)
            {
                NombreBombonTextBox.Text = bombon.NombreBombon;
                ChocolateComboBox.SelectedValue = bombon.TipoChocolateId;
                NuezComboBox.SelectedValue = bombon.TipoNuezId;
                RellenoComboBox.SelectedValue = bombon.TipoRellenoId;
                FabricaComboBox.SelectedValue = bombon.FabricaId;
                PrecioVentaTextBox.SelectedText = Convert.ToString(bombon.PrecioVenta);
            }
        }
        private void CargarDatosComboChocolates(ref ComboBox combo)
        {
            var lista = servicioChocolate.GetLista();
            var defaultChocolate = new Tipo_Chocolate()
            {
                TipoChocolateId = 0,
                Chocolate = "Seleccione Chocolate"
            };
            lista.Insert(0, defaultChocolate);
            combo.DataSource = lista;
            combo.DisplayMember = "Chocolate";
            combo.ValueMember = "TipoChocolateId";
            combo.SelectedIndex = 0;
        }

        private void CargarDatosComboNuez(ref ComboBox combo)
        {
            var lista = servicioNuez.GetLista();
            var defaultNuez = new Tipo_Nuez()
            {
                TipoNuezId = 0,
                Nuez = "Seleccione Nuez"
            };
            lista.Insert(0, defaultNuez);
            combo.DataSource = lista;
            combo.DisplayMember = "Nuez";
            combo.ValueMember = "TipoNuezId";
            combo.SelectedIndex = 0;
        }

        private void CargarDatosComboRelleno(ref ComboBox combo)
        {
            var lista = servicioRelleno.GetLista();
            var defaultRelleno = new Tipo_Relleno()
            {
                TipoRellenoId = 0,
                Relleno = "Seleccione Relleno"
            };
            lista.Insert(0, defaultRelleno);
            combo.DataSource = lista;
            combo.DisplayMember = "Relleno";
            combo.ValueMember = "TipoRellenoId";
            combo.SelectedIndex = 0;
        }

        private void CargarDatosComboFabrica(ref ComboBox combo)
        {
            var lista = servicioFabrica.GetLista();
            var defaultFabrica = new Fabrica()
            {
                FabricaId = 0,
                NombreFabrica = "Seleccione Fabrica"
            };
            lista.Insert(0, defaultFabrica);
            combo.DataSource = lista;
            combo.DisplayMember = "NombreFabrica";
            combo.ValueMember = "FabricaId";
            combo.SelectedIndex = 0;
        }
        public void SetBombon(Bombon bombon)
        {
            this.bombon = bombon;
        }
        public Bombon GetBombon()
        {
            return bombon;
        }

        private void OKIconButton_Click(object sender, EventArgs e)
        {
                if (bombon == null)
                {
                    bombon = new Bombon();
                }

                bombon.NombreBombon = NombreBombonTextBox.Text;
                bombon.Tipo_Chocolate = (Tipo_Chocolate)ChocolateComboBox.SelectedItem;
                bombon.TipoChocolateId = ((Tipo_Chocolate)ChocolateComboBox.SelectedItem).TipoChocolateId;
                bombon.Tipo_Nuez = (Tipo_Nuez)NuezComboBox.SelectedItem;
                bombon.TipoNuezId = ((Tipo_Nuez)NuezComboBox.SelectedItem).TipoNuezId;
                bombon.Tipo_Relleno = (Tipo_Relleno)RellenoComboBox.SelectedItem;
                bombon.TipoRellenoId = ((Tipo_Relleno)RellenoComboBox.SelectedItem).TipoRellenoId;
                bombon.Fabrica = (Fabrica)FabricaComboBox.SelectedItem;
                bombon.FabricaId = ((Fabrica)FabricaComboBox.SelectedItem).FabricaId;
                bombon.PrecioVenta = Convert.ToDecimal(PrecioVentaTextBox.Text);
                bombon.Stock = 100;
                DialogResult = DialogResult.OK;
        }
    }
}
