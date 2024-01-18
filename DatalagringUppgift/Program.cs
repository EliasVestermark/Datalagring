using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<BookingCatalogContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\elias\source\repos\Datalagring\Infrastructure\Data\BookingCatalog.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddScoped<ClientRepository>();
    services.AddScoped<LocationRepository>();
    services.AddScoped<BookingRepository>();
    services.AddScoped<BookingService>();
}).Build();

builder.Start();

Console.Clear();

var bookingService = builder.Services.GetRequiredService<BookingService>();

var resultList = bookingService.GetAllBookings();

foreach (var item in resultList)
{
    Console.WriteLine(item.FirstName);
    Console.WriteLine(item.LastName);
    Console.WriteLine(item.PhoneNumber);
    Console.WriteLine(item.Email);
    Console.WriteLine("");
    Console.WriteLine(item.Address);
    Console.WriteLine(item.PostalCode);
    Console.WriteLine(item.City);
    Console.WriteLine("");
    Console.WriteLine(item.Status);
    Console.WriteLine(item.Participants);
    Console.WriteLine(item.Time);
    Console.WriteLine("");
    Console.WriteLine("");
}


string firstName = "Emma";
string lastName = "Mellström";
string phoneNumber = "1234567890";
string email = "emma@gmail.com";
string address = "Vägen";
string postalcode = "34533";
string city = "Stan";
string date = "2024-03-31";
int statusId = 1;
int participantsId = 3;
int timeId = 2;

var result = bookingService.CreateBooking(new CreateBookingDto(firstName, lastName, phoneNumber, email, address, postalcode, city, date, statusId, participantsId, timeId));

if (result)
{
    Console.WriteLine("Lyckades");
}
else
{
    Console.WriteLine("Redan tillagd");
}

Console.ReadKey();
