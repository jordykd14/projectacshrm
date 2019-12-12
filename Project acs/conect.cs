using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_acs
{
    public partial class conect : Form
    {
        public conect()
        {
            InitializeComponent();
        }
        public static string datasource;
        public static string iduser;
        public static string password;
        public static string host;
        private void button1_Click(object sender, EventArgs e)
        {
            datasource = textBox1.Text;
            iduser = textBox2.Text;
            password = textBox3.Text;
            host = textBox4.Text;
            Form1 log = new Form1();
            this.Hide();
            log.ShowDialog();
            
        }
    }
}
