using BombonesPP2022.Entidades;
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

        private void frmBombonAE_Load(object sender, EventArgs e)
        {

        }

        private Bombon bombon;
        public void SetBombon(Bombon bombon)
        {
            this.bombon = bombon;
        }
        public Bombon GetBombon()
        {
            return bombon;
        }
    }
}
