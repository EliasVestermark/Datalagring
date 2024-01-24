using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class IngridientRepository : BaseRepository<Ingridient, ProductCatalogContext>, IIngridientRepository
{
    private readonly ProductCatalogContext _context;

    public IngridientRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }
}
