using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NesneProje
{
    public partial class yoneticiEkrani : Form
    {
        public yoneticiEkrani()
        {
            InitializeComponent();
        }

        private void yonetici_Load(object sender, EventArgs e)
        {
            string sorgu = "select * from filmler";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            baglanti.Open();
            string sorgu2 = "select filmadi from filmler";
            NpgsqlDataAdapter fa = new NpgsqlDataAdapter(sorgu2,baglanti);
            DataTable dt = new DataTable();
            fa.Fill(dt);
            comboBox1.DisplayMember = "filmadi";
            comboBox1.ValueMember = "filmid";
            comboBox1.DataSource = dt;
            baglanti.Close();
        }


        private void filmeklebuton_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void filmguncellebtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localhost; port=5432; Database=Film Kütüphanesi; user ID=postgres;" +
                                                         "password=Vipedmap1");
        public class Filmler
        {
            public string filmadi { get; set; }
            public string yonetmen { get; set; }
            public string oyuncular { get; set; }
            public string tur { get; set; }
            public int yayinyili { get; set; }
            public decimal degerlendirmepuani { get; set; }

            // Yapıcı (constructor) ekleyin
            public Filmler(string filmadi, string yonetmen, string oyuncular, string tur, string yayinyili, string degerlendirmepuani)
            {
                this.filmadi = filmadi;
                this.yonetmen = yonetmen;
                this.oyuncular = oyuncular;
                this.tur = tur;

                // Yayın yılını int türüne dönüştürme
                if (int.TryParse(yayinyili, out int yayinYiliInt))
                {
                    this.yayinyili = yayinYiliInt;
                }
                else
                {
                    // Hata durumunu ele alabilirsiniz, örneğin varsayılan bir değer atayabilirsiniz.
                    this.yayinyili = 0;
                }

                // Değerlendirme puanını decimal türüne dönüştürme
                if (decimal.TryParse(degerlendirmepuani, out decimal degerlendirmePuan))
                {
                    this.degerlendirmepuani = degerlendirmePuan;
                }
                else
                {
                    // Hata durumunu ele alabilirsiniz, örneğin varsayılan bir değer atayabilirsiniz.
                    this.degerlendirmepuani = 0.0m;
                }
            }
        }
        private void filmeklebtn_Click(object sender, EventArgs e)
        {
            Filmler film = new Filmler(filmAdiekletxt.Text, yonetmenekletxt.Text, oyuncuekletxxt.Text, turekletxt.Text, yayinyiliekletxt.Text, degerlendirmeekletxt.Text);

            baglanti.Open();
            NpgsqlCommand komut1 = new NpgsqlCommand("INSERT INTO filmler (filmadi, yonetmen, oyuncular, tur, yayinyili, degerlendirmepuani) " +
                                                   "VALUES (@p1, @p2, @p3, @p4, @p5, @p6)", baglanti);

            komut1.Parameters.AddWithValue("@p1", film.filmadi);
            komut1.Parameters.AddWithValue("@p2", film.yonetmen);
            komut1.Parameters.AddWithValue("@p3", film.oyuncular);
            komut1.Parameters.AddWithValue("@p4", film.tur);
            komut1.Parameters.AddWithValue("@p5", film.yayinyili);
            komut1.Parameters.AddWithValue("@p6", film.degerlendirmepuani);

            komut1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Film Ekleme İşlemi Başarılı");
            
            string sorgu = "select * from filmler";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];


        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string sorgu = "select * from filmler";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

    
        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void cikisYapbtn_Click(object sender, EventArgs e)
        {
            anaEkran anaEkranForm = new anaEkran();
            anaEkranForm.Show();
            this.Close();
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sorgu2 = "select filmadi from filmler";
            NpgsqlDataAdapter fa = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataTable dt = new DataTable();
            fa.Fill(dt);
            comboBox1.DisplayMember = "filmadi";
            comboBox1.ValueMember = "filmid";
            comboBox1.DataSource = dt;
            baglanti.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Close();
            DataRowView selectedRow = comboBox1.SelectedItem as DataRowView;
            string selectedFilmAdi = selectedRow["filmadi"].ToString();
            baglanti.Open();
            string sorgu = "SELECT * FROM filmler WHERE filmadi = @filmAdi";
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@filmAdi", selectedFilmAdi);

            NpgsqlDataReader reader = komut.ExecuteReader();
            if (reader.Read())
            {
                // Veritabanından alınan değerleri TextBox'lara yaz
                textBox1.Text = reader["filmadi"].ToString();
                textBox2.Text = reader["yonetmen"].ToString();
                textBox3.Text = reader["oyuncular"].ToString();
                textBox4.Text = reader["tur"].ToString();
                textBox5.Text = reader["yayinyili"].ToString();
                textBox6.Text = reader["degerlendirmepuani"].ToString();
            }
            else
            {
                MessageBox.Show("Film bilgileri bulunamadı.");
            }
            baglanti.Close();
        }

        private void guncellemeBtn_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan değerleri TextBox'lar üzerinden al
            string yeniFilmAdi = textBox1.Text;
            string yeniYonetmen = textBox2.Text;
            string yeniOyuncular = textBox3.Text;
            string yeniTur = textBox4.Text;
            int yeniYayinYili = Convert.ToInt32(textBox5.Text);
            double yeniDegerlendirmePuani = Convert.ToDouble(textBox6.Text);

            // Film adını belirle
            DataRowView selectedRow = comboBox1.SelectedItem as DataRowView;
            string selectedFilmAdi = selectedRow["filmadi"].ToString();

            try
            {
                // Veritabanı bağlantısı
                string baglantiString = "Host=localhost;Port=5432;Database=Film Kütüphanesi;user ID=postgres;password=Vipedmap1";

                using (NpgsqlConnection baglanti = new NpgsqlConnection(baglantiString))
                {
                    baglanti.Open();

                    // Kullanıcı bilgilerini güncelle
                    string sorgu = "UPDATE filmler SET filmadi = @p1, yonetmen = @p2, oyuncular = @p3, tur = @p4, yayinyili = @p5, degerlendirmepuani = @p6 WHERE filmadi = @p7";

                    using (NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@p1", yeniFilmAdi);
                        komut.Parameters.AddWithValue("@p2", yeniYonetmen);
                        komut.Parameters.AddWithValue("@p3", yeniOyuncular);
                        komut.Parameters.AddWithValue("@p4", yeniTur);
                        komut.Parameters.AddWithValue("@p5", yeniYayinYili);
                        komut.Parameters.AddWithValue("@p6", yeniDegerlendirmePuani);
                        komut.Parameters.AddWithValue("@p7", selectedFilmAdi);


                        int etkilenenSatirSayisi = komut.ExecuteNonQuery();

                        if (etkilenenSatirSayisi > 0)
                        {
                            MessageBox.Show("Film bilgileri başarıyla güncellendi.");
                        }
                        else
                        {
                            MessageBox.Show("Film bilgileri güncellenirken bir hata oluştu.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }

            
        }
    }
}