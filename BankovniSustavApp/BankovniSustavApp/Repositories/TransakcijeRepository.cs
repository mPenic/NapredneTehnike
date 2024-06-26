using BankovniSustavApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using BankovniSustavApp.DataAccess;

namespace BankovniSustavApp.Repositories
{
    public class TransakcijeRepository : IGenericRepository<Transakcije>
    {
        private readonly DatabaseHelper _dbHelper;

        public TransakcijeRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<IEnumerable<Transakcije>> GetAllAsync()
        {
            var transakcijeList = new List<Transakcije>();
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT * FROM transakcije", connection))
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

                        while (await reader.ReadAsync())
                        {
                            transakcijeList.Add(new Transakcije
                            {
                                TransakcijaID = reader.GetInt32(transakcijaIdOrdinal),
                                RacunID = reader.GetInt32(racunIdOrdinal),
                                DatumVrijeme = reader.GetDateTime(datumVrijemeOrdinal),
                                Iznos = reader.GetDecimal(iznosOrdinal),
                                Vrsta = reader.GetString(vrstaOrdinal),
                                Opis = reader.IsDBNull(opisOrdinal) ? null : reader.GetString(opisOrdinal) // Opis might be null
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
                                Vrsta = reader.IsDBNull(vrstaOrdinal) ? null : reader.GetString(vrstaOrdinal),
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
                using (var command = new MySqlCommand(query, connection))
                {
                    // Parameterization
                    command.Parameters.AddWithValue("@RacunID", transakcija.RacunID);
                    command.Parameters.AddWithValue("@DatumVrijeme", transakcija.DatumVrijeme);
                    command.Parameters.AddWithValue("@Iznos", transakcija.Iznos);
                    command.Parameters.AddWithValue("@Vrsta", transakcija.Vrsta);
                    command.Parameters.AddWithValue("@Opis", transakcija.Opis);

                    await connection.OpenAsync();
                    int result = await command.ExecuteNonQueryAsync();

                    return result > 0;
                }
            }
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
       

                    return result > 0;
                }
            }
        }
    }
}
