using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

using System.Globalization;
namespace SalesWebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder services
            builder.Services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("SalesWebMvcContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found."),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMvcContext")),
                    b => b.MigrationsAssembly("SalesWebMvc"))
                );
            builder.Services.AddScoped<SeedingService>();
            builder.Services.AddScoped<SellerService>();
            builder.Services.AddScoped<Department>();
            builder.Services.AddScoped<SalesRecord>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // CultureInfo, Localization
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/select-language-culture?view=aspnetcore-8.0
            var ciList = new List<CultureInfo> {
                new("en-US")
                ,new("pt-BR")
                ,new("ja-JP")
            };
            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = ciList,
                SupportedUICultures = ciList,
                DefaultRequestCulture = new RequestCulture(ciList[0])
            };
            app.UseRequestLocalization(localizationOptions);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                // https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-6.0&tabs=visual-studio#seed-the-database
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<SalesWebMvcContext>();
                    var seedingService = new SeedingService(context);
                    seedingService.Seed();
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}