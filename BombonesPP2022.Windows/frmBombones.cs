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
    public partial class frmBombones : Form
    {
        public frmBombones()
        {
            InitializeComponent();
        }

        private BombonesServicios servicio;
        private List<Bombon> lista;

        private void frmBombones_Load(object sender, EventArgs e)
        {
            servicio = new BombonesServicios();
            try
            {
                lista = servicio.GetLista();
                MostrarDatosEnGrilla(DatosDataGridView, lista);

            }
            catch (Exception ex)
            {

            }
        }

        public static void MostrarDatosEnGrilla(DataGridView dataGrid, List<Bombon> lista)
        {
            LimpiarGrilla(dataGrid);
            foreach (var bombon in lista)
            {
                DataGridViewRow r = ConstruirFila(dataGrid);
                SetearFila(r, bombon);
                AgregarFila(dataGrid, r);
            }
        }

        public static void SetearFila(DataGridViewRow r, Bombon bombon)
        {
            r.Cells[0].Value = bombon.NombreBombon;
            r.Cells[1].Value = bombon.Tipo_Relleno.Relleno;
            r.Cells[2].Value = bombon.Tipo_Chocolate.Chocolate;
            r.Cells[3].Value = bombon.Tipo_Nuez.Nuez;
            r.Cells[4].Value = bombon.PrecioVenta;
            r.Cells[5].Value = bombon.Fabrica.NombreFabrica;
            r.Tag = bombon;
        }

        public static void LimpiarGrilla(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();

        }

        public static DataGridViewRow ConstruirFila(DataGridView dataGrid)
        {
            var r = new DataGridViewRow();
            r.CreateCells(dataGrid);
            return r;
        }
        

        public static void AgregarFila(DataGridView dataGrid, DataGridViewRow r)
        {
            dataGrid.Rows.Add(r);
        }

        private void NuevoIconButton_Click(object sender, EventArgs e)
        {
            frmBombonAE frm = new frmBombonAE() { Text = "Agregar un Bombon" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                Bombon bombon = frm.GetBombon();
                int registrosAfectados = servicio.Agregar(bombon);
                if (registrosAfectados == 0)
                {
                    MessageBox.Show("No se agregaron registros...",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    //Recargar grilla
                    RecargarGrilla();
                }
                else
                {                   
                    var r = ConstruirFila(DatosDataGridView);
                    SetearFila(r, bombon);
                    AgregarFila(DatosDataGridView, r);
                    MessageBox.Show("Registro agregado",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RecargarGrilla()
        {
            try
            {
                lista = servicio.GetLista();
                MostrarDatosEnGrilla(DatosDataGridView, lista);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BorrarIconButton_Click(object sender, EventArgs e)
        {
            {
                if (DatosDataGridView.SelectedRows.Count == 0)
                {
                    return;
                }

                try
                {
                    var r = DatosDataGridView.SelectedRows[0];
                    Bombon bombon = (Bombon)r.Tag;
                    DialogResult dr = MessageBox.Show($"¿Desea borrar el registro seleccionado de {bombon.NombreBombon}?",
                        "Confirmar Eliminación",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.No)
                    {
                        return;
                    }

                    int registrosAfectados = servicio.Borrar(bombon);
                    if (registrosAfectados == 0)
                    {
                        MessageBox.Show("No se borraron registros...",
                            "Advertencia",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        //Recargar grilla
                        RecargarGrilla();

                    }
                    else
                    {
                        DatosDataGridView.Rows.Remove(r);
                        MessageBox.Show("Registro eliminado",
                            "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
            }
        }

        private void EditarIconButton_Click(object sender, EventArgs e)
        {
            if (DatosDataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            var r = DatosDataGridView.SelectedRows[0];
            Bombon bombon = (Bombon)r.Tag;
            Bombon bombonAuxiliar = (Bombon)bombon.Clone();
            try
            {
                frmBombonAE frm = new frmBombonAE() { Text = "Editar un Bombon" };
                frm.SetBombon(bombon);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                bombon = frm.GetBombon();
                int registrosAfectados = servicio.Editar(bombon);
                if (registrosAfectados == 0)
                {
                    MessageBox.Show("No se borraron registros...",
                        "Advertencia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    //Recargar grilla
                    RecargarGrilla();

                }
                else
                {
                    SetearFila(r, bombon);
                    MessageBox.Show("Registro modificado",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                SetearFila(r, bombonAuxiliar);
                MessageBox.Show(exception.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
    
}
