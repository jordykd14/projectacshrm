﻿using System;
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
        public string nama;
        public string jabatan;
        private void lihat_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            klaim k = new klaim();
            k.conn = conn;
            k.nama = nama;
            k.jabatan = jabatan;
            k.ShowDialog();
        }
    }
}