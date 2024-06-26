using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BankovniSustavApp.Models
{
    public class Transakcije
    {
        public int TransakcijaID { get; set; }
        public int RacunID { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public decimal Iznos { get; set; }
        public string Vrsta { get; set; }
        public string Opis { get; set; }
    }
}

