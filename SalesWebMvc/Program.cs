using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace SalesWebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {



            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SalesWebMvcContext>(options =>
             options.UseMySql(
              builder.Configuration.GetConnectionString("SalesWebMVCContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMVCContext' not found."),
              ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMVCContext")),
              b => b.MigrationsAssembly("SalesWebMVC"))
              );



            builder.Services.AddControllersWithViews();


            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
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