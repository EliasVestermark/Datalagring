using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Dapper.SqlMapper;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product, ProductCatalogContext>, IProductRepository
    {
        private readonly ProductCatalogContext _context;

        public ProductRepository(ProductCatalogContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                var result = await _context.Products.Include(i => i.Category).Include(i => i.Ingridients).ToListAsync();
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }

        public override async Task<Product> Update(int id, Product entity)
        {
            try
            {
                var entityToUpdate = await _context.Set<Product>().FindAsync(id);

                entityToUpdate!.Ingridients.Clear();

                foreach (var ingridient in entity.Ingridients)
                {
                    var existingIngridient = await _context.Set<Ingridient>().FirstOrDefaultAsync(i => i.Name == ingridient.Name);

                    if (existingIngridient != null)
                    {
                        entityToUpdate.Ingridients.Add(existingIngridient);
                    }
                }

                if (entityToUpdate != null)
                {
                    _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                    await _context.SaveChangesAsync();

                    return entityToUpdate;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }
    }
}
