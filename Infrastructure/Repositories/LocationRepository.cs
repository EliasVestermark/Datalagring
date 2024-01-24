using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;
public class LocationRepository : BaseRepository<LocationEntity, BookingCatalogContext>, ILocationRepository
{
    private readonly BookingCatalogContext _context;

    public LocationRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }
}
