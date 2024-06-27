using BankovniSustavApp.DataAccess;
using BankovniSustavApp.Helpers;
using BankovniSustavApp.Models;
using BankovniSustavApp.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

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

        public UserAccountService(IGenericRepository<Korisnik> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(Korisnik newUser)
        {
            string plainText = newUser.Lozinka;
            // Generate the hash (includes the salt)
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Lozinka);

            // Store the full hash in the user record
            newUser.Lozinka = hashedPassword;

            // Save the user to the database
            var result = await _userRepository.AddAsync(newUser);
            if (result)
            {
                Console.WriteLine($"Stored Hash for {newUser.Email}: {hashedPassword}");

                // Immediate verification using the original plain text password
                bool verificationResult = BCrypt.Net.BCrypt.Verify(plainText, hashedPassword);
                Console.WriteLine($"Immediate verification result: {verificationResult}");
                if (!verificationResult)
                {
                    Console.WriteLine("Immediate verification during registration failed.");
                }
                return verificationResult;
            }
            return false;
        }

        public async Task<Korisnik> AuthenticateUserAsync(string email, string password)
        {
            var korisnik = await GetUserByEmailAsync(email);
            Console.WriteLine($"User retrieved: {korisnik?.Email}");
            if (korisnik != null)
            {
                Console.WriteLine($"Stored Hash: {korisnik.Lozinka}");
                if (VerifyPassword(password, korisnik.Lozinka))
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

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            try
            {
                Console.WriteLine($"Entered Password for Verification: {enteredPassword}");
                Console.WriteLine($"Stored Hash: {storedHash}");

                bool verified = BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
                Console.WriteLine($"Verification result: {verified}");
                if (verified)
                {
                    Console.WriteLine("Password verification succeeded.");
                }
                else
                {
                    Console.WriteLine("Password verification failed.");
                }

                return verified;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during password verification: {ex.Message}");
                return false;
            }
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
    }
}
