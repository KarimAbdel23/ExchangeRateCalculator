using System.ComponentModel.DataAnnotations;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models
{
    public class FavoriteCurrency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(3)]
        public string CurrencyCode { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int Priority { get; set; }

        public UserDB UserDb { get; set; }
    }
}
