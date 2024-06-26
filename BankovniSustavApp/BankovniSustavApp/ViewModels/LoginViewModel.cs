using BankovniSustavApp.Commands;
using BankovniSustavApp.Services;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BankovniSustavApp.ViewModels
{
    public class LoginViewModel
    {
        private readonly IUserAccountService _userAccountService;
        private string _email;
        private SecureString _securePassword;
        private readonly INavigationService _navigationService; // Action delegate to navigate to the RegisterWindow.

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(IUserAccountService userAccountService, INavigationService navigationService)
        {
            _userAccountService = userAccountService;
            _navigationService = navigationService; // Assign the passed-in delegate.
            LoginCommand = new RelayCommand(async () => await ExecuteLogin(), CanExecuteLogin);
            NavigateToRegisterCommand = new RelayCommand(() => _navigationService.NavigateToRegister());
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                // OnPropertyChanged(nameof(Email)); // Uncomment when OnPropertyChanged is implemented
            }
        }

        internal SecureString SecurePassword
        {
            get => _securePassword;
            set
            {
                _securePassword = value;
                // OnPropertyChanged(nameof(SecurePassword)); // Uncomment when OnPropertyChanged is implemented
            }
        }

        public async Task ExecuteLogin()
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
                var password = System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);

                Console.WriteLine($"Entered Password: {password}");

                VerifySecurePassword(); // Add this line to check the conversion

                var korisnik = await _userAccountService.AuthenticateUserAsync(Email, password);
                if (korisnik != null)
                {
                    Console.WriteLine("Login successful.");
                    _navigationService.NavigateToDashboard();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your email and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrEmpty(_email) && _securePassword != null && _securePassword.Length > 0;
        }

        private void VerifySecurePassword()
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
                var password = System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
                Console.WriteLine($"SecurePassword converted to: {password}");
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
