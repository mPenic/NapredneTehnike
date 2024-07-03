using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace BankovniSustavApp.Models
{
    public class Logovi
    {
        public int LogID { get; set; }
        public int? KorisnikID { get; set; }
        public string UserEmail { get; set; }
        public DateTime DatumVrijeme { get; set; }
        public string Opis { get; set; }
        public string Operation { get; set; }
    }
}
