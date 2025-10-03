
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Currencies> GetCurrenciesAsync()
        {
            var url = $"https://api.frankfurter.dev/v1/currencies";
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>(url);

            var currencies = new Currencies();
            foreach (var par in response)
            {
                currencies.Add(new CurrencyDTO()
                {
                    CurrencySymbol = par.Key,
                    FullName = par.Value
                });
            }
            
            return currencies;
        }

        public async Task<ExchangeRateResponse> GetExchangeRatesBySymbos(string mainCurrency, string favoriteCurrencies)
        {
            string baseParam = $"base={mainCurrency}";
            string symbolsParam = $"symbols={favoriteCurrencies}";
            var url = $"https://api.frankfurter.dev/v1/latest?{baseParam}&{symbolsParam}";
            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);
            return response;
        }

        public async Task<ExchangeRateResponse> GetExchangeRatesByDate(DateTime date, string mainCurrency, string favoriteCurrencies)
        {
            string queryDate = date.ToString("yyyy-MM-yy");
            string baseParam = $"base={mainCurrency}";
            string symbolsParam = $"symbols={favoriteCurrencies}";
            var url = $"https://api.frankfurter.dev/v1/{queryDate}?{baseParam}&{symbolsParam}";
            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);
            return response;
        }

        public async Task<ExchangeRateResponse> GetExchangeRatesByDateAndAmount(DateTime date, decimal amountConvert, string mainCurrency, string favoriteCurrencies)
        {
            string queryDate = date.ToString("yyyy-MM-yy");
            string amountParam = $"amount={amountConvert}";
            string baseParam = $"base={mainCurrency}";
            string symbolsParam = $"symbols={favoriteCurrencies}";
            var url = $"https://api.frankfurter.dev/v1/{queryDate}?{baseParam}&{symbolsParam}&{amountParam}";
            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);
            return response;
        }
    }
}
