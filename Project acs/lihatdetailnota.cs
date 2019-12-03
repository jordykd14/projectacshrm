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
    public partial class lihatdetailnota : Form
    {
        public lihatdetailnota()
        {
            InitializeComponent();
        }
        public OracleConnection conn;
        public string id;
        private void lihatdetailnota_Load(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();
            reportdetailnota crpt = new reportdetailnota();
            try
            {
                crpt.SetDatabaseLogon("proyek", "proyek");
            }
            catch (Exception)
            {

                throw;
            }
            crpt.SetParameterValue("idnota", id);
            crystalReportViewer1.ReportSource = crpt;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
