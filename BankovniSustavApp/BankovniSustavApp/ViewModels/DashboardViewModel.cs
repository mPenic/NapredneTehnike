using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BankovniSustavApp.Models;
using BankovniSustavApp.Repositories;
using BankovniSustavApp.Commands;
using BankovniSustavApp.Helpers;
using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BankovniSustavApp.Services;
using System.Security.Cryptography;

namespace BankovniSustavApp.ViewModels
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class DashboardViewModel : ObservableObject
    {
        private SettingsManager _settingsManager;
        private string _selectedLanguage;
        private string _welcomeMessage;
        private int _autoRefreshInterval;
        private int _maxDisplayItems;
        private int _deleteTransactionId;
        private int _searchId;
        private int _currentKorisnikId;
        private string _userEmail;
        private Racuni _selectedAccount;
        private ObservableCollection<Racuni> _userAccounts;
        private readonly IUserAccountService _userAccountService;
        private readonly IGenericRepository<Transakcije> _transakcijeRepository;
        private readonly IAccountRepository<Racuni> _racuniRepository;
        private ObservableCollection<Transakcije> _transactions;


        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand LoadUserAccountsCommand { get; private set; }
        public ICommand GetAllTransactionsCommand { get; private set; }
        public ICommand GetTransactionByIdCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }
        public ICommand GeneratePdfReportCommand { get; private set; }
        public ICommand GenerateRtfReportCommand { get; private set; }
        public ICommand SignDataCommand { get; private set; }
        public ICommand VerifySignatureCommand { get; private set; }

        public DashboardViewModel(IGenericRepository<Transakcije> transakcijeRepository, IAccountRepository<Racuni> racuniRepository, IUserAccountService userAccountService)
        {
            _settingsManager = new SettingsManager("settings.ini");
            _transakcijeRepository = transakcijeRepository;
            _userAccountService = userAccountService;
            _racuniRepository = racuniRepository;

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            GetAllTransactionsCommand = new RelayCommand(async () => await LoadTransactions());
            GetTransactionByIdCommand = new RelayCommand(async () => await GetTransactionById());
            DeleteTransactionCommand = new RelayCommand(async () => await DeleteTransaction());
            GeneratePdfReportCommand = new RelayCommand(() => GeneratePdfReport());
            GenerateRtfReportCommand = new RelayCommand(() => GenerateRtfReport());
            SignDataCommand = new RelayCommand(SignDataExecute);
            VerifySignatureCommand = new RelayCommand(VerifySignatureExecute);

            LoadSettings();
            LoadTransactions().ConfigureAwait(false);

            LoadUserAccountsCommand = new RelayCommand(async () => await LoadUserAccounts());
        }
        public ObservableCollection<Racuni> UserAccounts
        {
            get => _userAccounts;
            set => SetProperty(ref _userAccounts, value);
        }
        public async Task InitializeUserAccounts(string email)
        {
            _userEmail = email;
            CurrentKorisnikId = await _userAccountService.GetKorisnikIdByEmailAsync(email);
            await LoadUserAccounts();
        }
        public async Task LoadUserAccounts()
        {
            // This should be the list of accounts for the logged-in user
            var userAccounts = await _racuniRepository.GetByKorisnikIdAsync(CurrentKorisnikId);
            UserAccounts = new ObservableCollection<Racuni>(userAccounts);

            // Automatically select the first account
            if (UserAccounts.Any())
            {
                SelectedAccount = UserAccounts.First();
            }
        }

        public int CurrentKorisnikId
        {
            get => _currentKorisnikId;
            set => SetProperty(ref _currentKorisnikId, value);
        }
        public Racuni SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }
        public int DeleteTransactionId
        {
            get => _deleteTransactionId;
            set => SetProperty(ref _deleteTransactionId, value);
        }

        public int SearchId
        {
            get => _searchId;
            set => SetProperty(ref _searchId, value);
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public int AutoRefreshInterval
        {
            get => _autoRefreshInterval;
            set => SetProperty(ref _autoRefreshInterval, value);
        }

        public int MaxDisplayItems
        {
            get => _maxDisplayItems;
            set => SetProperty(ref _maxDisplayItems, value);
        }

        public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
        {
            "English",
            "Croatian"
        };

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }
        public ObservableCollection<Transakcije> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }
        private async Task GetTransactionById()
        {
            var transaction = await _transakcijeRepository.GetByIdAsync(SearchId);
            Transactions.Clear();
            if (transaction != null)
            {
                Transactions.Add(transaction);
            }
            else
            {
                MessageBox.Show("Transaction not found.");
            }
        }

        private async Task DeleteTransaction()
        {
            var transactionToDelete = Transactions.FirstOrDefault(t => t.TransakcijaID == DeleteTransactionId);
            if (transactionToDelete == null)
            {
                MessageBox.Show("Please enter a valid transaction ID to delete.");
                return;
            }

            bool result = await _transakcijeRepository.DeleteAsync(transactionToDelete.TransakcijaID);
            if (result)
            {
                Transactions.Remove(transactionToDelete);
                MessageBox.Show("Transaction deleted successfully.");
            }
            else
            {
                MessageBox.Show("Failed to delete transaction.");
            }
        }
        private void GeneratePdfReport()
        {
            var reportService = new ReportGenerationService();
            // Assuming filePath is determined via a SaveFileDialog or similar
            reportService.GeneratePdfReport(Transactions, "transactions_report.pdf");
        }

        private void GenerateRtfReport()
        {
            var reportService = new ReportGenerationService();
            // Assuming filePath is determined via a SaveFileDialog or similar
            reportService.GenerateRtfReport(Transactions, "transactions_report.rtf");
        }
        private async Task LoadTransactions()
        {
            var transactionsList = await _transakcijeRepository.GetAllAsync();
            Transactions = new ObservableCollection<Transakcije>(transactionsList);
        }

        private void LoadSettings()
        {
            SelectedLanguage = _settingsManager.ReadSetting("General", "Language");
            AutoRefreshInterval = int.TryParse(_settingsManager.ReadSetting("General", "AutoRefreshInterval"), out int interval) ? interval : 30;
            MaxDisplayItems = int.TryParse(_settingsManager.ReadSetting("General", "MaxDisplayItems"), out int items) ? items : 100;

            WelcomeMessage = "Welcome, User!";
        }

        private void SaveSettings()
        {
            _settingsManager.WriteSetting("General", "Language", SelectedLanguage);
            _settingsManager.WriteSetting("General", "AutoRefreshInterval", AutoRefreshInterval.ToString());
            _settingsManager.WriteSetting("General", "MaxDisplayItems", MaxDisplayItems.ToString());
        }

        public string DataToSign { get; set; }
        public byte[] Signature { get; set; }
        public string DataToVerify { get; set; }
        public byte[] SignData(string dataToSign)
        {
            var keys = KeyManager.LoadKeys();
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(keys.PrivateKey);
                var dataBytes = Encoding.UTF8.GetBytes(dataToSign);
                return rsa.SignData(dataBytes, SHA256.Create());
            }
        }
        private void SignDataExecute()
        {
            // Assumes DataToSign is a property bound to a text box in the UI
            Signature = SignData(DataToSign);
        }
        public bool VerifySignature(string originalData, byte[] signature)
        {
            var keys = KeyManager.LoadKeys();
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(keys.PublicKey);
                var dataBytes = Encoding.UTF8.GetBytes(originalData);
                return rsa.VerifyData(dataBytes, SHA256.Create(), signature);
            }
        }

        private void VerifySignatureExecute()
        {
            // Assumes DataToVerify is a property bound to a text box in the UI
            var isVerified = VerifySignature(DataToVerify, Signature);
            MessageBox.Show(isVerified ? "Signature verified." : "Verification failed.");
        }
    }
}
