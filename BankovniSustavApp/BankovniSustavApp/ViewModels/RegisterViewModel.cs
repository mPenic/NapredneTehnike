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
                // Convert SecureString to string for Lozinka
                var lozinka = new System.Net.NetworkCredential(string.Empty, Lozinka).Password;
                var newUser = new Korisnik
                {
                    Ime = this.Ime,
                    Prezime = this.Prezime,
                    Email = this.Email,
                    Lozinka = lozinka, // This should be hashed inside the service
                    DatumRegistracije = DateTime.Now, // Assuming registration date is now
                    Status = "Active" // Assuming default status is "Active"
                };

                var result = await _userAccountService.RegisterUserAsync(newUser);
                if (result)
                {
                    MessageBox.Show("Registration successful", "Success");
                    _navigationService.NavigateToDashboard();  // Close the window after successful registration
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }
    }
}
