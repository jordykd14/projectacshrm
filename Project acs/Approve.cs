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
    public partial class Approve : Form
    {
        public OracleConnection conn;
        OracleCommand cmd;
        OracleDataReader reader;
        public string nama;
        string id_peg;
        List<string> id_nota = new List<string>();
        List<string> nama_peg = new List<string>();
        List<string> tanggal = new List<string>();
        List<int> total = new List<int>();

        public Approve()
        {
            InitializeComponent();
        }

        private void Approve_Load(object sender, EventArgs e)
        {
            load();
            
        }
        public void load()
        {
            id_nota.Clear();
            nama_peg.Clear();
            tanggal.Clear();
            total.Clear();
            conn.Close();
            conn.Open();
            cmd = new OracleCommand($"select id_pegawai from pegawai where nama_pegawai ='{nama}'", conn);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                id_peg = reader.GetString(0);
            }
            reader.Close();
            cmd = new OracleCommand($"select h.id_nota, p.nama_pegawai, h.tanggal_diserahkan,h.total from hnota h, pegawai p where h.id_pegawai = p.id_pegawai and h.status_nota='0' and h.id_manager = '{id_peg}'", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_nota.Add(reader.GetString(0));
                nama_peg.Add(reader.GetString(1));
                tanggal.Add(reader.GetDateTime(2).ToString("dd/MM/yyyy"));
                total.Add(Convert.ToInt32(reader.GetValue(3).ToString()));
            }
            reader.Close();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < id_nota.Count; i++)
            {
                dataGridView1.Rows.Add(id_nota[i].ToString(), nama_peg[i].ToString(), tanggal[i].ToString(), total[i].ToString(), "Detail", "Approve");
            }
            conn.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            if (e.ColumnIndex==4)
            {
                lihatdetailnota ld = new lihatdetailnota();
                ld.conn = conn;
                ld.id = dataGridView1.Rows[idx].Cells[0].Value.ToString();
                ld.ShowDialog();
            }
            if (e.ColumnIndex==5)
            {
                conn.Close();
                conn.Open();
                cmd = new OracleCommand($"update hnota set tanggal_diclaim=TO_DATE('{DateTime.Now.ToString("dd/MM/yyy")}','dd/MM/yyyy'), status_nota='1' where id_nota='{dataGridView1.Rows[idx].Cells[0].Value.ToString()}'", conn);
                cmd.ExecuteNonQuery();
                load();
                
            }
        }
    }
}
