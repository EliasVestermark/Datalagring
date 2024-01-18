using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class LocationRepository : BaseRepository<LocationEntity>
{
    private readonly BookingCatalogContext _context;

    public LocationRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }
}
