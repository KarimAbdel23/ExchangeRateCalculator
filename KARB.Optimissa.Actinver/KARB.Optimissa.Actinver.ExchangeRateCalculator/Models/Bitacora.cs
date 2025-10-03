namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models
{
    public class Bitacora
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
        public string Route { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }
    }
}
