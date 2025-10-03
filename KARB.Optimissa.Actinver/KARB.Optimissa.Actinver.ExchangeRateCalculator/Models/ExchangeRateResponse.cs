namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models
{
    public class ExchangeRateResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
