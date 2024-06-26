using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;


namespace BankovniSustavApp.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<MySqlConnection> OpenNewConnectionAsync()
        {
            var connection = CreateConnection();
            await connection.OpenAsync();
            return connection;
        }
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();
                    // Optionally perform a simple retrieval to ensure connection is valid
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                // Handle exception
                // Logging the exception might be useful.
                Console.WriteLine(ex.Message);
                return false;
            }
            // No need for finally to close the connection, as 'using' statement handles it
        }
    }
}

