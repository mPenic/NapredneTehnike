using BankovniSustavApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using BankovniSustavApp.DataAccess;

namespace BankovniSustavApp.Repositories
{
    public class LogoviRepository : IGenericRepository<Logovi>
    {
        private readonly DatabaseHelper _dbHelper;

        public LogoviRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<IEnumerable<Logovi>> GetAllAsync()
        {
            var logoviList = new List<Logovi>();
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT * FROM logovi", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int logIdOrdinal = reader.GetOrdinal("LogID");
                            int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                            int datumVrijemeOrdinal = reader.GetOrdinal("DatumVrijeme");
                            int opisOrdinal = reader.GetOrdinal("Opis");

                            logoviList.Add(new Logovi
                            {
                                LogID = reader.GetInt32(logIdOrdinal),
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                DatumVrijeme = reader.GetDateTime(datumVrijemeOrdinal),
                                Opis = reader.GetString(opisOrdinal)
                            });
                        }
                    }
        
                }
            }
            return logoviList;
        }

        public async Task<Logovi> GetByIdAsync(int id)
        {
            Logovi log = null;
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT * FROM logovi WHERE LogID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int logIdOrdinal = reader.GetOrdinal("LogID");
                            int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                            int datumVrijemeOrdinal = reader.GetOrdinal("DatumVrijeme");
                            int opisOrdinal = reader.GetOrdinal("Opis");

                            log = new Logovi
                            {
                                LogID = reader.GetInt32(logIdOrdinal),
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                DatumVrijeme = reader.GetDateTime(datumVrijemeOrdinal),
                                Opis = reader.GetString(opisOrdinal)
                            };
                        }
                    }
                }
            }
            return log;
        }
        public async Task<bool> AddAsync(Logovi log)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                var query = "INSERT INTO logovi (KorisnikID, DatumVrijeme, Opis) VALUES (@KorisnikID, @DatumVrijeme, @Opis)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KorisnikID", log.KorisnikID);
                    command.Parameters.AddWithValue("@DatumVrijeme", log.DatumVrijeme);
                    command.Parameters.AddWithValue("@Opis", log.Opis);

                    await connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();
                 

                    return result > 0;
                }
            }
        }

        public async Task<bool> UpdateAsync(Logovi log)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("UPDATE logovi SET KorisnikID = @KorisnikID, DatumVrijeme = @DatumVrijeme, Opis = @Opis WHERE LogID = @LogID", connection))
                {
                    command.Parameters.AddWithValue("@LogID", log.LogID);
                    command.Parameters.AddWithValue("@KorisnikID", log.KorisnikID);
                    command.Parameters.AddWithValue("@DatumVrijeme", log.DatumVrijeme);
                    command.Parameters.AddWithValue("@Opis", log.Opis);

                    await connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();
              

                    return result > 0;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("DELETE FROM logovi WHERE LogID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    await connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();
                

                    return result > 0;
                }
            }
        }
    }
}
