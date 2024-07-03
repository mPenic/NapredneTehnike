using BankovniSustavApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankovniSustavApp.DataAccess;
using BankovniSustavApp.Helpers;
using System;

namespace BankovniSustavApp.Repositories
{
    public class TransakcijeRepository : IGenericRepository<Transakcije>
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly LogoviDataAccess _logoviDataAccess;

        public TransakcijeRepository(DatabaseHelper dbHelper, LogoviDataAccess logoviDataAccess)
        {
            _dbHelper = dbHelper;
            _logoviDataAccess = logoviDataAccess;
        }

        public async Task<IEnumerable<Transakcije>> GetAllAsync()
        {
            var transakcijeList = new List<Transakcije>();
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT t.*, r.BrojRacuna FROM transakcije t JOIN racuni r ON t.RacunID = r.RacunID", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int transakcijaIdOrdinal = reader.GetOrdinal("TransakcijaID");
                        int racunIdOrdinal = reader.GetOrdinal("RacunID");
                        int datumVrijemeOrdinal = reader.GetOrdinal("DatumVrijeme");
                        int iznosOrdinal = reader.GetOrdinal("Iznos");
                        int vrstaOrdinal = reader.GetOrdinal("Vrsta");
                        int opisOrdinal = reader.GetOrdinal("Opis");
                        int brojRacunaOrdinal = reader.GetOrdinal("BrojRacuna");

                        while (await reader.ReadAsync())
                        {
                            transakcijeList.Add(new Transakcije
                            {
                                TransakcijaID = reader.GetInt32(transakcijaIdOrdinal),
                                RacunID = reader.GetInt32(racunIdOrdinal),
                                DatumVrijeme = reader.GetDateTime(datumVrijemeOrdinal),
                                Iznos = reader.GetDecimal(iznosOrdinal),
                                Vrsta = reader.GetString(vrstaOrdinal),
                                Opis = reader.IsDBNull(opisOrdinal) ? null : reader.GetString(opisOrdinal),
                                BrojRacuna = reader.GetString(brojRacunaOrdinal)
                            });
                        }
                    }
                }
            }
            return transakcijeList;
        }

        public async Task<Transakcije> GetByIdAsync(int id)
        {
            Transakcije transakcija = null;
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = "SELECT * FROM transakcije WHERE TransakcijaID = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        int transakcijaIdOrdinal = reader.GetOrdinal("TransakcijaID");
                        int racunIdOrdinal = reader.GetOrdinal("RacunID");
                        int datumVrijemeOrdinal = reader.GetOrdinal("DatumVrijeme");
                        int iznosOrdinal = reader.GetOrdinal("Iznos");
                        int vrstaOrdinal = reader.GetOrdinal("Vrsta");
                        int opisOrdinal = reader.GetOrdinal("Opis");

                        if (await reader.ReadAsync())
                        {
                            transakcija = new Transakcije
                            {
                                TransakcijaID = reader.GetInt32(transakcijaIdOrdinal),
                                RacunID = reader.GetInt32(racunIdOrdinal),
                                DatumVrijeme = reader.GetDateTime(datumVrijemeOrdinal),
                                Iznos = reader.GetDecimal(iznosOrdinal),
                                Vrsta = reader.GetString(vrstaOrdinal),
                                Opis = reader.IsDBNull(opisOrdinal) ? null : reader.GetString(opisOrdinal)
                            };
                        }
                    }
                }
            }
            return transakcija;
        }

        public async Task<bool> AddAsync(Transakcije transakcija)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = @"INSERT INTO transakcije (RacunID, DatumVrijeme, Iznos, Vrsta, Opis) 
                      VALUES (@RacunID, @DatumVrijeme, @Iznos, @Vrsta, @Opis)";
                var updateQuery = @"UPDATE racuni SET Stanje = Stanje + @Iznos WHERE RacunID = @RacunID";

                using (var command = new MySqlCommand(query, connection))
                using (var updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@RacunID", transakcija.RacunID);
                    command.Parameters.AddWithValue("@DatumVrijeme", transakcija.DatumVrijeme);
                    command.Parameters.AddWithValue("@Iznos", transakcija.Iznos);
                    command.Parameters.AddWithValue("@Vrsta", transakcija.Vrsta);
                    command.Parameters.AddWithValue("@Opis", transakcija.Opis);

                    updateCommand.Parameters.AddWithValue("@RacunID", transakcija.RacunID);
                    updateCommand.Parameters.AddWithValue("@Iznos", transakcija.Iznos);

                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        command.Transaction = transaction;
                        updateCommand.Transaction = transaction;

                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            int updateResult = await updateCommand.ExecuteNonQueryAsync();
                            if (updateResult > 0)
                            {
                                await transaction.CommitAsync();
                                AddLog("Added transaction");
                                return true;
                            }
                        }
                        await transaction.RollbackAsync();
                    }
                }
            }
            return false;
        }



        public async Task<bool> UpdateAsync(Transakcije transakcija)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = @"UPDATE transakcije 
                          SET RacunID = @RacunID, 
                              DatumVrijeme = @DatumVrijeme, 
                              Iznos = @Iznos, 
                              Vrsta = @Vrsta, 
                              Opis = @Opis 
                          WHERE TransakcijaID = @TransakcijaID";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransakcijaID", transakcija.TransakcijaID);
                    command.Parameters.AddWithValue("@RacunID", transakcija.RacunID);
                    command.Parameters.AddWithValue("@DatumVrijeme", transakcija.DatumVrijeme);
                    command.Parameters.AddWithValue("@Iznos", transakcija.Iznos);
                    command.Parameters.AddWithValue("@Vrsta", transakcija.Vrsta);
                    command.Parameters.AddWithValue("@Opis", transakcija.Opis);

                    await connection.OpenAsync();
                    int result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        AddLog("Updated transaction");
                    }

                    return result > 0;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = "DELETE FROM transakcije WHERE TransakcijaID = @TransakcijaID";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransakcijaID", id);

                    await connection.OpenAsync();
                    int result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        AddLog("Deleted transaction");
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
                UserEmail = SessionManager.CurrentUserEmail,
                DatumVrijeme = DateTime.Now,
                Opis = action,
                Operation = action
            };
            _logoviDataAccess.AddTransactionLog(log);
        }
    }
}
