using DatalagringUppgift.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder();
builder.ConfigureServices(services =>
{
    services.AddDbContext<BookingCatalogContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\elias\source\repos\Datalagring\DatalagringUppgift\Data\BookingCatalog.mdf;Integrated Security=True;Connect Timeout=30"));
});