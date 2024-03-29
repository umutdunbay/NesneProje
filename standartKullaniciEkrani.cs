﻿using Npgsql;
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


                baglanti.Open();
                string sorgu5 = "select filmadi from filmler";
                NpgsqlDataAdapter fa = new NpgsqlDataAdapter(sorgu5, baglanti);
                DataTable dt = new DataTable();
                fa.Fill(dt);
                comboBox1.DisplayMember = "filmadi";
                comboBox1.ValueMember = "filmid";
                comboBox1.DataSource = dt;
                baglanti.Close();

                string sorgu3 = "SELECT k.kullaniciadi, f.filmadi, d.degerlendirmepuani, d.yorum " +
                 "FROM degerlendirmeler d " +
                 "JOIN kullanicilar k ON d.kullaniciid = k.kullaniciid " +
                 "JOIN filmler f ON d.filmid = f.filmid";
                NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sorgu3, baglanti);
                DataSet ds3 = new DataSet();
                da3.Fill(ds3);

                dataGridView3.DataSource = ds3.Tables[0];
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
            try
            {
                // PostgreSQL bağlantı nesnesi ve bağlantı bilgileri
                NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Database=Film Kütüphanesi;Username=postgres;Password=Vipedmap1");

                // Veri çekme sorgusu
                string sorgu = "SELECT * FROM filmler";

                // DataAdapter ve DataTable oluşturma
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dataTable = new DataTable();

                // DataTable'ı doldurma
                dataAdapter.Fill(dataTable);

                // DataGridView2'ye DataTable'ı ata
                dataGridView2.DataSource = dataTable;

                // Bağlantıyı kapat
                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            istatistikGoruntule ig = new istatistikGoruntule();
            ig.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // TextBox, NumericUpDown ve ComboBox'tan değerleri al
            string yorum = textBox7.Text;
            int degerlendirmePuani = (int)numericUpDown1.Value;
            DataRowView selectedRow = (DataRowView)comboBox1.SelectedItem;
            string filmAdi = selectedRow["filmadi"].ToString();
            string qkullaniciAdi = kullaniciAdi; // Burada kullanıcı adını almanız gerekiyor.


            using (var baglanti = new NpgsqlConnection(baglantiString))
            {
                // Kullanıcı ID'sini almak için "kullanicilar" tablosunu sorgula
                baglanti.Open();
                string kullaniciIdSorgu = "SELECT kullaniciid FROM kullanicilar WHERE kullaniciadi = @kullaniciadi";
                NpgsqlCommand kullaniciIdKomut = new NpgsqlCommand(kullaniciIdSorgu, baglanti);
                kullaniciIdKomut.Parameters.AddWithValue("@kullaniciadi", qkullaniciAdi);
                int kullaniciId = (int)kullaniciIdKomut.ExecuteScalar();
                baglanti.Close();

                // Film ID'sini almak için "filmler" tablosunu sorgula
                baglanti.Open();
                string filmIdSorgu = "SELECT filmid FROM filmler WHERE filmadi = @filmadi";
                NpgsqlCommand filmIdKomut = new NpgsqlCommand(filmIdSorgu, baglanti);
                filmIdKomut.Parameters.AddWithValue("@filmadi", filmAdi);
                int filmId = (int)filmIdKomut.ExecuteScalar();
                baglanti.Close();

                // "degerlendirmeler" tablosuna ekleme yap
                baglanti.Open();
                string degerlendirmeEkleSorgu = "INSERT INTO degerlendirmeler (kullaniciid, filmid, degerlendirmepuani, yorum) VALUES (@kullaniciid, @filmid, @degerlendirmepuani, @yorum)";
                NpgsqlCommand degerlendirmeEkleKomut = new NpgsqlCommand(degerlendirmeEkleSorgu, baglanti);
                degerlendirmeEkleKomut.Parameters.AddWithValue("@kullaniciid", kullaniciId);
                degerlendirmeEkleKomut.Parameters.AddWithValue("@filmid", filmId);
                degerlendirmeEkleKomut.Parameters.AddWithValue("@degerlendirmepuani", degerlendirmePuani);
                degerlendirmeEkleKomut.Parameters.AddWithValue("@yorum", yorum);
                degerlendirmeEkleKomut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Değerlendirme Ekleme İşlemi Başarılı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Değer ekledikten sonra kullanıcıya geri bildirim göstermek istiyorsanız, mesela TextBox'ı temizleyebilirsiniz.
                textBox7.Clear();
                numericUpDown1.Value = 0;
                comboBox1.SelectedIndex = -1;

                // Yeni değer ekledikten sonra DataGridView'ı güncelle (varsayılan olarak DataGridView1 olarak adlandırılmış)
                string sorgu3 = "SELECT k.kullaniciadi, f.filmadi, d.degerlendirmepuani, d.yorum " +
                "FROM degerlendirmeler d " +
                "JOIN kullanicilar k ON d.kullaniciid = k.kullaniciid " +
                "JOIN filmler f ON d.filmid = f.filmid";
                NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sorgu3, baglanti);
                DataSet ds3 = new DataSet();
                da3.Fill(ds3);

                dataGridView3.DataSource = ds3.Tables[0];
            }    
        }
    }
}
