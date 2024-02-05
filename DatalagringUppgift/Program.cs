using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PresentationDatalagringUppgift.Interfaces;
using PresentationDatalagringUppgift.Services;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        // Use in-memory database for testing
        services.AddDbContext<BookingCatalogContext>(x => x.UseInMemoryDatabase($"{Guid.NewGuid()}"));
        services.AddDbContext<ProductCatalogContext>(x => x.UseInMemoryDatabase($"{Guid.NewGuid()}"));
    }
    else
    {
        // Use SQL Server database for the actual application
        services.AddDbContext<BookingCatalogContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\elias\source\repos\Datalagring\Infrastructure\Data\BookingCatalog.mdf;Integrated Security=True;Connect Timeout=30"));
        services.AddDbContext<ProductCatalogContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\elias\source\repos\Datalagring\Infrastructure\Data\ProductCatalog.mdf;Integrated Security=True;Connect Timeout=30"));
    }
    services.AddScoped<ClientRepository>();
    services.AddScoped<LocationRepository>();
    services.AddScoped<BookingRepository>();
    services.AddScoped<ProductRepository>();
    services.AddScoped<IngridientRepository>();
    services.AddScoped<IBookingService, BookingService>();
    services.AddScoped<IProductService, ProductService>();
    services.AddSingleton<IMenuService, MenuService>();
    services.AddSingleton<IValidationService, ValidationService>();
}).Build();

builder.Start();

Console.Clear();

var menuService = builder.Services.GetRequiredService<IMenuService>();

await menuService.ShowMainMenu();

