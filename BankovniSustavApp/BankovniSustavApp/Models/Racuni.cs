using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace BankovniSustavApp.Models
{
    public class Racuni
    {
        public int RacunID { get; set; }
        public int KorisnikID { get; set; }
        public string BrojRacuna { get; set; }
        public decimal Stanje { get; set; }
        public DateTime DatumOtvaranja { get; set; }
        public string Vrsta { get; set; }
        public string Valuta { get; set; }
    }
}

