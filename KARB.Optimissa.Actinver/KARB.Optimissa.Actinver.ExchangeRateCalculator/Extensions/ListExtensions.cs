namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Extensions
{
    public static class ListExtensions
    {
        public static string JoinByCommas(this List<string> list)
        {
            return string.Join(",", list);
        }
    }
}
