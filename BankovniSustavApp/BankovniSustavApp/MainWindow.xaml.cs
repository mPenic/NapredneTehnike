using BankovniSustavApp.DataAccess;
using BankovniSustavApp.Models;
using BankovniSustavApp.Repositories;
using BankovniSustavApp.Services;
using BankovniSustavApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankovniSustavApp
{
    
    public partial class MainWindow : Window
    {
        private readonly IGenericRepository<Korisnik> _korisnikRepository;
        private readonly IGenericRepository<Logovi> _logoviRepository;
        private readonly IGenericRepository<Racuni> _racuniRepository;
        private readonly IGenericRepository<Transakcije> _transakcijeRepository;
        private readonly IUserAccountService _userAccountService;
        private readonly INavigationService _navigationService;
        private readonly LoginViewModel _loginViewModel;

        public MainWindow()
        {

        }
        public MainWindow(IGenericRepository<Korisnik> korisnikRepository,
            IGenericRepository<Logovi> logoviRepository,
            IAccountRepository<Racuni> racuniRepository,
            IGenericRepository<Transakcije> transakcijeRepository,
            IUserAccountService userAccountService,
            INavigationService navigationService,
            LoginViewModel loginViewModel) : this()
        {
            _logoviRepository = logoviRepository;
            _korisnikRepository = korisnikRepository;
            _racuniRepository = racuniRepository;
            _transakcijeRepository = transakcijeRepository;
            _userAccountService = userAccountService;
            _navigationService = navigationService;
            InitializeComponent();
            _loginViewModel = loginViewModel;
            this.DataContext = _loginViewModel;
          
         
            TestDatabaseConnection();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel loginViewModel)
            {
                var passwordBox = (PasswordBox)sender;
                loginViewModel.SecurePassword = passwordBox.SecurePassword;

                Console.WriteLine("PasswordBox changed, SecurePassword updated in ViewModel.");
            }
        }

        private async void TestDatabaseConnection()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            bool isConnected = await dbHelper.TestConnectionAsync();
            if (isConnected)
            {
                MessageBox.Show("Connection successful!", "Database Test", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Connection failed. Check the details and try again.", "Database Test", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
