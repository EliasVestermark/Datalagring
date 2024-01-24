using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public class BookingRepository : BaseRepository<BookingEntity, BookingCatalogContext>, IBookingRepository
{
    private readonly BookingCatalogContext _context;

    public BookingRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }

    public override IEnumerable<BookingEntity> GetAll()
    {
        try
        {
            var result = _context.Set<BookingEntity>().Include("Client").Include("Location").Include("Status").Include("Participants").Include("Time").ToList();
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
