using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class ClientRepository : BaseRepository<ClientEntity>
{
    private readonly BookingCatalogContext _context;

    public ClientRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }
}
