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
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

