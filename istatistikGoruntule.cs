using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NesneProje
{
    public partial class istatistikGoruntule : Form
    {
        public istatistikGoruntule()
        {
            InitializeComponent();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void istatistikGoruntule_Load(object sender, EventArgs e)
        {
            try
            {
                NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=Film Kütüphanesi;Username=postgres;Password=Vipedmap1");
                baglanti.Open();

                // En yüksek puanlı filmler
                string enYuksekPuanSorgu = "SELECT filmadi, degerlendirmepuani FROM filmler ORDER BY degerlendirmepuani DESC LIMIT 10";
                NpgsqlDataAdapter enYuksekPuanAdapter = new NpgsqlDataAdapter(enYuksekPuanSorgu, baglanti);
                DataTable enYuksekPuanTable = new DataTable();
                enYuksekPuanAdapter.Fill(enYuksekPuanTable);
                dataGridView1.DataSource = enYuksekPuanTable;

                // En çok değerlendirilen türler
                string enCokDegerlendirilenTurSorgu = "SELECT tur, COUNT(*) AS filmSayisi FROM filmler GROUP BY tur ORDER BY filmSayisi DESC LIMIT 10";
                NpgsqlDataAdapter enCokDegerlendirilenTurAdapter = new NpgsqlDataAdapter(enCokDegerlendirilenTurSorgu, baglanti);
                DataTable enCokDegerlendirilenTurTable = new DataTable();
                enCokDegerlendirilenTurAdapter.Fill(enCokDegerlendirilenTurTable);
                dataGridView2.DataSource = enCokDegerlendirilenTurTable;

                // En eski filmler
                string enEskiFilmlerSorgu = "SELECT filmadi, yayinyili FROM filmler ORDER BY yayinyili ASC LIMIT 10";
                NpgsqlDataAdapter enEskiFilmlerAdapter = new NpgsqlDataAdapter(enEskiFilmlerSorgu, baglanti);
                DataTable enEskiFilmlerTable = new DataTable();
                enEskiFilmlerAdapter.Fill(enEskiFilmlerTable);
                dataGridView3.DataSource = enEskiFilmlerTable;

                // En yeni filmler
                string enYeniFilmlerSorgu = "SELECT filmadi, yayinyili FROM filmler ORDER BY yayinyili DESC LIMIT 10";
                NpgsqlDataAdapter enYeniFilmlerAdapter = new NpgsqlDataAdapter(enYeniFilmlerSorgu, baglanti);
                DataTable enYeniFilmlerTable = new DataTable();
                enYeniFilmlerAdapter.Fill(enYeniFilmlerTable);
                dataGridView4.DataSource = enYeniFilmlerTable;

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
