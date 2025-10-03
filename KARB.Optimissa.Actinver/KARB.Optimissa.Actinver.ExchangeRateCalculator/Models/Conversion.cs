namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models
{
    public class Conversion
    {
        public string TargetCurrency { get; set; }
        public decimal ExchangeRate { get; set; }

        public decimal AmountConverted { get; set; }

    }
}
