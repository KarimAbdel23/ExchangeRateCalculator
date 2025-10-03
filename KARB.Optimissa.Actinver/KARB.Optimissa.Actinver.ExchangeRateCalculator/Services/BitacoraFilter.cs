using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator.Services
{
    public class BitacoraFilter : IActionFilter
    {
        private readonly AppDbContext _context;

        public BitacoraFilter(AppDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var ruta = httpContext.Request.Path;
            var metodo = httpContext.Request.Method;
            var controlador = context.RouteData.Values["controller"]?.ToString();
            var usuario = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "Anónimo";
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();

            var bitacora = new Bitacora
            {
                User = usuario,
                Method = metodo,
                Controller = controlador,
                Route = ruta,
                Date = DateTime.Now,
                IP = ip
            };

            _context.Bitacoras.Add(bitacora);
            _context.SaveChanges();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se necesita lógica aquí para este caso
        }


    }
}
