using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesneProje
{
    public class Filmler
    {
        public string filmAdi {  get; set; }
        public string yonetmen { get; set; }
        public string[] oyuncular { get; set; }
        public string tur { get; set; }
        public int yayinYili { get; set; }
        public decimal degerlendirmePuani { get; set; }

        public Filmler(string _filmAdi, string _yonetmen, string[] _oyuncular, string _tur, int _yayinYili, decimal _degerlendirmePuani)
        {
            filmAdi = _filmAdi;
            yonetmen = _yonetmen;
            oyuncular = _oyuncular;
            tur = _tur;
            yayinYili = _yayinYili;
            degerlendirmePuani = _degerlendirmePuani;
        }

    }
}
