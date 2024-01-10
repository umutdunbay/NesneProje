using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NesneProje
{
    public abstract class Kullanicilar
    {
        public string kullaniciAdi { get; set; }
        public string sifre { get; set; }
        public string ad { get; set; }
        public string soyAd { get; set; }
        public string tcNo { get; set; }
        public DateTime dogumTarihi { get; set; }
        public string cinsiyet { get; set; }
        public string uyelikTuru { get; set; }
     

        public Kullanicilar(string _kullaniciAdi, string _sifre, string _ad, string _soyAd, string _tcNo, DateTime _dogumTarihi, string _cinsiyet, string _uyelikTuru) 
        {
            kullaniciAdi = _kullaniciAdi;
            sifre = _sifre; 
            ad = _ad;
            soyAd = _soyAd;
            tcNo = _tcNo;
            dogumTarihi = _dogumTarihi;
            cinsiyet = _cinsiyet;
            uyelikTuru = _uyelikTuru;
        }

        public abstract decimal uyelikUcretiHesapla();
    }

    

    

}
