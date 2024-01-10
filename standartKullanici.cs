using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesneProje
{
    public class StandartKullanici : Kullanicilar
    {
        public StandartKullanici(string _kullaniciAdi, string _sifre, string _ad, string _soyAd, string _tcNo, DateTime _dogumTarihi, string _cinsiyet, string _uyelikTuru) : base(_kullaniciAdi, _sifre, _ad, _soyAd, _tcNo, _dogumTarihi, _cinsiyet, _uyelikTuru)
        {
        }

        public override decimal uyelikUcretiHesapla()
        {
            return 100;
        }
    }
}
