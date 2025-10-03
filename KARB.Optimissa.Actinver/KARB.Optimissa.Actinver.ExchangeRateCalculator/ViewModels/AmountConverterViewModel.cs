using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels
{
    public class AmountConverterViewModel
    {
        public DateTime SelectedDate { get; set; }
        public decimal AmountEntered { get; set; }
        public string MainCurrency { get; set; }
        public List<Conversion> Conversions { get; set; }
    }
}
