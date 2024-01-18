using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class BookingRepository : BaseRepository<BookingEntity>
{
    private readonly BookingCatalogContext _context;

    public BookingRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }
}
