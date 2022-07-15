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
    public partial class frmFabricas : Form
    {
        public frmFabricas()
        {
            InitializeComponent();
        }

        private FabricaServicios servicio;
        private List<Fabrica> lista;
        private void frmFabricas_Load(object sender, EventArgs e)
        {
            servicio = new FabricaServicios();
            try
            {
                lista = servicio.GetLista();
                MostrarDatosEnGrilla(DatosDataGridView, lista);

            }
            catch (Exception ex)
            {

            }
        }

        public static void MostrarDatosEnGrilla(DataGridView dataGrid, List<Fabrica> lista)
        {
            LimpiarGrilla(dataGrid);
            foreach (var fabrica in lista)
            {
                DataGridViewRow r = ConstruirFila(dataGrid);
                SetearFila(r, fabrica);
                AgregarFila(dataGrid, r);
            }
        }

        public static void SetearFila(DataGridViewRow r, Fabrica fabrica)
        {
            r.Cells[0].Value = fabrica.NombreFabrica;
            r.Cells[1].Value = fabrica.Direccion;
            r.Cells[2].Value = fabrica.Pais.NombrePais;
            r.Cells[3].Value = fabrica.GerenteDeVentas;
            r.Tag = fabrica;
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
            frmFabricasAE frm = new frmFabricasAE() { Text = "Agregar una Fabrica" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                Fabrica fabrica = frm.GetFabrica();
                int registrosAfectados = servicio.Agregar(fabrica);
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
                    SetearFila(r, fabrica);
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
                    Fabrica fabrica = (Fabrica)r.Tag;
                    DialogResult dr = MessageBox.Show($"¿Desea borrar el registro seleccionado de {fabrica.NombreFabrica}?",
                        "Confirmar Eliminación",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.No)
                    {
                        return;
                    }

                    int registrosAfectados = servicio.Borrar(fabrica);
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
            Fabrica fabrica = (Fabrica)r.Tag;
            Fabrica fabricaAuxiliar = (Fabrica)fabrica.Clone();
            try
            {
                frmFabricasAE frm = new frmFabricasAE() { Text = "Editar una fabrica" };
                frm.SetFabrica(fabrica);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                fabrica = frm.GetFabrica();
                int registrosAfectados = servicio.Editar(fabrica);
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
                    SetearFila(r, fabrica);
                    MessageBox.Show("Registro modificado",
                        "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                SetearFila(r, fabricaAuxiliar);
                MessageBox.Show(exception.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DatosDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
