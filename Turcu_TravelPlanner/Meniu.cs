using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turcu_TravelPlanner
{
    public partial class Meniu : Form
    {
        public Meniu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TravelPlanner frm = new TravelPlanner();
            frm.ShowDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Meniu_FormClosed(sender, new FormClosedEventArgs(CloseReason.None));
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            TravelPlanner frm = new TravelPlanner();
            frm.ShowDialog();
        }

        private void Meniu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
