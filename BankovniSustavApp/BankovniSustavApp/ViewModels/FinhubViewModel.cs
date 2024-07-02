using System.Threading.Tasks;
using System.Windows.Input;
using BankovniSustavApp.Commands;
using BankovniSustavApp.Services;
using Newtonsoft.Json;

namespace BankovniSustavApp.ViewModels
{
    public class FinnhubViewModel : ObservableObject
    {
        private readonly FinnhubService _finnhubService;
        private string _stockQuote;
        private string _companyProfile;
        private string _stockQuoteFormatted;
        private string _companyProfileFormatted;

        public string StockQuote
        {
            get => _stockQuote;
            set
            {
                SetProperty(ref _stockQuote, value);
                StockQuoteFormatted = FormatStockQuote(value);
            }
        }

        public string StockQuoteFormatted
        {
            get => _stockQuoteFormatted;
            set => SetProperty(ref _stockQuoteFormatted, value);
        }

        public string CompanyProfile
        {
            get => _companyProfile;
            set
            {
                SetProperty(ref _companyProfile, value);
                CompanyProfileFormatted = FormatCompanyProfile(value);
            }
        }

        public string CompanyProfileFormatted
        {
            get => _companyProfileFormatted;
            set => SetProperty(ref _companyProfileFormatted, value);
        }

        public ICommand FetchStockQuoteCommand { get; }
        public ICommand FetchCompanyProfileCommand { get; }

        public FinnhubViewModel(FinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
            FetchStockQuoteCommand = new RelayCommand(async () => await FetchStockQuoteAsync());
            FetchCompanyProfileCommand = new RelayCommand(async () => await FetchCompanyProfileAsync());
        }

        private async Task FetchStockQuoteAsync()
        {
            StockQuote = await _finnhubService.GetStockQuoteAsync("AAPL");
        }

        private async Task FetchCompanyProfileAsync()
        {
            CompanyProfile = await _finnhubService.GetCompanyProfileAsync("AAPL");
        }

        private string FormatStockQuote(string json)
        {
            try
            {
                var stockQuote = JsonConvert.DeserializeObject<StockQuote>(json);
                return $"Current Price: {stockQuote.c}\nHigh Price: {stockQuote.h}\nLow Price: {stockQuote.l}\nOpen Price: {stockQuote.o}\nPrevious Close: {stockQuote.pc}";
            }
            catch
            {
                return "Error parsing stock quote data.";
            }
        }

        private string FormatCompanyProfile(string json)
        {
            try
            {
                var companyProfile = JsonConvert.DeserializeObject<CompanyProfile>(json);
                return $"Country: {companyProfile.country}\nCurrency: {companyProfile.currency}\nExchange: {companyProfile.exchange}";
            }
            catch
            {
                return "Error parsing company profile data.";
            }
        }
    }

    public class StockQuote
    {
        public decimal c { get; set; } // Current price
        public decimal h { get; set; } // High price
        public decimal l { get; set; } // Low price
        public decimal o { get; set; } // Open price
        public decimal pc { get; set; } // Previous close price
    }

    public class CompanyProfile
    {
        public string country { get; set; }
        public string currency { get; set; }
        public string exchange { get; set; }
    }
}
