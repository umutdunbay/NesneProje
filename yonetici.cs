using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesneProje
{
    public class Yonetici
    {
        public string kullaniciAdi { get; set; }
        public string sifre { get; set; }

        public Yonetici(string _kullaniciAdi, string _sifre) 
        {
            kullaniciAdi = _kullaniciAdi;
            sifre = _sifre;
        }
    }
}
