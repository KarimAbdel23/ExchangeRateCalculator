using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Services;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExchangeRateService _exchangeRateService;

        public AccountController(AppDbContext context, ExchangeRateService exchangeRateService)
        {
            _context = context;
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {            
            var userDb = await _context.UsersDB.FirstOrDefaultAsync(u=> u.UserName == userName);
            
            if (userDb == null) return Unauthorized();            

            var hasher = new PasswordHasher<UserDB>();
            var result = hasher.VerifyHashedPassword(null, userDb.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed) return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDb.UserName),
                new Claim(ClaimTypes.Role, userDb.Rol),
                new Claim("MainCurrency", userDb.MainCurrency) // ¡Aquí se añade!
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            var currencyCatalog = await _exchangeRateService.GetCurrenciesAsync();

            var viewModel = new SignUpViewModel
            {
                CurrencyCatalog = currencyCatalog
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string userName, string password, string mainCurrency)
        {
            var exist = await _context.UsersDB.AnyAsync(u => u.UserName == userName);
            if (exist) return BadRequest("Usuario ya existe");

            var hasher = new PasswordHasher<UserDB>();
            var userDb = new UserDB
            {
                UserName = userName,
                Rol = "User",
                PasswordHash = hasher.HashPassword(null, password),
                MainCurrency = mainCurrency
            };

            _context.UsersDB.Add(userDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");

        }
        public IActionResult Index()
        {
            return View();
        }

        

    }
}
