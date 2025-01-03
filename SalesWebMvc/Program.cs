using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
var builder = WebApplication.CreateBuilder(args);

string mySqlConnectionStr = builder.Configuration.GetConnectionString("SalesWebMvcContext");

// 2. Verifica se a string de conexão foi encontrada (IMPORTANTE)
if (string.IsNullOrEmpty(mySqlConnectionStr))
{
    throw new InvalidOperationException("A string de conexão 'SalesWebMvcContext' não foi encontrada no appsettings.json.");
}
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr), b =>
        b.MigrationsAssembly("SalesWebMvc")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.aaaaaaa
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
