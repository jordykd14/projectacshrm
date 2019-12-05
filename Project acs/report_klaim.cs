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

        private void report_klaim_Load(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();
            CrystalReport1 crpt = new CrystalReport1();
            try
            {
                crpt.SetDatabaseLogon("proyek", "proyek");
            }
            catch (Exception)
            {

                throw;
            }
            crystalReportViewer1.ReportSource = crpt;
        }
    }
}
