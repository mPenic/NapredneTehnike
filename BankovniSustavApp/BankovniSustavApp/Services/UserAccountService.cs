using BankovniSustavApp.DataAccess;
using BankovniSustavApp.Helpers;
using BankovniSustavApp.Models;
using BankovniSustavApp.Repositories;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BankovniSustavApp.Services
{
    public interface IUserAccountService
    {
        Task<bool> RegisterUserAsync(Korisnik newUser);
        Task<bool> UpdateUserProfileAsync(Korisnik user);
        Task<Korisnik> GetUserByIdAsync(int userId);
        Task<Korisnik> GetUserByEmailAsync(string email);
        Task<int> GetKorisnikIdByEmailAsync(string email);
        Task<Korisnik> AuthenticateUserAsync(string email, string password);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IGenericRepository<Korisnik> _userRepository;
        private const string SaltFilePath = "../../Data/UserSalts.xml";

        public UserAccountService(IGenericRepository<Korisnik> userRepository)
        {
            _userRepository = userRepository;
            var keys = KeyManager.LoadKeys();
            if (keys == null)
            {
                keys = KeyManager.GenerateRsaKeys();
                KeyManager.SaveKeys(keys);
            }
        }

        public async Task<bool> RegisterUserAsync(Korisnik newUser)
        {
            var (hash, salt) = HashPassword(newUser.Lozinka);
            newUser.Lozinka = hash; // Store combined hash
            var result = await _userRepository.AddAsync(newUser);
            if (result)
            {
                StoreUserSalt(newUser.Email, salt); // Store salt separately
            }
            return result;
        }

        public async Task<bool> UpdateUserProfileAsync(Korisnik user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<Korisnik> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<Korisnik> GetUserByEmailAsync(string email)
        {
            var korisnik = (await _userRepository.GetAllAsync()).FirstOrDefault(k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (korisnik != null)
            {
                Console.WriteLine($"User found: {korisnik.Email}");
            }
            else
            {
                Console.WriteLine($"User not found with email: {email}");
            }
            return korisnik;
        }

        public async Task<int> GetKorisnikIdByEmailAsync(string email)
        {
            var korisnik = await GetUserByEmailAsync(email);
            return korisnik?.KorisnikID ?? -1;
        }

        public async Task<Korisnik> AuthenticateUserAsync(string email, string password)
        {
            var korisnik = await GetUserByEmailAsync(email);
            Console.WriteLine($"User retrieved: {korisnik?.Email}");
            if (korisnik != null)
            {
                var salt = GetUserSalt(korisnik.Email);
                Console.WriteLine($"Salt retrieved: {salt}");
                if (salt != null && VerifyPassword(password, korisnik.Lozinka, salt))
                {
                    Console.WriteLine("Password verified successfully.");
                    return korisnik;
                }
                else
                {
                    Console.WriteLine("Password verification failed.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
            return null;
        }

        private (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(saltBytes, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            Console.WriteLine($"Generated Salt: {salt}");
            Console.WriteLine($"Generated Hash: {savedPasswordHash}");

            return (savedPasswordHash, salt);
        }

        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            try
            {
                Console.WriteLine($"Entered Password for Verification: {enteredPassword}");

                byte[] saltBytes = Convert.FromBase64String(storedSalt);
                Console.WriteLine($"Salt Bytes: {BitConverter.ToString(saltBytes)}");

                var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
                byte[] computedHash = pbkdf2.GetBytes(20);
                Console.WriteLine($"Computed Hash: {BitConverter.ToString(computedHash)}");

                byte[] storedHashBytes = Convert.FromBase64String(storedHash);
                Console.WriteLine($"Stored Hash Bytes: {BitConverter.ToString(storedHashBytes)}");

                if (storedHashBytes.Length != 36)
                {
                    Console.WriteLine("Stored hash length is incorrect.");
                    return false;
                }

                byte[] storedSaltBytes = new byte[16];
                Array.Copy(storedHashBytes, 0, storedSaltBytes, 0, 16);
                Console.WriteLine($"Stored Salt Bytes: {BitConverter.ToString(storedSaltBytes)}");

                if (!saltBytes.SequenceEqual(storedSaltBytes))
                {
                    Console.WriteLine("Salt does not match.");
                    return false;
                }

                byte[] storedHashPart = new byte[20];
                Array.Copy(storedHashBytes, 16, storedHashPart, 0, 20);
                Console.WriteLine($"Stored Hash Part Bytes: {BitConverter.ToString(storedHashPart)}");

                for (int i = 0; i < 20; i++)
                {
                    if (storedHashPart[i] != computedHash[i])
                    {
                        Console.WriteLine($"Password verification failed at byte {i}: stored {storedHashPart[i]} != computed {computedHash[i]}");
                        return false;
                    }
                }

                Console.WriteLine("Password verification succeeded.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during password verification: {ex.Message}");
                return false;
            }
        }

        private void StoreUserSalt(string email, string salt)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Load(SaltFilePath);
            }
            catch (Exception)
            {
                // If the file does not exist, create a new document
                doc = new XDocument(new XElement("Users"));
            }

            var root = doc.Element("Users");
            if (root == null)
            {
                root = new XElement("Users");
                doc.Add(root);
            }

            var userElement = new XElement("User",
                new XElement("Email", email),
                new XElement("Salt", salt));
            root.Add(userElement);
            doc.Save(SaltFilePath);
        }

        private string GetUserSalt(string email)
        {
            try
            {
                var doc = XDocument.Load(SaltFilePath);
                var userElement = doc.Descendants("User")
                    .FirstOrDefault(user => user.Element("Email")?.Value == email);
                string salt = userElement?.Element("Salt")?.Value;
                if (salt != null)
                {
                    Console.WriteLine($"Salt found for {email}: {salt}");
                    if (Convert.FromBase64String(salt).Length != 16)
                    {
                        throw new InvalidOperationException("Retrieved salt length is incorrect.");
                    }
                }
                else
                {
                    Console.WriteLine($"Salt not found for {email}");
                }
                return salt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving salt for {email}: {ex.Message}");
                return null;
            }
        }
    }
}
