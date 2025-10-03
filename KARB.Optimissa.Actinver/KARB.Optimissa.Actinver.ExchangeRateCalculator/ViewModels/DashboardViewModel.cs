

using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels
{
    public class DashboardViewModel
    {
        public string MainCurrency { get; set; }
        public List<Conversion> Conversions { get; set; }

        public DateTime DateLatest { get; set; }
    }
}
