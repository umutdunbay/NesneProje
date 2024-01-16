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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NesneProje
{
    public partial class anaEkran : Form
    {
        public anaEkran()
        {
            InitializeComponent();
           
        }

        private void anaEkran_Load(object sender, EventArgs e)
        {
            string sorgu = "select * from kullanicilar";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            

            string sorgu2 = "select * from yoneticiler";
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2);
            
        }

       

        private void girisEkraniBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server = localhost; port=5432; Database=Film Kütüphanesi; user ID = postgres;password=Vipedmap1");
        private void kullaniciEkraniBtn_Click(object sender, EventArgs e)
        {

            tabControl1.SelectedTab = tabPage2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            


            Kullanicilar kullanici;
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || comboBox1.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Lütfen Tüm Alanları Doldurunuz");
            }
            else
            {
                if (radioButton3.Checked)
                {
                    kullanici = new StandartKullanici(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, Convert.ToDateTime(textBox6.Text), comboBox1.SelectedItem.ToString(), "Standart");

                    baglanti.Open();
                    NpgsqlCommand komut1 = new NpgsqlCommand("insert into kullanicilar (kullaniciadi, sifre, isim, soyisim,tcno, dogumtarihi," +
                                                            " cinsiyet, uyelikturu) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", baglanti);


                    komut1.Parameters.AddWithValue("@p1", kullanici.kullaniciAdi);
                    komut1.Parameters.AddWithValue("@p2", kullanici.sifre);
                    komut1.Parameters.AddWithValue("@p3", kullanici.ad);
                    komut1.Parameters.AddWithValue("@p4", kullanici.soyAd);
                    komut1.Parameters.AddWithValue("@p5", kullanici.tcNo);
                    komut1.Parameters.AddWithValue("@p6", kullanici.dogumTarihi);
                    komut1.Parameters.AddWithValue("@p7", kullanici.cinsiyet);
                    komut1.Parameters.AddWithValue("@p8", kullanici.uyelikTuru);
                    komut1.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Kullanıcı Ekleme İşlemi Başarılı");

                    string sorgu = "select * from kullanicilar";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                    DataSet ds = new DataSet();
                    da.Fill(ds);


                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    comboBox1.SelectedItem = null;
                    radioButton3.Checked = false;
                }
                else if (radioButton4.Checked)
                {
                    kullanici = new PremiumKullanici(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, Convert.ToDateTime(textBox6.Text), comboBox1.SelectedItem.ToString(), "Premium");

                    baglanti.Open();
                    NpgsqlCommand komut1 = new NpgsqlCommand("insert into kullanicilar (kullaniciadi, sifre, isim, soyisim,tcno, dogumtarihi," +
                                                            " cinsiyet, uyelikturu) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", baglanti);


                    komut1.Parameters.AddWithValue("@p1", kullanici.kullaniciAdi);
                    komut1.Parameters.AddWithValue("@p2", kullanici.sifre);
                    komut1.Parameters.AddWithValue("@p3", kullanici.ad);
                    komut1.Parameters.AddWithValue("@p4", kullanici.soyAd);
                    komut1.Parameters.AddWithValue("@p5", kullanici.tcNo);
                    komut1.Parameters.AddWithValue("@p6", kullanici.dogumTarihi);
                    komut1.Parameters.AddWithValue("@p7", kullanici.cinsiyet);
                    komut1.Parameters.AddWithValue("@p8", kullanici.uyelikTuru);
                    komut1.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Kullanıcı Ekleme İşlemi Başarılı");

                    string sorgu = "select * from kullanicilar";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                    DataSet ds = new DataSet();
                    da.Fill(ds);


                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    comboBox1.SelectedItem = null;
                    radioButton4.Checked = false;
                }
                else
                {
                    MessageBox.Show("Lütfen Kullanıcı Türünü Seçiniz");
                }
            }
        }

      
       

        private void yoneticiEkraniBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }



        private void yoneticiKayitTamamlaBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox'ların boş olup olmadığını kontrol et
                if (string.IsNullOrWhiteSpace(textBox13.Text) || string.IsNullOrWhiteSpace(textBox14.Text))
                {
                    MessageBox.Show("Lütfen kullanıcı adı ve şifreyi doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Yonetici yonetici = new Yonetici(textBox13.Text, textBox14.Text);

                baglanti.Open();
                NpgsqlCommand komut1 = new NpgsqlCommand("insert into yoneticiler (ykullaniciadi, ysifre) values (@p1, @p2)", baglanti);

                komut1.Parameters.AddWithValue("@p1", yonetici.kullaniciAdi);
                komut1.Parameters.AddWithValue("@p2", yonetici.sifre);
                komut1.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Yönetici Ekleme İşlemi Başarılı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Yönetici ekledikten sonra veritabanındaki güncel liste için DataGridView'ı güncelle
                string sorgu2 = "select * from yoneticiler";
                NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sorgu2, baglanti);
                DataSet ds2 = new DataSet();
                da2.Fill(ds2);

                textBox13.Clear();
                textBox14.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void kullaniciGirisBtn_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = kullaniciAdiText.Text;
            string sifre = kullaniciSifreText.Text;

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string uyelikTuru = radioButton1.Checked ? "Standart" : (radioButton2.Checked ? "Premium" : "");

            if (string.IsNullOrEmpty(uyelikTuru))
            {
                MessageBox.Show("Üyelik türü seçilmelidir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (KullaniciGirisKontrolu(kullaniciAdi, sifre, uyelikTuru))
            {
                if (uyelikTuru == "Standart")
                {
                    standartKullaniciEkrani standartForm = new standartKullaniciEkrani(kullaniciAdi);
                    this.Hide();
                    standartForm.Show();
                    
                }
                else if (uyelikTuru == "Premium")
                {
                    premiumKullaniciEkrani premiumForm = new premiumKullaniciEkrani(kullaniciAdi);
                    this.Hide();
                    premiumForm.Show();
                    
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı, şifre veya üyelik türü hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private bool KullaniciGirisKontrolu(string kullaniciAdi, string sifre, string uyelikTuru)
        {
            using (var komut = new NpgsqlCommand("SELECT COUNT(*) FROM kullanicilar WHERE kullaniciadi = @kullaniciadi AND sifre = @sifre AND uyelikturu = @uyelikturu", baglanti))
            {
                komut.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);
                komut.Parameters.AddWithValue("@sifre", sifre);
                komut.Parameters.AddWithValue("@uyelikturu", uyelikTuru);

                baglanti.Open();
                int kullaniciSayisi = Convert.ToInt32(komut.ExecuteScalar());
                baglanti.Close();

                return kullaniciSayisi > 0;
            }
        }

        private void yoneticiGirisBtn_Click(object sender, EventArgs e)
        {
            string yoneticiAdi = yoneticiAdiText.Text;
            string sifre = yoneticiSifreText.Text;

            if (string.IsNullOrEmpty(yoneticiAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (YoneticiGirisKontrolu(yoneticiAdi, sifre))
            {
                yoneticiEkrani yoneticiForm = new yoneticiEkrani();
                yoneticiForm.Show();
                this.Hide();

                
                anaEkran anaEkranForm = Application.OpenForms["AnaEkran"] as anaEkran;
                if (anaEkranForm != null)
                {
                    anaEkranForm.Hide();
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool YoneticiGirisKontrolu(string yoneticiAdi, string sifre)
        {
            using (var komut = new NpgsqlCommand("SELECT COUNT(*) FROM yoneticiler WHERE ykullaniciadi = @ykullaniciadi AND ysifre = @ysifre", baglanti))
            {
                komut.Parameters.AddWithValue("@ykullaniciadi", yoneticiAdi);
                komut.Parameters.AddWithValue("@ysifre", sifre);

                baglanti.Open();
                int yoneticiSayisi = Convert.ToInt32(komut.ExecuteScalar());
                baglanti.Close();

                return yoneticiSayisi > 0;
            }
        }

        private void kullaniciKayitBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void yoneticiKayitBtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
    }
}
