using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Services;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ExchangeRateService _exchangeRateService;

        public DashboardController(AppDbContext context, ExchangeRateService exchangeRateService)
        {
            _context = context;
            _exchangeRateService = exchangeRateService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _context.UsersDB.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null || string.IsNullOrEmpty(user.MainCurrency))
                return RedirectToAction("Login", "Account");

            var conversions = new List<Conversion>();

            var favoriteCurrencies = _context.FavoriteCurrencies.Where(f => f.UserId == user.Id).Select(x => x.CurrencyCode);

            DateTime dateLatest = DateTime.Today;
            if (favoriteCurrencies.Any())
            {
                string favoriteCurrenciesWithComma = string.Join(",", favoriteCurrencies);

                var response = await _exchangeRateService.GetExchangeRatesBySymbos(user.MainCurrency, favoriteCurrenciesWithComma);

                foreach (var item in response.Rates)
                {
                    conversions.Add(new Conversion
                    {
                        TargetCurrency = item.Key,
                        ExchangeRate = item.Value
                    });
                }
            }

            var viewModel = new DashboardViewModel
            {
                MainCurrency = user.MainCurrency,
                DateLatest = dateLatest,
                Conversions = conversions
            };

            return View(viewModel);
        }
    }
}
