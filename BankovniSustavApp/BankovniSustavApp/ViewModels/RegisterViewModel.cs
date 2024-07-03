using BankovniSustavApp.Commands;
using BankovniSustavApp.Models;
using BankovniSustavApp.Services;
using BankovniSustavApp.Helpers;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BankovniSustavApp.ViewModels
{
    public class RegisterViewModel
    {
        private readonly IUserAccountService _userAccountService;
        private readonly INavigationService _navigationService;

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public SecureString Lozinka { get; set; }

        public ICommand RegisterUserCommand { get; private set; }
        public ICommand NavigateToLoginCommand { get; private set; }

        public RegisterViewModel(IUserAccountService userAccountService, INavigationService navigationService)
        {
            _userAccountService = userAccountService;
            _navigationService = navigationService;
            RegisterUserCommand = new RelayCommand(async () => await RegisterUserExecute());
            NavigateToLoginCommand = new RelayCommand(NavigateToLoginExecute);
        }

        private void NavigateToLoginExecute()
        {
            _navigationService.NavigateToLogin();
        }

        private async Task RegisterUserExecute()
        {
            try
            {
                var lozinka = ConvertSecureStringToString(Lozinka);
                Console.WriteLine($"Converted Lozinka: {lozinka}");

                if (string.IsNullOrEmpty(lozinka))
                {
                    MessageBox.Show("Password cannot be empty.", "Error");
                    return;
                }

                var newUser = new Korisnik
                {
                    Ime = this.Ime,
                    Prezime = this.Prezime,
                    Email = this.Email,
                    Lozinka = lozinka, 
                    DatumRegistracije = DateTime.Now, 
                    Status = "Active" 
                };

                Console.WriteLine($"New User: {newUser.Ime}, {newUser.Prezime}, {newUser.Email}, {newUser.Lozinka}");

                if (string.IsNullOrEmpty(newUser.Ime) || string.IsNullOrEmpty(newUser.Prezime) || string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Lozinka))
                {
                    MessageBox.Show("All fields must be filled.", "Error");
                    return;
                }

                var result = await _userAccountService.RegisterUserAsync(newUser);
                if (result)
                {
                    MessageBox.Show("Registration successful", "Success");
                    _navigationService.NavigateToDashboard(); 
                }
                else
                {
                    MessageBox.Show("Registration failed", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private string ConvertSecureStringToString(SecureString secureString)
        {
            if (secureString == null || secureString.Length == 0)
            {
                return null;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
