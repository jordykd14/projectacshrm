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
    public partial class report_klaim : Form
    {
        public report_klaim()
        {
            InitializeComponent();
        }
        public OracleConnection conn;
        OracleCommand cmd;
        OracleDataReader reader;
        List<string> id_manager = new List<string>();

        private void report_klaim_Load(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();

            comboBox1.Items.Add("Semua");
            cmd = new OracleCommand("select nama_pegawai,id_pegawai from pegawai where id_jabatan != 'JA005'", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
                id_manager.Add(reader.GetString(1));
            }
            reader.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            KlaimReport crpt = new KlaimReport();
            try
            {
                crpt.SetDatabaseLogon("proyek", "proyek");
                crpt.SetParameterValue("NamaPegawai", comboBox1.SelectedText);
                crystalReportViewer1.ReportSource = crpt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }
    }
}
