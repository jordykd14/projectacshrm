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
    public partial class lihat : Form
    {
        public lihat()
        {
            InitializeComponent();
        }
        public OracleConnection conn;
        OracleDataReader reader;
        OracleCommand cmd;
        public string nama;
        public string id_peg;
        public string jabatan;
        List<string> id_nota = new List<string>();
        List<string> tanggal = new List<string>();
        List<string> status = new List<string>();
        List<int> total = new List<int>();
        
        private void lihat_Load(object sender, EventArgs e)
        {
            label2.Text = nama.ToString();
            load();
        }

        public void load()
        {
            id_nota.Clear();
            tanggal.Clear();
            total.Clear();
            status.Clear();
            conn.Close();
            conn.Open();
            OracleCommand cmd = new OracleCommand($"SELECT ID_NOTA, TANGGAL_DISERAHKAN, TOTAL , status_nota FROM HNOTA where id_pegawai='{id_peg.ToString()}'",conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_nota.Add(reader.GetString(0));
                tanggal.Add(reader.GetDateTime(1).ToString("dd/MM/yyy"));
                total.Add(Convert.ToInt32(reader.GetValue(2).ToString()));
                if (reader.GetString(3)=="1")
                {
                    status.Add("Disetujui");
                }
                else if (reader.GetString(3)=="0")
                {
                    status.Add("Belom Disetujui");
                }
            }
            reader.Close();
            for (int i = 0; i < id_nota.Count; i++)
            {
                if (status[i]=="Disetujui")
                {
                    dataGridView1.Rows.Add(id_nota[i].ToString(), tanggal[i].ToString(), total[i],"Approve");
                    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows.Add(id_nota[i].ToString(), tanggal[i].ToString(), total[i], "Belom");
                    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Red;
                }
                
                
            }
            //ini masih mau tak isi yo
        }
        private void button1_Click(object sender, EventArgs e)
        {
            klaim k = new klaim();
            k.conn = conn;
            k.nama = nama;
            k.jabatan = jabatan;
            this.Close();
            k.ShowDialog();
            
        }
    }
}
