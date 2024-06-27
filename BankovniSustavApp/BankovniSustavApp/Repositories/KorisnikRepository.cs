using BankovniSustavApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using BankovniSustavApp.DataAccess;
using MySqlX.XDevAPI.Common;
using System.Windows;

namespace BankovniSustavApp.Repositories
{
    public class KorisnikRepository : IGenericRepository<Korisnik>
    {
        private readonly DatabaseHelper _dbHelper;

        public KorisnikRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<IEnumerable<Korisnik>> GetAllAsync()
        {
            var korisnici = new List<Korisnik>();
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT * FROM korisnici", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                            int imeOrdinal = reader.GetOrdinal("Ime");
                            int prezimeOrdinal = reader.GetOrdinal("Prezime");
                            int emailOrdinal = reader.GetOrdinal("Email");
                            int lozinkaOrdinal = reader.GetOrdinal("Lozinka");
                            int datumRegistracijeOrdinal = reader.GetOrdinal("DatumRegistracije");
                            int statusOrdinal = reader.GetOrdinal("Status");

                            korisnici.Add(new Korisnik
                            {
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                Ime = reader.IsDBNull(imeOrdinal) ? null : reader.GetString(imeOrdinal),
                                Prezime = reader.IsDBNull(prezimeOrdinal) ? null : reader.GetString(prezimeOrdinal),
                                Email = reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                                Lozinka = reader.IsDBNull(lozinkaOrdinal) ? null : reader.GetString(lozinkaOrdinal),
                                DatumRegistracije = reader.GetDateTime(datumRegistracijeOrdinal),
                                Status = reader.IsDBNull(statusOrdinal) ? null : reader.GetString(statusOrdinal)
                            });
                        }
                    }
                }
            }
            return korisnici;
        }

        public async Task<Korisnik> GetByIdAsync(int id)
        {
            Korisnik korisnik = null;
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("SELECT * FROM korisnici WHERE KorisnikID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int korisnikIdOrdinal = reader.GetOrdinal("KorisnikID");
                            int imeOrdinal = reader.GetOrdinal("Ime");
                            int prezimeOrdinal = reader.GetOrdinal("Prezime");
                            int emailOrdinal = reader.GetOrdinal("Email");
                            int lozinkaOrdinal = reader.GetOrdinal("Lozinka");
                            int datumRegistracijeOrdinal = reader.GetOrdinal("DatumRegistracije");
                            int statusOrdinal = reader.GetOrdinal("Status");

                            korisnik = new Korisnik
                            {
                                KorisnikID = reader.GetInt32(korisnikIdOrdinal),
                                Ime = reader.IsDBNull(imeOrdinal) ? null : reader.GetString(imeOrdinal),
                                Prezime = reader.IsDBNull(prezimeOrdinal) ? null : reader.GetString(prezimeOrdinal),
                                Email = reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                                Lozinka = reader.IsDBNull(lozinkaOrdinal) ? null : reader.GetString(lozinkaOrdinal),
                                DatumRegistracije = reader.GetDateTime(datumRegistracijeOrdinal),
                                Status = reader.IsDBNull(statusOrdinal) ? null : reader.GetString(statusOrdinal)
                            };
                        }
                    }
                }
            }
            return korisnik;
        }

        public async Task<bool> AddAsync(Korisnik korisnik)
        {
            if (string.IsNullOrEmpty(korisnik.Ime))
            {
                return false;
            }
            string query = @"INSERT INTO korisnici 
                     (Ime, Prezime, Email, Lozinka, DatumRegistracije, Status) 
                     VALUES 
                     (@ime, @prezime, @email, @lozinka, @datumRegistracije, @status);";
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ime", korisnik.Ime);
                    command.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                    command.Parameters.AddWithValue("@email", korisnik.Email);
                    command.Parameters.AddWithValue("@lozinka", korisnik.Lozinka);
                    command.Parameters.AddWithValue("@datumRegistracije", korisnik.DatumRegistracije);
                    command.Parameters.AddWithValue("@status", korisnik.Status);

                    await connection.OpenAsync();

                    try
                    {
                        var result = await command.ExecuteNonQueryAsync();
                        return result == 1;
                    }
                    catch (MySqlException ex) when (ex.Number == 1062) // MySQL error code for duplicate entry
                    {
                        // Handle the duplicate entry case
                        MessageBox.Show("User already exists", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    catch (MySqlException ex)
                    {
                        // Handle other database-related errors
                        MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
        }


        public async Task<bool> UpdateAsync(Korisnik korisnik)
        {
            using (var connection = _dbHelper.CreateConnection())
            {
                using (var command = new MySqlCommand("UPDATE korisnici SET Ime = @ime, Prezime = @prezime, Email = @email, Lozinka = @lozinka, DatumRegistracije = @datumRegistracije, Status = @status WHERE KorisnikID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", korisnik.KorisnikID);
                    command.Parameters.AddWithValue("@ime", korisnik.Ime);
                    command.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                    command.Parameters.AddWithValue("@email", korisnik.Email);
                    command.Parameters.AddWithValue("@lozinka", korisnik.Lozinka);
                    command.Parameters.AddWithValue("@datumRegistracije", korisnik.DatumRegistracije);
                    command.Parameters.AddWithValue("@status", korisnik.Status);

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
                using (var command = new MySqlCommand("DELETE FROM korisnici WHERE KorisnikID = @id", connection))
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
