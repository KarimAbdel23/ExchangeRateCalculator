using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Services;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Controllers
{
    public class CurrencyManagementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyManagementController(AppDbContext context, ExchangeRateService exchangeRateService)
        {
            _context = context;
            _exchangeRateService = exchangeRateService;
        }

        [Authorize] //Solo administradores (Roles = "Admin")
        public async Task<IActionResult> Index()
        {
            var user = await _context.UsersDB                
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var catalog = await _exchangeRateService.GetCurrenciesAsync();            

            var favoriteCurrencies = _context.FavoriteCurrencies
                .Where(m => m.UserId == user.Id)
                .Select(c => c.CurrencyCode)
                .ToList();

            var viewModel = new CurrencyManagementViewModel
            {
                MainCurrency = user.MainCurrency,
                FavoriteCurrencies = favoriteCurrencies,
                CurrencyCatalog = catalog
            };
            
            ViewBag.MainCurrencyList = new SelectList(catalog, "CurrencySymbol", "FullName", user.MainCurrency);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(string mainCurrency, List<string> favoriteCurrencies) 
        {
            var user = await _context.UsersDB.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return Unauthorized();
            }

            // 1. Actualiza la moneda principal
            user.MainCurrency = mainCurrency;

            // 2. Elimina las monedas favoritas anteriores
            var favoritasAnteriores = _context.FavoriteCurrencies
                .Where(m => m.UserId == user.Id);
            _context.FavoriteCurrencies.RemoveRange(favoritasAnteriores);

            // 3. Agrega las nuevas monedas favoritas
            if (favoriteCurrencies != null) { 
                int priority = 1;
                foreach (var favoriteCode in favoriteCurrencies)
                {
                    var favoriteCurrency = new FavoriteCurrency
                    {
                        UserId = user.Id,
                        CurrencyCode = favoriteCode,
                        DateAdded = DateTime.Now,
                        Priority = priority
                    };
                    _context.FavoriteCurrencies.Add(favoriteCurrency);
                    priority++;
                }
            }

            // 4. Guarda todos los cambios.
            await _context.SaveChangesAsync();

            TempData["Message"] = "Configuracion guardada correctamente.";               

            return RedirectToAction("Index");
        }
    }
}
