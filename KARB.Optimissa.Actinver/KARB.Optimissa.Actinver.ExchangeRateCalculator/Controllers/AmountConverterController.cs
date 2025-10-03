using KARB.Optimissa.Actinver.ExchangeRateCalculator.Extensions;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Services;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Controllers
{
    public class AmountConverterController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExchangeRateService _exchangeRateService;

        public AmountConverterController(AppDbContext context, ExchangeRateService exchangeRateService)
        {
            _context = context;
            _exchangeRateService = exchangeRateService;            
        }

        [Authorize] // Requiere estar autenticado
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _context.UsersDB.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null || string.IsNullOrEmpty(user.MainCurrency))
                return RedirectToAction("Login", "Account");
         
            var viewModel = new AmountConverterViewModel
            {
                SelectedDate = DateTime.Today,
                MainCurrency = user.MainCurrency,
                Conversions = new List<Conversion>()
            };

            return View(viewModel);
        }

        [Authorize] // Requiere estar autenticado
        [HttpPost]
        public async Task<IActionResult> Index(DateTime? selectedDate, decimal amountEntered)
        {
            var user = await _context.UsersDB.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
                return Unauthorized();

            var queryDate = selectedDate ?? DateTime.Today;
            var conversions = new List<Conversion>();

            var favoriteCurrencies = _context.FavoriteCurrencies.Where(f => f.UserId == user.Id).Select(s => s.CurrencyCode);

            if (favoriteCurrencies.Any())
            {
                string favoriteCurrenciesWithComma = string.Join(",", favoriteCurrencies);

                //var response = await _exchangeRateService.GetExchangeRatesByDateAndAmount(queryDate, amountEntered, user.MainCurrency, favoriteCurrenciesWithComma);
                var response = await _exchangeRateService.GetExchangeRatesByDate(queryDate, user.MainCurrency, favoriteCurrenciesWithComma);

                foreach (var item in response.Rates)
                {
                    conversions.Add(new Conversion
                    {
                        TargetCurrency = item.Key,
                        ExchangeRate = item.Value,
                        AmountConverted = item.Value * amountEntered
                    });
                }
            }

            var viewModel = new AmountConverterViewModel
            {
                SelectedDate = queryDate,
                MainCurrency = user.MainCurrency,
                Conversions = conversions,
                AmountEntered = amountEntered
            };

            return View(viewModel);
        }
    }
}
