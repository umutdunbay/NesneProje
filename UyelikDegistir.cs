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
    public partial class UyelikDegistir : Form
    {
        string kullaniciAdi;

        
        public UyelikDegistir(string _kAdi)
        {
            InitializeComponent();
            kullaniciAdi = _kAdi;
        }

        private void UyelikDegistir_Load(object sender, EventArgs e)
        {
            string ykullaniciAdi = kullaniciAdi;

            try
            {
                // Veritabanı bağlantısı
                string baglantiString = "server = localhost; port=5432; Database=Film Kütüphanesi; user ID = postgres;password=Vipedmap1";

                using (NpgsqlConnection baglanti = new NpgsqlConnection(baglantiString))
                {
                    baglanti.Open();

                    // Kullanıcıya ait uyelikturu, uyelikUcreti ve diger bilgileri al
                    string sorgu = "SELECT uyelikturu FROM kullanicilar WHERE kullaniciadi = @kullaniciAdi";

                    using (NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@kullaniciAdi", ykullaniciAdi);

                        object uyelikTuru = komut.ExecuteScalar();

                        if (uyelikTuru != null)
                        {
                            label5.Text = uyelikTuru.ToString();

                            // Kullanıcı türüne göre işlemleri gerçekleştir
                            if (uyelikTuru.ToString() == "Standart")
                            {
                                label6.Text = new StandartKullanici("kullaniciAdi", "sifre", "ad", "soyAd", "tcNo", DateTime.Now, "Erkek", "Standart").uyelikUcretiHesapla().ToString(); 
                                label7.Text = "Premium";
                                label8.Text = new PremiumKullanici("kullaniciAdi", "sifre", "ad", "soyAd", "tcNo", DateTime.Now, "Erkek", "Premium").uyelikUcretiHesapla().ToString();
                            }
                            else if (uyelikTuru.ToString() == "Premium")
                            {
                                label6.Text = new PremiumKullanici("kullaniciAdi", "sifre", "ad", "soyAd", "tcNo", DateTime.Now, "Erkek", "Premium").uyelikUcretiHesapla().ToString();
                                label7.Text = "Standart";
                                label8.Text = new StandartKullanici("kullaniciAdi", "sifre", "ad", "soyAd", "tcNo", DateTime.Now, "Erkek", "Standart").uyelikUcretiHesapla().ToString();
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bulunamadı.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Emin misiniz? Bu işlem geri alınamaz.", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Kullanıcı Onayla butonuna bastıysa
            if (result == DialogResult.OK)
            {
                // Kullanıcı bilgilerini veritabanından al
                // Burada uyelikturu'yu alıp değiştirme işlemini gerçekleştir

                // Örnek:
                string ykullaniciAdi = kullaniciAdi; // Kullanıcı adını gerçek değerle değiştir
                string yeniUyelikTuru = "";

                // Uyelikturu'yu veritabanından al
                string mevcutUyelikTuru = GetUyelikTuruFromDatabase(ykullaniciAdi);

                // Kullanıcının mevcut uyelikturu'na göre yeni uyelikturu'yu belirle
                if (mevcutUyelikTuru == "Standart")
                {
                    yeniUyelikTuru = "Premium";
                }
                else if (mevcutUyelikTuru == "Premium")
                {
                    yeniUyelikTuru = "Standart";
                }

                // Yeni uyelikturu'yu veritabanına kaydet
                UpdateUyelikTuruInDatabase(ykullaniciAdi, yeniUyelikTuru);
                // Şu anki formu kapat
                this.Hide();
            }
            // Kullanıcı İptal butonuna bastıysa
            else if (result == DialogResult.Cancel)
            {
                // Herhangi bir işlem yapma, iptal
            }

        }

        private string GetUyelikTuruFromDatabase(string kullaniciAdi)
        {
            string uyelikTuru = "";

            // PostgreSQL sorgusu
            string sorgu = "SELECT uyelikturu FROM kullanicilar WHERE kullaniciadi = @kullaniciadi";
            string baglantiString = "Host=localhost;Port=5432;Database=Film Kütüphanesi;user ID=postgres;password=Vipedmap1";

            using (NpgsqlConnection baglanti = new NpgsqlConnection(baglantiString))
            {
                baglanti.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(sorgu, baglanti))
                {
                    command.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            uyelikTuru = reader["uyelikturu"].ToString();
                        }
                    }
                }
            }

            return uyelikTuru;
        }

        // Bu metod, kullanıcının uyelikturu'yu veritabanında günceller
        private void UpdateUyelikTuruInDatabase(string kullaniciAdi, string yeniUyelikTuru)
        {
            // PostgreSQL sorgusu
            string sorgu = "UPDATE kullanicilar SET uyelikturu = @uyelikturu WHERE kullaniciadi = @kullaniciadi";
            string baglantiString = "Host=localhost;Port=5432;Database=Film Kütüphanesi;user ID=postgres;password=Vipedmap1";

            using (NpgsqlConnection baglanti = new NpgsqlConnection(baglantiString))
            {
                baglanti.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(sorgu, baglanti))
                {
                    command.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);
                    command.Parameters.AddWithValue("@uyelikturu", yeniUyelikTuru);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
