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
using BankovniSustavApp.Views;
using System.Windows.Navigation;
using System.Windows.Data;
using BankingLibrary;

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
        private readonly Utility _utility;
        private string _selectedLanguage;
        private string _welcomeMessage;
        private int _autoRefreshInterval;
        private int _maxDisplayItems;
        private int _deleteTransactionId;
        private int _searchId;
        private int _currentKorisnikId;
        private string _userEmail;
        private string _userName;
        private Racuni _selectedAccount;
        private ObservableCollection<Racuni> _userAccounts;
        private readonly IUserAccountService _userAccountService;
        private readonly IGenericRepository<Korisnik> _korisnikRepository;
        private readonly IGenericRepository<Transakcije> _transakcijeRepository;
        private readonly IAccountRepository<Racuni> _racuniRepository;
        private ObservableCollection<Transakcije> _transactions;
        private readonly FinnhubViewModel _finnhubViewModel;
        private readonly BankingViewModel _bankingViewModel;
        private string _selectedTransactionType;
        private decimal _amount;
        public ObservableCollection<string> TransactionTypes { get; } = new ObservableCollection<string> { "Deposit", "Withdrawal" };

        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand LoadUserAccountsCommand { get; private set; }

        public ICommand GetAllTransactionsCommand { get; private set; }
        public ICommand GetTransactionByIdCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }

        public ICommand GeneratePdfReportCommand { get; private set; }
        public ICommand GenerateRtfReportCommand { get; private set; }

        public ICommand SignDataCommand { get; private set; }
        public ICommand VerifySignatureCommand { get; private set; }

        public ICommand AddAccountCommand { get; private set; }
        public ICommand CreateTransactionCommand { get; private set; }
        public ICommand OpenTransactionFormCommand { get; private set; }
        public ICommand SubmitTransactionCommand { get; private set; }

        public ICommand OpenFinnhubViewCommand { get; }
        public ICommand OpenBankingViewCommand { get; }

        public ICommand LoadTransactionsCommand { get; }
        public ICommand SortTransactionsCommand { get; }

        public ICommand ShowDialogOneCommand { get; }
        public ICommand ShowDialogTwoCommand { get; }

        public ICollectionView TransactionsView { get; set; }
        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                SetProperty(ref _filterText, value);
                TransactionsView?.Refresh();
            }
        }

        public ObservableCollection<Transakcije> Transactions
        {
            get => _transactions;
            set
            {
                SetProperty(ref _transactions, value);
                OnPropertyChanged();
            }
        }

        public DashboardViewModel(
            IGenericRepository<Transakcije> transakcijeRepository,
            IAccountRepository<Racuni> racuniRepository,
            IUserAccountService userAccountService,
            IGenericRepository<Korisnik> korisnikRepository,
            FinnhubViewModel finnhubViewModel,
            BankingViewModel bankingViewModel)
        {
            _transakcijeRepository = transakcijeRepository;
            _userAccountService = userAccountService;
            _racuniRepository = racuniRepository;
            _korisnikRepository = korisnikRepository;

            _utility = new Utility();

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            GetAllTransactionsCommand = new RelayCommand(async () => await LoadTransactions());
            GetTransactionByIdCommand = new RelayCommand(async () => await GetTransactionById());
            DeleteTransactionCommand = new RelayCommand(async () => await DeleteTransaction());
            GeneratePdfReportCommand = new RelayCommand(() => GeneratePdfReport());
            GenerateRtfReportCommand = new RelayCommand(() => GenerateRtfReport());
            SignDataCommand = new RelayCommand(SignDataExecute);
            VerifySignatureCommand = new RelayCommand(VerifySignatureExecute);
            AddAccountCommand = new RelayCommand(async () => await AddAccount());
            SubmitTransactionCommand = new RelayCommand(async () => await SubmitTransaction());

            InitializeUser().ConfigureAwait(false);
            LoadSettings();
            LoadTransactions().ConfigureAwait(false);

            _finnhubViewModel = finnhubViewModel;
            OpenFinnhubViewCommand = new RelayCommand(OpenFinnhubView);

            _bankingViewModel = bankingViewModel;
            OpenBankingViewCommand = new RelayCommand(OpenBankingView);

            LoadTransactionsCommand = new RelayCommand(async () => await LoadTransactions());
            SortTransactionsCommand = new RelayCommand(param => SortTransactions(param as string));

            Transactions = new ObservableCollection<Transakcije>();
            TransactionsView = CollectionViewSource.GetDefaultView(Transactions);
            TransactionsView.Filter = FilterTransactions;
            TransactionsView.SortDescriptions.Add(new SortDescription("TransakcijaID", ListSortDirection.Ascending));

            ShowDialogOneCommand = new RelayCommand(ShowDialogOne);
            ShowDialogTwoCommand = new RelayCommand(ShowDialogTwo);
        }

        // Account Management

        public int CurrentKorisnikId
        {
            get => _currentKorisnikId;
            set => SetProperty(ref _currentKorisnikId, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string UserEmail
        {
            get => _userEmail;
            set => SetProperty(ref _userEmail, value);
        }

        public ObservableCollection<Racuni> UserAccounts
        {
            get => _userAccounts;
            set => SetProperty(ref _userAccounts, value);
        }

        public Racuni SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }

        private async Task InitializeUser()
        {
            CurrentKorisnikId = SessionManager.CurrentKorisnikId;
            _userEmail = SessionManager.CurrentUserEmail;

            var korisnik = await _korisnikRepository.GetByIdAsync(CurrentKorisnikId);
            if (korisnik != null)
            {
                UserName = korisnik.Ime;
                WelcomeMessage = $"Welcome, {UserName}!";
                await LoadUserAccounts();
            }
        }

        public async Task LoadUserAccounts()
        {
            var userAccounts = await _racuniRepository.GetByKorisnikIdAsync(CurrentKorisnikId);
            UserAccounts = new ObservableCollection<Racuni>(userAccounts);

            if (UserAccounts.Any())
            {
                SelectedAccount = UserAccounts.First();
            }
        }

        private async Task AddAccount()
        {
            var random = new Random();
            var newAccount = new Racuni
            {
                KorisnikID = CurrentKorisnikId,
                BrojRacuna = "HR" + random.Next(100000000, 999999999),
                Stanje = 0,
                DatumOtvaranja = DateTime.Now,
                Vrsta = "Savings",
                Valuta = "EUR"
            };

            bool success = await _racuniRepository.AddAsync(newAccount);

            if (success)
            {
                await LoadUserAccounts();
                MessageBox.Show("New account added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add new account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Transactions

        public int DeleteTransactionId
        {
            get => _deleteTransactionId;
            set => SetProperty(ref _deleteTransactionId, value);
        }

        public string SelectedTransactionType
        {
            get => _selectedTransactionType;
            set => SetProperty(ref _selectedTransactionType, value);
        }

        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public int SearchId
        {
            get => _searchId;
            set => SetProperty(ref _searchId, value);
        }

        private async Task LoadTransactions()
        {
            var transactionsList = await _transakcijeRepository.GetAllAsync();

            foreach (var transaction in transactionsList)
            {
                transaction.IznosFormatted = _utility.FormatCurrency(transaction.Iznos);
            }

            Transactions = new ObservableCollection<Transakcije>(transactionsList);
            TransactionsView = CollectionViewSource.GetDefaultView(Transactions);
            TransactionsView.Filter = FilterTransactions;
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

        private async Task SubmitTransaction()
        {
            if (SelectedAccount == null)
            {
                MessageBox.Show("Please select an account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var transactionType = SelectedTransactionType == "Deposit" ? "Credit" : "Debit";
            var newTransaction = new Transakcije
            {
                RacunID = SelectedAccount.RacunID,
                DatumVrijeme = DateTime.Now,
                Iznos = transactionType == "Credit" ? Amount : -Amount,
                Vrsta = transactionType,
                Opis = "User transaction"
            };

            bool success = await _transakcijeRepository.AddAsync(newTransaction);

            if (success)
            {
                await LoadTransactions();
                MessageBox.Show("Transaction completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to complete the transaction.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool FilterTransactions(object obj)
        {
            if (obj is Transakcije transaction)
            {
                return string.IsNullOrEmpty(FilterText) || transaction.Opis.Contains(FilterText);
            }
            return false;
        }

        private void SortTransactions(object parameter)
        {
            if (parameter is string sortBy)
            {
                var direction = ListSortDirection.Ascending;
                if (TransactionsView.SortDescriptions.Any() && TransactionsView.SortDescriptions[0].PropertyName == sortBy)
                {
                    direction = TransactionsView.SortDescriptions[0].Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }

                TransactionsView.SortDescriptions.Clear();
                TransactionsView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            }
        }

        //Settings

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

        public ObservableCollection<string> StorageMethods { get; } = new ObservableCollection<string>
        {
            "INI File",
            "Windows Registry"
        };

        private string _selectedStorageMethod;
        public string SelectedStorageMethod
        {
            get => _selectedStorageMethod;
            set
            {
                if (SetProperty(ref _selectedStorageMethod, value))
                {
                    InitializeSettingsManager();
                }
            }
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        private void LoadSettings()
        {
            InitializeSettingsManager();
            SelectedLanguage = _settingsManager.ReadSetting("General", "Language");
            AutoRefreshInterval = int.TryParse(_settingsManager.ReadSetting("General", "AutoRefreshInterval"), out int interval) ? interval : 30;
            MaxDisplayItems = int.TryParse(_settingsManager.ReadSetting("General", "MaxDisplayItems"), out int items) ? items : 100;
            WelcomeMessage = $"Welcome, {UserName}!";
        }

        private void SaveSettings()
        {
            _settingsManager.WriteSetting("General", "Language", SelectedLanguage);
            _settingsManager.WriteSetting("General", "AutoRefreshInterval", AutoRefreshInterval.ToString());
            _settingsManager.WriteSetting("General", "MaxDisplayItems", MaxDisplayItems.ToString());

            MessageBox.Show("Settings have been saved successfully.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void InitializeSettingsManager()
        {
            bool useRegistry = SelectedStorageMethod == "Windows Registry";
            _settingsManager = new SettingsManager("settings.ini", useRegistry);
        }

        //Reports

        private void GeneratePdfReport()
        {
            var reportService = new ReportGenerationService();
            reportService.GeneratePdfReport(Transactions, "transactions_report.pdf");
        }

        private void GenerateRtfReport()
        {
            var reportService = new ReportGenerationService();
            reportService.GenerateRtfReport(Transactions, "transactions_report.rtf");
        }

        //Signature

        public string DataToSign { get; set; }
        public byte[] Signature { get; set; }
        public string DataToVerify { get; set; }

        public byte[] SignData(string dataToSign)
        {
            var privateKey = KeyManager.LoadPrivateKeyForUser(CurrentKorisnikId);
            if (string.IsNullOrEmpty(privateKey))
            {
                MessageBox.Show("Private key not found for the user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new ArgumentNullException(nameof(privateKey), "Private key not found for the user.");
            }

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                var dataBytes = Encoding.UTF8.GetBytes(dataToSign);
                return rsa.SignData(dataBytes, SHA256.Create());
            }
        }

        private void SignDataExecute()
        {
            try
            {
                Signature = SignData(DataToSign);
                MessageBox.Show("Data signed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error signing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool VerifySignature(string originalData, byte[] signature)
        {
            var publicKey = KeyManager.PublicKey;
            if (string.IsNullOrEmpty(publicKey))
            {
                MessageBox.Show("Public key not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new ArgumentNullException(nameof(publicKey), "Public key not found.");
            }

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                var dataBytes = Encoding.UTF8.GetBytes(originalData);
                return rsa.VerifyData(dataBytes, SHA256.Create(), signature);
            }
        }

        private void VerifySignatureExecute()
        {
            try
            {
                var isVerified = VerifySignature(DataToVerify, Signature);
                MessageBox.Show(isVerified ? "Signature verified." : "Verification failed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error verifying signature: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Navigation

        private void OpenFinnhubView()
        {
            var finnhubView = new FinnhubWindow(_finnhubViewModel);
            finnhubView.Show();
        }

        private void OpenBankingView()
        {
            var bankingView = new BankingWindow(_bankingViewModel);
            bankingView.Show();
        }

        private void ShowDialogOne()
        {
            var dialog = new DialogOneWindow();
            dialog.ShowDialog();
        }

        private void ShowDialogTwo()
        {
            var dialog = new DialogTwoWindow();
            dialog.ShowDialog();
        }
    }
}
