using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public class BookingRepository : BaseRepository<BookingEntity, BookingCatalogContext>, IBookingRepository
{
    private readonly BookingCatalogContext _context;

    public BookingRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<BookingEntity>> GetAll()
    {
        try
        {
            var result = await _context.Boookings.Include(i => i.Client).Include(i => i.Location).Include(i => i.Status).Include(i => i.Participants).Include(i => i.Time).ToListAsync();
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
