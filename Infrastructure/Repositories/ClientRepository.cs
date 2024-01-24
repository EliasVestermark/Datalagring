using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;
public class ClientRepository : BaseRepository<ClientEntity, BookingCatalogContext>, IClientRepository
{
    private readonly BookingCatalogContext _context;

    public ClientRepository(BookingCatalogContext context) : base(context)
    {
        _context = context;
    }
}
