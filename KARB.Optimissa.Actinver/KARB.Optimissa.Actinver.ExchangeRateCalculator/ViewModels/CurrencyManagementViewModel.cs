using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels
{
    public class CurrencyManagementViewModel
    {
        public string MainCurrency { get; set; }
        public List<CurrencyDTO> CurrencyCatalog { get; set; }

        public List<string> FavoriteCurrencies { get; set; } = new();
    }
}
