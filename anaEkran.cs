using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace NesneProje
{
    public partial class anaEkran : Form
    {
        public anaEkran()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void girisEkraniBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=Film Kütüphanesi; user ID=postgres;" +
                                                         "password=Vipedmap1");
        private void kullaniciEkraniBtn_Click(object sender, EventArgs e)
        {

            tabControl1.SelectedTab = tabPage2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sorgu = "select * from kullanicilar";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu,baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void anaEkran_Load(object sender, EventArgs e)
        {

        }

        private void yoneticiEkraniBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
