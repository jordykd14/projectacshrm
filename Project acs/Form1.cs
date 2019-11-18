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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OracleCommand cmd;
        OracleConnection conn;
        OracleDataReader reader;
        bool cek = false;
        List<string> nama = new List<string>();
        List<string> username = new List<string>();
        List<string> pass = new List<string>();
        List<string> jabatan = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            nama.Clear();
            pass.Clear();
            username.Clear();
            jabatan.Clear();
            conn = new OracleConnection("data source = XE; user id = proyek;password = proyek");
            conn.Close();
            conn.Open();
            cmd = new OracleCommand("select p.nama_pegawai,p.pass,j.nama_jabatan,p.username from pegawai p,jabatan j where p.id_jabatan = j.id_jabatan",conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama.Add(reader.GetString(0));
                pass.Add(reader.GetString(1));
                jabatan.Add(reader.GetString(2));
                username.Add(reader.GetString(3));
            }
            reader.Close();
            conn.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idx = 0;
            cek = false;
            for (int i = 0; i < nama.Count(); i++)
            {
                if (textBox1.Text==username[i].ToString())
                {
                    cek = true;
                    idx = i;
                }
            }
            if (cek==true)
            {
                if (textBox2.Text==pass[idx].ToString())
                {
                    MessageBox.Show("Berhasil Login sebagai "+jabatan[idx].ToString());
                    if (jabatan[idx].ToString()=="Manager")
                    {
                        manager m = new manager();
                        m.conn = conn;
                        m.nama_peg = nama[idx].ToString();
                        m.jabatan = jabatan[idx].ToString();
                        m.ShowDialog();
                    }
                    else if (jabatan[idx].ToString()=="Owner")
                    {
                        owner o = new owner();
                        o.conn = conn;
                        o.ShowDialog();
                    }
                    else
                    {
                        lihat l = new lihat();
                        l.conn = conn;
                        l.nama = nama[idx].ToString();
                        l.jabatan = jabatan[idx].ToString();
                        l.ShowDialog();

                        

                    }
                }
                else
                {
                    MessageBox.Show("passwor anda salah");
                }
            }
            else
            {
                MessageBox.Show("USername anda salah");
            }
        }
    }
}