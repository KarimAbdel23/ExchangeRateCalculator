namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsValidDate(this DateTime date)
        {
            return date <= DateTime.Today && date >= DateTime.MinValue;
        }
    }
}
