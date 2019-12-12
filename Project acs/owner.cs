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
    public partial class owner : Form
    {
        public owner()
        {
            InitializeComponent();
        }
        public OracleConnection conn;
        
       
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void owner_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lihat_Pegawai l = new Lihat_Pegawai();
            l.conn = conn;
            l.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            report_klaim r = new report_klaim();
            r.conn = conn;
            r.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 log = new Form1();
            this.Hide();
            log.Show();
           

        }
    }
}
