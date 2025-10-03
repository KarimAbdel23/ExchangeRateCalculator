
namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Extensions
{
    public static class ConversionExtensions
    {
        public static string ShowRate(this decimal rate, string origin, string destination)
        {
            return $"1 {origin} = {rate:F4} {destination}";
        }

        public static string ShowConvertedAmount(this decimal amount, string destination)
        {
            return $"{amount:F2} {destination}";
        }
    }
}
