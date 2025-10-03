using KARB.Optimissa.Actinver.ExchangeRateCalculator.Models.Context;
using KARB.Optimissa.Actinver.ExchangeRateCalculator.Services;
using Microsoft.EntityFrameworkCore;

namespace KARB.Optimissa.Actinver.ExchangeRateCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Database Context
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Authentication with Cookie
            builder.Services.AddAuthentication("MyCookieAuth")
                .AddCookie("MyCookieAuth", options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            // Authorization Policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy => policy.RequireRole("Admin"));
            });

            // Filters
            builder.Services.AddScoped<BitacoraFilter>();
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.AddService<BitacoraFilter>();
            });

            // Services
            builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
            builder.Services.AddHttpClient<ExchangeRateService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
