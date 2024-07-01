using BankovniSustavApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankovniSustavApp.DataAccess;
using BankovniSustavApp.Helpers;
using System;

namespace BankovniSustavApp.Repositories
{
    public class RacuniRepository : IAccountRepository<Racuni>
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly LogoviDataAccess _logoviDataAccess;

        public RacuniRepository(DatabaseHelper dbHelper, LogoviDataAccess logoviDataAccess)
        {
            _dbHelper = dbHelper;
            _logoviDataAccess = logoviDataAccess;
        }

        public async Task<IEnumerable<Racuni>> GetByKorisnikIdAsync(int korisnikId)
        {
            var accountsList = new List<Racuni>();
            using (var connection = _dbHelper.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM racuni WHERE KorisnikID = @KorisnikID", connection))
                {
                    command.Parameters.AddWithValue("@KorisnikID", korisnikId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int racunIdOrdinal = reader.GetOrdinal("RacunID");
                        int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                        int brojRacunaOrdinal = reader.GetOrdinal("BrojRacuna");
                        int stanjeOrdinal = reader.GetOrdinal("Stanje");
                        int datumOtvaranjaOrdinal = reader.GetOrdinal("DatumOtvaranja");
                        int vrstaOrdinal = reader.GetOrdinal("Vrsta");
                        int valutaOrdinal = reader.GetOrdinal("Valuta");

                        while (await reader.ReadAsync())
                        {
                            accountsList.Add(new Racuni
                            {
                                RacunID = reader.GetInt32(racunIdOrdinal),
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                BrojRacuna = reader.GetString(brojRacunaOrdinal),
                                Stanje = reader.GetDecimal(stanjeOrdinal),
                                DatumOtvaranja = reader.GetDateTime(datumOtvaranjaOrdinal),
                                Vrsta = reader.GetString(vrstaOrdinal),
                                Valuta = reader.GetString(valutaOrdinal)
                            });
                        }
                    }
                }
            }
            return accountsList;
        }

        public async Task<IEnumerable<Racuni>> GetAllAsync()
        {
            var racuniList = new List<Racuni>();
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT * FROM racuni", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int racunIdOrdinal = reader.GetOrdinal("RacunID");
                        int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                        int brojRacunaOrdinal = reader.GetOrdinal("BrojRacuna");
                        int stanjeOrdinal = reader.GetOrdinal("Stanje");
                        int datumOtvaranjaOrdinal = reader.GetOrdinal("DatumOtvaranja");
                        int vrstaOrdinal = reader.GetOrdinal("Vrsta");
                        int valutaOrdinal = reader.GetOrdinal("Valuta");

                        while (await reader.ReadAsync())
                        {
                            racuniList.Add(new Racuni
                            {
                                RacunID = reader.GetInt32(racunIdOrdinal),
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                BrojRacuna = reader.GetString(brojRacunaOrdinal),
                                Stanje = reader.GetDecimal(stanjeOrdinal),
                                DatumOtvaranja = reader.GetDateTime(datumOtvaranjaOrdinal),
                                Vrsta = reader.GetString(vrstaOrdinal),
                                Valuta = reader.GetString(valutaOrdinal)
                            });
                        }
                    }
                }
            }
            return racuniList;
        }

        public async Task<Racuni> GetByIdAsync(int id)
        {
            Racuni racun = null;
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = "SELECT * FROM racuni WHERE RacunID = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int racunIdOrdinal = reader.GetOrdinal("RacunID");
                        int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                        int brojRacunaOrdinal = reader.GetOrdinal("BrojRacuna");
                        int stanjeOrdinal = reader.GetOrdinal("Stanje");
                        int datumOtvaranjaOrdinal = reader.GetOrdinal("DatumOtvaranja");
                        int vrstaOrdinal = reader.GetOrdinal("Vrsta");
                        int valutaOrdinal = reader.GetOrdinal("Valuta");

                        if (await reader.ReadAsync())
                        {
                            racun = new Racuni
                            {
                                RacunID = reader.GetInt32(racunIdOrdinal),
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                BrojRacuna = reader.GetString(brojRacunaOrdinal),
                                Stanje = reader.GetDecimal(stanjeOrdinal),
                                DatumOtvaranja = reader.GetDateTime(datumOtvaranjaOrdinal),
                                Vrsta = reader.GetString(vrstaOrdinal),
                                Valuta = reader.GetString(valutaOrdinal)
                            };
                        }
                    }
                }
            }
            return racun;
        }

        public async Task<bool> AddAsync(Racuni racun)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = @"INSERT INTO racuni (KorisnikID, BrojRacuna, Stanje, DatumOtvaranja, Vrsta, Valuta) 
                              VALUES (@KorisnikID, @BrojRacuna, @Stanje, @DatumOtvaranja, @Vrsta, @Valuta)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KorisnikID", racun.KorisnikID);
                    command.Parameters.AddWithValue("@BrojRacuna", racun.BrojRacuna);
                    command.Parameters.AddWithValue("@Stanje", racun.Stanje);
                    command.Parameters.AddWithValue("@DatumOtvaranja", racun.DatumOtvaranja);
                    command.Parameters.AddWithValue("@Vrsta", racun.Vrsta);
                    command.Parameters.AddWithValue("@Valuta", racun.Valuta);

                    await connection.OpenAsync();
                    int result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        AddLog("Added account");
                    }

                    return result > 0;
                }
            }
        }

        public async Task<bool> UpdateAsync(Racuni racun)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = "UPDATE racuni SET KorisnikID = @KorisnikID, BrojRacuna = @BrojRacuna, Stanje = @Stanje, DatumOtvaranja = @DatumOtvaranja, Vrsta = @Vrsta, Valuta = @Valuta WHERE RacunID = @RacunID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RacunID", racun.RacunID);
                    command.Parameters.AddWithValue("@KorisnikID", racun.KorisnikID);
                    command.Parameters.AddWithValue("@BrojRacuna", racun.BrojRacuna);
                    command.Parameters.AddWithValue("@Stanje", racun.Stanje);
                    command.Parameters.AddWithValue("@DatumOtvaranja", racun.DatumOtvaranja);
                    command.Parameters.AddWithValue("@Vrsta", racun.Vrsta);
                    command.Parameters.AddWithValue("@Valuta", racun.Valuta);

                    await connection.OpenAsync();
                    int result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        AddLog("Updated account");
                    }

                    return result > 0;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = "DELETE FROM racuni WHERE RacunID = @RacunID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RacunID", id);

                    await connection.OpenAsync();
                    int result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        AddLog("Deleted account");
                    }

                    return result > 0;
                }
            }
        }

        private void AddLog(string action)
        {
            var log = new Logovi
            {
                KorisnikID = SessionManager.CurrentKorisnikId,
                UserEmail = SessionManager.CurrentUserEmail, // Assuming you store this in SessionManager
                DatumVrijeme = DateTime.Now,
                Opis = action,
                Operation = action
            };
            _logoviDataAccess.AddAccountLog(log);
        }
    }
}
