using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace Project_acs
{
    public partial class manager : Form
    {
        public manager()
        {
            InitializeComponent();
        }
        public OracleConnection conn;

        public string nama_peg, jabatan;
        private void manager_Load(object sender, EventArgs e)
        {

        }

        private void approveNotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
          


        }

        private void button1_Click(object sender, EventArgs e)
        {
            klaim k = new klaim();
            k.nama = nama_peg;
            k.jabatan = jabatan;
            k.conn = conn;
            k.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Approve a = new Approve();
            a.nama = nama_peg;
            a.conn = conn;
            a.ShowDialog();
        }

        private void kalimToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
