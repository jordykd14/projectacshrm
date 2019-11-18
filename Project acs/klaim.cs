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
    public partial class klaim : Form
    {
        public klaim()
        {
            InitializeComponent();
        }
        public OracleConnection conn;
        public string nama;
        public string jabatan;
        OracleCommand cmd;
        OracleDataReader reader;
        List<string> id_peg = new List<string>();
        List<string> id_manager = new List<string>();
        private void klaim_Load(object sender, EventArgs e)
        {
            id_peg.Clear();
            id_manager.Clear();
            conn.Close();
            conn.Open();
            //cmd = new OracleCommand("select nama_pegawai,id_pegawai from pegawai where id_jabatan != 'JA001'", conn);
            //reader = cmd.ExecuteReader();
            //while (reader.Read()) 
            //{
            //    comboBox1.Items.Add(reader.GetString(0));
            //    id_peg.Add(reader.GetString(1));
            //}
            //reader.Close();
            textBox1.Text = nama.ToString();
            textBox2.Text = jabatan.ToString();
            cmd = new OracleCommand("select nama_pegawai,id_pegawai from pegawai where id_jabatan = 'JA001'", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetString(0));
                id_manager.Add(reader.GetString(1));
            }
            reader.Close();
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();
            string id = "";
            MessageBox.Show(dateTimePicker1.Value.ToString("ddMMyyyy"));
            cmd = new OracleCommand($"select fautogenid({dateTimePicker1.Value.ToString("ddMMyyyy")}) from dual", conn);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                id = reader.GetString(0);
            }
            reader.Close();
            MessageBox.Show(id.ToString());
            nota n = new nota();
            n.nama_pegawai = nama.ToString();
            n.nama_manager = comboBox2.SelectedItem.ToString();
            n.id = id.ToString();
            n.conn = conn;
            n.ShowDialog();

        }
    }
}
