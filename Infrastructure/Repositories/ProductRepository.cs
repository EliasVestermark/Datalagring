using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product, ProductCatalogContext>, IProductRepository
    {
        private readonly ProductCatalogContext _context;

        public ProductRepository(ProductCatalogContext context) : base(context)
        {
            _context = context;
        }

        public override IEnumerable<Product> GetAll()
        {
            try
            {
                var result = _context.Set<Product>().Include("Category").Include("Ingridients").ToList();
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }
    }
}
