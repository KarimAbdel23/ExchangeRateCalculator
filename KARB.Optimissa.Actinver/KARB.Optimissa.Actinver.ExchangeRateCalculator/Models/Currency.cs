namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models
{
    public class CurrencyDTO
    {
        public string CurrencySymbol { get; set; }
        public string FullName { get; set; }
    }

    public class Currencies : List<CurrencyDTO> { }
}
