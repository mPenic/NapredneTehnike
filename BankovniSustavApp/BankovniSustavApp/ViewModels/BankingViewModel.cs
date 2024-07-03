using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BankovniSustavApp.Commands;
using BankovniSustavApp.Services;
using Newtonsoft.Json;

namespace BankovniSustavApp.ViewModels
{
    public class BankingViewModel : ObservableObject
    {
        private readonly BankingService _bankingService;
        private string _accounts;
        private string _transactions;
        private int _sourceAccountId;
        private int _destinationAccountId;
        private double _transferAmount;

        public string Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        public string Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        public int SourceAccountId
        {
            get => _sourceAccountId;
            set => SetProperty(ref _sourceAccountId, value);
        }

        public int DestinationAccountId
        {
            get => _destinationAccountId;
            set => SetProperty(ref _destinationAccountId, value);
        }

        public double TransferAmount
        {
            get => _transferAmount;
            set => SetProperty(ref _transferAmount, value);
        }

        public ICommand FetchAccountsCommand { get; }
        public ICommand FetchTransactionsCommand { get; }
        public ICommand TransferFundsCommand { get; }

        public BankingViewModel(BankingService bankingService)
        {
            _bankingService = bankingService;
            FetchAccountsCommand = new RelayCommand(async () => await FetchAccountsAsync());
            FetchTransactionsCommand = new RelayCommand(async () => await FetchTransactionsAsync());
            TransferFundsCommand = new RelayCommand(async () => await TransferFundsAsync());
        }

        private async Task FetchAccountsAsync()
        {
            var accountsJson = await _bankingService.GetAccountsAsync();
            var accounts = JsonConvert.DeserializeObject<List<Account>>(accountsJson);
            Accounts = FormatAccounts(accounts);
        }

        private async Task FetchTransactionsAsync()
        {
            var transactionsJson = await _bankingService.GetTransactionsAsync();
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(transactionsJson);
            Transactions = FormatTransactions(transactions);
        }

        private async Task TransferFundsAsync()
        {
            var transferRequest = new
            {
                SourceAccountId,
                DestinationAccountId,
                Amount = TransferAmount
            };
            await _bankingService.TransferFundsAsync(transferRequest);
            await FetchAccountsAsync();
        }

        private string FormatAccounts(List<Account> accounts)
        {
            var sb = new StringBuilder();
            foreach (var account in accounts)
            {
                sb.AppendLine($"ID: {account.Id}, Name: {account.Name}, Balance: {account.Balance:C}");
            }
            return sb.ToString();
        }

        private string FormatTransactions(List<Transaction> transactions)
        {
            var sb = new StringBuilder();
            foreach (var transaction in transactions)
            {
                sb.AppendLine($"ID: {transaction.Id}, Amount: {transaction.Amount:C}, Description: {transaction.Description}");
            }
            return sb.ToString();
        }
    }

    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
