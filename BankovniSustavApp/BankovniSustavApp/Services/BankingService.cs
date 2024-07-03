using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankovniSustavApp.Services
{
    public class BankingService
    {
        private readonly HttpClient _httpClient;

        public BankingService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7000/api/") };
        }

        public async Task<string> GetAccountsAsync()
        {
            var response = await _httpClient.GetAsync("accounts");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetTransactionsAsync()
        {
            var response = await _httpClient.GetAsync("transactions");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> TransferFundsAsync(object transferRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(transferRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("accounts/transfer", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
