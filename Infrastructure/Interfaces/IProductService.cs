using Infrastructure.Entities;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface IProductService
    {
        ServiceStatus CreateProduct(ICreateProductDto product);
        ServiceStatus DeleteProduct(string name);
        IEnumerable<Ingridient> GetAllIngridients();
        IEnumerable<IProductDto> GetAllProducts();
        ServiceStatus UpdateProduct(string oldName, string newName, decimal newPrice, ICollection<Ingridient> newIngridients, int newCategoryId);
    }
}