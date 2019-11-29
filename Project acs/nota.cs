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

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();
            int index = 0; ;
            Boolean berhasil = false;
            try
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    OracleCommand commit = new OracleCommand("commit",conn);
                    commit.ExecuteNonQuery();
                    int idx = -1;
                    string tgl = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    for (int j = 0; j < jenis.Count; j++)
                    {
                        if (jenis[j] == dataGridView1.Rows[i].Cells[1].Value.ToString())
                        {
                            idx = j;
                        }
                    }
                    int harga = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    double hargaBersih = harga - (harga * 10 / 100);
                    OracleCommand cmd = new OracleCommand($"INSERT INTO DNOTA VALUES('{label6.Text}','{id_jenis[idx]}',TO_DATE('{tgl}','dd/MM/yyyy'),{harga},{10},{hargaBersih})", conn);
                    cmd.ExecuteNonQuery();
                    index++;
                }
                MessageBox.Show("Berhasil mengklaim nota");
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Klaim nota gagal, coba ulang kembali");
                OracleCommand roll = new OracleCommand("rollback", conn);
                roll.ExecuteNonQuery();
            }
            
            
        }
    }
}
