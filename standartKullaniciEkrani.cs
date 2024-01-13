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
        
        private string baglantiString = "Host=localhost;Port=5432;Database=Film Kütüphanesi;user ID=postgres;password=Vipedmap1";

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

        private void bilgiGuncelleBtn_Click(object sender, EventArgs e)
        {
            
        }

    }
}
