using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankovniSustavApp.Models;
using MySql.Data.MySqlClient;

namespace BankovniSustavApp.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
    public interface IAccountRepository<T> : IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetByKorisnikIdAsync(int korisnikId);
    }
}
