using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse> GetExchangeRatesBySymbos(string mainCurrency, string favoriteCurrencies);
        Task<Currencies> GetCurrenciesAsync();
        Task<ExchangeRateResponse> GetExchangeRatesByDate(DateTime date, string mainCurrency, string favoriteCurrencies);
        Task<ExchangeRateResponse> GetExchangeRatesByDateAndAmount(DateTime date, decimal amountConvert, string mainCurrency, string favoriteCurrencies);
    }
}
