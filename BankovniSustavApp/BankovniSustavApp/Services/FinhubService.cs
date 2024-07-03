using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankovniSustavApp.Services
{
    public class FinnhubService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<string> GetStockQuoteAsync(string symbol)
        {
            var response = await httpClient.GetAsync($"https://finnhub.io/api/v1/quote?symbol={symbol}&token=cq1u0vhr01qjh3d5voqgcq1u0vhr01qjh3d5vor0");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetCompanyProfileAsync(string symbol)
        {
            var response = await httpClient.GetAsync($"https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token=cq1u0vhr01qjh3d5voqgcq1u0vhr01qjh3d5vor0");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
