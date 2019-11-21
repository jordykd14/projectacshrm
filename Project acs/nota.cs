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
    public partial class nota : Form
    {
        public nota()
        {
            InitializeComponent();
        }

        public string nama_pegawai;
        public string nama_manager;
        public string id;
        List<string> id_jenis = new List<string>();
        List<string> tanggal = new List<string>();
        List<string> jenis = new List<string>();
        List<int> uang = new List<int>();
        public OracleConnection conn;
        OracleCommand cmd;
        OracleDataReader reader;
        private void nota_Load(object sender, EventArgs e)
        {
            label2.Text = nama_pegawai.ToString();
            label4.Text = nama_manager.ToString();
            label6.Text = id.ToString();
            conn.Close();
            conn.Open();
            id_jenis.Clear();
            cmd = new OracleCommand("select nama_jenis,id_jenis from jenis", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
                id_jenis.Add(reader.GetString(1));
            }
            reader.Close();
            conn.Close();
            uang.Clear();
            tanggal.Clear();
            jenis.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Add(dateTimePicker1.Value.ToString("dd/MM/yyyy"), comboBox1.SelectedItem.ToString(),harga.ToString(), "Hapus");
            tanggal.Add(dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            jenis.Add(comboBox1.SelectedItem.ToString());
            uang.Add(Convert.ToInt32(numericUpDown1.Value));
            dataGridView1.Rows.Clear();
            for (int i = 0; i < tanggal.Count; i++)
            {
                dataGridView1.Rows.Add(tanggal[i].ToString(), jenis[i].ToString(), uang[i].ToString(),"Hapus");
            }
            numericUpDown1.Value = 0;
            comboBox1.SelectedIndex = -1;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            if (e.ColumnIndex==3)
            {
                tanggal.Remove(dataGridView1.Rows[idx].Cells[0].Value.ToString());
                jenis.Remove(dataGridView1.Rows[idx].Cells[1].Value.ToString());
                uang.Remove(Convert.ToInt32(dataGridView1.Rows[idx].Cells[2].Value.ToString()));
                dataGridView1.Rows.RemoveAt(idx);

            }
            
            
        }
        
    }
}
