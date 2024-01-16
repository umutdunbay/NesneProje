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
    public partial class standartKullaniciEkrani : Form
    {
        
        private string baglantiString = "server = localhost; port=5432; Database=Film Kütüphanesi; user ID = postgres;password=Vipedmap1";

        private string kullaniciAdi;
        private string uyelikDurumu;

        
        public standartKullaniciEkrani(string kullaniciAdi)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
        }

        private void standartKullaniciEkrani_Load(object sender, EventArgs e)
        {
            using (var baglanti = new NpgsqlConnection(baglantiString))
            {
                baglanti.Open();

                string sorgu = "SELECT isim, soyisim, tcno, dogumtarihi, cinsiyet FROM kullanicilar WHERE kullaniciadi = @kullaniciadi";

                using (var komut = new NpgsqlCommand(sorgu, baglanti))
                {
                    // Burada uygun kullaniciadi değerini belirleyerek sorguyu çalıştırabilirsiniz.
                    komut.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);

                    using (var reader = komut.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Verileri alarak TextBox'lara yerleştir
                            textBox1.Text = reader["isim"].ToString();
                            textBox2.Text = reader["soyisim"].ToString();
                            textBox3.Text = reader["tcno"].ToString();
                            textBox4.Text = reader["dogumtarihi"].ToString();
                            textBox5.Text = reader["cinsiyet"].ToString();
                        }
                        else
                        {
                            // Kullanıcı bulunamadı veya hata oluştu
                            // Burada gerekli hata işlemlerini ekleyebilirsiniz.
                        }
                    }
                }

                baglanti.Close();

                baglanti.Open();

                string sorgu2 = "SELECT uyelikturu FROM kullanicilar WHERE kullaniciadi = @kullaniciadi";

                using (var komut = new NpgsqlCommand(sorgu2, baglanti))
                {
                    komut.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);

                    object result = komut.ExecuteScalar();

                    if (result != null)
                    {
                        uyelikDurumu = result.ToString();
                    }
                    else
                    {
                        // Kullanıcı bulunamadı veya uyelikturu bilgisi yoksa uygun bir hata işlemi ekleyebilirsiniz.
                    }
                }
                linkLabel1.Text = uyelikDurumu;
                baglanti.Close();
            }
        }

        private void hesabaGitmebtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void cikisYapbtn_Click(object sender, EventArgs e)
        {
            anaEkran anaEkranForm = new anaEkran();
            anaEkranForm.Show();
            this.Close();
        }

        private void filmPagebtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void bilgiGuncelleBtn_Click_1(object sender, EventArgs e)
        {
            BilgiGuncelleme bilgiGuncellemeForm = new BilgiGuncelleme(kullaniciAdi);
            bilgiGuncellemeForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string skullaniciAdi = kullaniciAdi;

            try
            {
                // Veritabanı bağlantısı
                string baglantiString = "Host=localhost;Port=5432;Database=Film Kütüphanesi;user ID=postgres;password=Vipedmap1";

                using (NpgsqlConnection baglanti = new NpgsqlConnection(baglantiString))
                {
                    baglanti.Open();

                    // Kullanıcıya ait tüm verileri sil
                    string sorgu = "DELETE FROM kullanicilar WHERE kullaniciadi = @q1";

                    using (NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@q1", skullaniciAdi);

                        int etkilenenSatirSayisi = komut.ExecuteNonQuery();

                        if (etkilenenSatirSayisi > 0)
                        {
                            MessageBox.Show("Hesap başarıyla silindi.");
                            anaEkran anaEkranForm = new anaEkran();
                            anaEkranForm.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Hesap silinirken bir hata oluştu.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UyelikDegistir uyelikDegistir = new UyelikDegistir(kullaniciAdi);
            uyelikDegistir.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filmadi = textBox6.Text.Trim();

            if (!string.IsNullOrEmpty(filmadi))
            {
                FilmAraVeGoster(filmadi);
            }
            else
            {
                MessageBox.Show("Lütfen film adını giriniz.");
            }
        }
        private void FilmAraVeGoster(string filmadi)
        {
            try
            {
                using (var baglanti = new NpgsqlConnection(baglantiString))
                {
                    baglanti.Open();

                    string sorgu = "SELECT * FROM filmler WHERE filmadi ILIKE @filmadi";

                    using (var komut = new NpgsqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@filmadi", "%" + filmadi + "%");

                        using (var adapter = new NpgsqlDataAdapter(komut))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                dataGridView2.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("Aradığınız film bulunamadı.");
                                dataGridView2.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filmadi = textBox6.Text.Trim();
            try
            {
                using (var baglanti = new NpgsqlConnection(baglantiString))
                {
                    baglanti.Open();

                    string sorgu = "SELECT * FROM filmler WHERE filmadi ILIKE @filmadi";

                    using (var komut = new NpgsqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@filmadi", "%" + filmadi + "%");

                        using (var adapter = new NpgsqlDataAdapter(komut))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("Aradığınız film bulunamadı.");
                                dataGridView1.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

    }
}
