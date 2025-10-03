using System.ComponentModel.DataAnnotations;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models
{
    public class UserDB
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Rol { get; set; } // "Admin" or "User"
        [Required]
        public string MainCurrency { get; set; }
    }
}
