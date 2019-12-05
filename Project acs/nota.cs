using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace Project_acs
{
    public partial class nota : Form
    {
        public nota()
        {
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        public string nama_pegawai;
        public string nama_manager;
        public string id_peg;
        public string jabatan;
        public string id;
        List<string> id_jenis = new List<string>();
        List<string> nama_jenis = new List<string>();
        List<string> tanggal = new List<string>();
        List<string> jenis = new List<string>();
        List<int> uang = new List<int>();
        public OracleConnection conn;
        OracleCommand cmd;
        OracleDataReader reader;
        List<string> gambar = new List<string>();
        List<string> lokasi = new List<string>();
        int total = 0;
        private void nota_Load(object sender, EventArgs e)
        {
            label2.Text = nama_pegawai.ToString();
            label4.Text = nama_manager.ToString();
            label6.Text = id.ToString();
            conn.Close();
            conn.Open();
            id_jenis.Clear();
            nama_jenis.Clear();
            cmd = new OracleCommand("select nama_jenis,id_jenis from jenis", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
                nama_jenis.Add(reader.GetString(0));
                id_jenis.Add(reader.GetString(1));
            }
            label8.Text = total.ToString();
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
            total += Convert.ToInt32(numericUpDown1.Value);
            label8.Text = total.ToString();
            numericUpDown1.Value = 0;
            comboBox1.SelectedIndex = -1;

            gambar.Add(nameImage);
            lokasi.Add(file.FileName);

            nameImage = null;
            file.FileName = null;
            pictureBox1.Image = null;
            button2.Enabled = true;
            button1.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            if (e.ColumnIndex==3)
            {
                tanggal.Remove(dataGridView1.Rows[idx].Cells[0].Value.ToString());
                jenis.Remove(dataGridView1.Rows[idx].Cells[1].Value.ToString());
                total -= Convert.ToInt32(dataGridView1.Rows[idx].Cells[2].Value.ToString());
                uang.Remove(Convert.ToInt32(dataGridView1.Rows[idx].Cells[2].Value.ToString()));
                dataGridView1.Rows.RemoveAt(idx);  
            }
            label8.Text = total.ToString();
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();
            int index = 0;
            double total_bersih = 0;
            Boolean berhasil = false;
            try
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string sourceFile = @""+lokasi[i]+"";
                    string destinationFile = @""+ Application.StartupPath + "\\BuktiBayar\\"+gambar[i];
                    try
                    {
                        File.Copy(sourceFile, destinationFile, true);
                    }
                    catch (IOException iox)
                    {
                        MessageBox.Show(iox.ToString());
                    }
                    OracleCommand commit = new OracleCommand("commit",conn);
                    commit.ExecuteNonQuery();
                    int idx = -1;
                    string tgl = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    for (int j = 0; j < nama_jenis.Count; j++)
                    {
                        if ( nama_jenis[j] == dataGridView1.Rows[i].Cells[1].Value.ToString())
                        {
                            idx = j;
                        }
                    }
                    int harga = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    double hargaBersih = harga + (harga * 10 / 100);
                    total_bersih += hargaBersih;
                    OracleCommand cmd = new OracleCommand($"INSERT INTO DNOTA VALUES('{label6.Text}','{id_jenis[idx]}',TO_DATE('{tgl}','dd/MM/yyyy'),{harga},{10},{hargaBersih},'{lokasi[i]}')", conn);

                    cmd.ExecuteNonQuery();
                    index++;
                }
                cmd = new OracleCommand($"update hnota set total={total_bersih} where id_nota='{label6.Text}'", conn);
                cmd.ExecuteNonQuery();
                cmd = new OracleCommand($"select p.id_pegawai, j.nama_jabatan from pegawai p, jabatan j where p.id_jabatan = j.id_jabatan and nama_pegawai ='{nama_pegawai}'", conn);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    id_peg = reader.GetString(0);
                    jabatan = reader.GetString(1);

                }                                      
                MessageBox.Show("Berhasil mengklaim nota");
                this.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Klaim nota gagal, coba ulang kembali");
                OracleCommand roll = new OracleCommand("rollback", conn);
                roll.ExecuteNonQuery();
            }
            
            
        }

        string nameImage;
        OpenFileDialog file = new OpenFileDialog();

        private void button3_Click(object sender, EventArgs e)
        {
            string inDirectory = file.InitialDirectory;
            inDirectory = @"D:\";
            file.Title = "Open Image File";
            file.Filter = "Image Files(*.jpg,*.png,*.tiff,*.bmp,*.gif)|*.jpg;*.png;*.tiff;*.bmp;*.gif";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;
            
            if (file.ShowDialog() == DialogResult.OK)
            {
                Image gbr = new Bitmap(file.FileName);
                pictureBox1.Image = new Bitmap(gbr, pictureBox1.Width,pictureBox1.Height);
                nameImage = file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1);
                gambar.Add(nameImage);
                lokasi.Add(file.FileName);
                button1.Enabled = true;
            }
            
        }
    }
}
