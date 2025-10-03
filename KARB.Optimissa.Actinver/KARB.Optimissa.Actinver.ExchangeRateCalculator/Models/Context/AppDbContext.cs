using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UserDB> UsersDB { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }
        public DbSet<FavoriteCurrency> FavoriteCurrencies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            var hasher = new PasswordHasher<UserDB>();

            modelBuilder.Entity<UserDB>().HasData(new UserDB
            {
                Id = 1,
                UserName = "actinver",
                //"admin123"
                PasswordHash = "AQAAAAIAAYagAAAAEEaEKUjWtSiprRFec3aXtjQoyAxVuxC3BkOSxfYBPafOKa53ZzB6Uhz6CW4NN2jGKg==",
                Rol = "Admin",
                MainCurrency = "MXN"
            });

            modelBuilder.Entity<FavoriteCurrency>()
                .HasOne(m => m.UserDb)
                .WithMany()
                .HasForeignKey(m => m.UserId);
        }
    }
}
