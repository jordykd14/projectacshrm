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
    public partial class Lihat_Pegawai : Form
    {
        public Lihat_Pegawai()
        {
            InitializeComponent();
        }
        public OracleConnection conn;
        public OracleCommand cmd;
        public OracleDataReader reader;
        List<string> id_jabatan = new List<string>();
        List<string> nama_jabatan = new List<string>();
        List<string> id_pegawai = new List<string>();
        List<string> nama_pegawai = new List<string>();
        List<string> username = new List<string>();
        List<string> pasword = new List<string>();
        String mode = "";
        private void button1_Click(object sender, EventArgs e)
        {
            //insert
            conn.Close();
            conn.Open();
            cmd = new OracleCommand($"insert into pegawai values('{id_jabatan[comboBox1.SelectedIndex].ToString()}','{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}')", conn);
            cmd.ExecuteNonQuery();
            Lihat_Pegawai_Load(this, e);
            button4_Click(this, e);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //update
            //MessageBox.Show("Test");
            conn.Close();
            conn.Open();
            cmd = new OracleCommand($"update pegawai set id_jabatan='{id_jabatan[comboBox1.SelectedIndex]}', nama_pegawai='{textBox2.Text}', username='{textBox3.Text}',pass='{textBox4.Text}' where id_pegawai='{textBox1.Text}'", conn);
            cmd.ExecuteNonQuery();
            load();
            button4_Click(this, e);
        }

        private void Lihat_Pegawai_Load(object sender, EventArgs e)
        {
            conn.Close();
            conn.Open();
            id_jabatan.Clear();
            comboBox1.Items.Clear();
            cmd = new OracleCommand("select nama_jabatan, id_jabatan from jabatan", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(1).ToString()!="JA005")
                {
                    comboBox1.Items.Add(reader.GetString(0));
                    id_jabatan.Add(reader.GetString(1));
                }
            }
            reader.Close();
            conn.Close();
            button2.Visible = false;
            button4.Enabled = false;
            load();
            mode ="insert";

        }
        public void load()
        {
            id_pegawai.Clear();
            nama_pegawai.Clear();
            username.Clear();
            nama_jabatan.Clear();
            pasword.Clear();
            conn.Close();
            conn.Open();
            cmd = new OracleCommand("select p.id_pegawai,j.nama_jabatan,p.nama_pegawai,p.username,p.pass from pegawai p , jabatan j where p.id_jabatan = j.id_jabatan and p.id_jabatan != 'JA005'", conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_pegawai.Add(reader.GetString(0));
                nama_jabatan.Add(reader.GetString(1));
                nama_pegawai.Add(reader.GetString(2));
                username.Add(reader.GetString(3));
                pasword.Add(reader.GetString(4));
            }
            reader.Close();
            conn.Close();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < id_pegawai.Count; i++)
            {
                dataGridView1.Rows.Add(id_pegawai[i].ToString(), nama_pegawai[i].ToString(), username[i].ToString(), pasword[i].ToString(), nama_jabatan[i].ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mode = "update";
            int idx = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[idx].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[idx].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[idx].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[idx].Cells[3].Value.ToString();
            comboBox1.SelectedItem = dataGridView1.Rows[idx].Cells[4].Value.ToString();
            button1.Visible = false;
            button2.Visible = true;
            button4.Enabled = true;
           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            mode = "insert";
            button1.Visible = true;
            button2.Visible = false;
            button4.Enabled = false;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
            dataGridView1.ClearSelection();
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text!="")
            {
                if (mode=="insert")
                {
                    if (textBox2.TextLength > 1)
                    {
                        conn.Close();
                        conn.Open();
                        cmd = new OracleCommand($"select autoid('{textBox2.Text}') from dual", conn);
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            textBox1.Text = reader.GetString(0);
                        }
                        reader.Close();
                        conn.Close();
                    }
                    else
                    {
                        textBox1.ResetText();
                    }
                }
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Boolean ada = false;

            conn.Close();
            conn.Open();
            cmd = new OracleCommand($"select * from hnota where id_pegawai='{textBox1.Text}'",conn);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                ada = true;
            }
            reader.Close();

            if (!ada)
            {
                cmd = new OracleCommand($"delete from pegawai where id_pegawai='{textBox1.Text}'", conn);
                cmd.ExecuteNonQuery();
                load();
                button4_Click(this, e);
            }
            else
            {
                List<string> idNota = new List<string>();
                cmd = new OracleCommand($"select id_nota from hnota where id_pegawai = '{textBox1.Text}'",conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idNota.Add(reader.GetString(0));
                }
                reader.Close();

                for (int i = 0; i < idNota.Count; i++)
                {
                    MessageBox.Show(idNota[i]);
                    cmd = new OracleCommand($"delete from dnota where id_nota = '{idNota[i]}'",conn);
                    cmd.ExecuteNonQuery();
                }
                cmd = new OracleCommand($"delete from hnota where id_pegawai = '{textBox1.Text}'",conn);
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand($"delete from pegawai where id_pegawai='{textBox1.Text}'", conn);
                cmd.ExecuteNonQuery();

                int index = -1;

                for (int i = 0; i < id_pegawai.Count; i++)
                {
                    if (id_pegawai[i] == textBox1.Text)
                    {
                        index = i;
                    }
                }

                id_pegawai.RemoveAt(index);
                nama_jabatan.RemoveAt(index);
                nama_pegawai.RemoveAt(index);
                username.RemoveAt(index);
                pasword.RemoveAt(index);

                load();
                button4_Click(this, e);
            }
            
        }
    }
}
