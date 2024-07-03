using BankovniSustavApp.Commands;
using BankovniSustavApp.Services;
using BankovniSustavApp.Helpers;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BankingLibrary;

namespace BankovniSustavApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IUserAccountService _userAccountService;
        private string _email;
        private SecureString _securePassword;
        private readonly INavigationService _navigationService;
        private string _welcomeMessage;

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(IUserAccountService userAccountService, INavigationService navigationService)
        {
            _userAccountService = userAccountService;
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(async () => await ExecuteLogin(), CanExecuteLogin);
            NavigateToRegisterCommand = new RelayCommand(() => _navigationService.NavigateToRegister());

            WelcomeMessage = ResourceAccessor.GetStringResource("WelcomeMessage");
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        internal SecureString SecurePassword
        {
            get => _securePassword;
            set
            {
                _securePassword = value;
                OnPropertyChanged();
            }
        }

        private async Task ExecuteLogin()
        {
            string password = SecureStringToString(SecurePassword);

            Console.WriteLine($"Entered Password: {password}");

            var korisnik = await _userAccountService.AuthenticateUserAsync(Email, password);
            if (korisnik != null)
            {
                Console.WriteLine("Login successful.");
                await SessionManager.SetCurrentKorisnikIdByEmailAsync(Email);

                if (!KeyManager.KeysExistForUser(SessionManager.CurrentKorisnikId))
                {
                    var keys = KeyManager.GenerateRsaKeys();
                    KeyManager.SaveKeysForUser(SessionManager.CurrentKorisnikId, keys);
                }

                _navigationService.NavigateToDashboard();
            }
            else
            {
                MessageBox.Show("Login failed. Please check your email and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrEmpty(_email) && _securePassword != null && _securePassword.Length > 0;
        }

        private string SecureStringToString(SecureString secureString)
        {
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
