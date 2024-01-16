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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NesneProje
{
    public partial class BilgiGuncelleme : Form
    {
        private string baglantiString = "server = localhost; port=5432; Database=Film Kütüphanesi; user ID = postgres;password=Vipedmap1";

        string kAdi;

        public BilgiGuncelleme(string _kAdi)
        {
            InitializeComponent();
            kAdi = _kAdi;
        }

        private void BilgiGuncelleme_Load(object sender, EventArgs e)
        {
            using (var baglanti = new NpgsqlConnection(baglantiString))
            {
                baglanti.Open();

                string sorgu = "SELECT isim, soyisim, tcno, dogumtarihi, cinsiyet FROM kullanicilar WHERE kullaniciadi = @kullaniciadi";

                using (var komut = new NpgsqlCommand(sorgu, baglanti))
                {
                    // Burada uygun kullaniciadi değerini belirleyerek sorguyu çalıştırabilirsiniz.
                    komut.Parameters.AddWithValue("@kullaniciadi", kAdi);

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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan değerleri TextBox'lar üzerinden al
            string yeniIsim = textBox1.Text;
            string yeniSoyisim = textBox2.Text;
            string yeniTcNo = textBox3.Text;
            DateTime yeniDogumTarihi = Convert.ToDateTime( textBox4.Text);
            string yeniCinsiyet = textBox5.Text;

            // Kullanıcı adını belirle (bu değeri uygun bir şekilde almalısınız)
            string kullaniciAdi = kAdi;

            try
            {
                // Veritabanı bağlantısı
                string baglantiString = "Host=localhost;Port=5432;Database=Film Kütüphanesi;user ID=postgres;password=Vipedmap1";

                using (NpgsqlConnection baglanti = new NpgsqlConnection(baglantiString))
                {
                    baglanti.Open();

                    // Kullanıcı bilgilerini güncelle
                    string sorgu = "UPDATE kullanicilar SET isim = @p1, soyisim = @p2, tcno = @p3, dogumtarihi = @p4, cinsiyet = @p5 WHERE kullaniciadi = @p6";

                    using (NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@p1", yeniIsim);
                        komut.Parameters.AddWithValue("@p2", yeniSoyisim);
                        komut.Parameters.AddWithValue("@p3", yeniTcNo);
                        komut.Parameters.AddWithValue("@p4", yeniDogumTarihi);
                        komut.Parameters.AddWithValue("@p5", yeniCinsiyet);
                        komut.Parameters.AddWithValue("@p6", kullaniciAdi);

                        int etkilenenSatirSayisi = komut.ExecuteNonQuery();

                        if (etkilenenSatirSayisi > 0)
                        {
                            MessageBox.Show("Kullanıcı bilgileri başarıyla güncellendi.");
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bilgileri güncellenirken bir hata oluştu.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }

            this.Hide();
        }
    }
}
