using Infrastructure.Entities;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface IProductService
    {
        Task<ServiceStatus> CreateProduct(ICreateProductDto product);
        Task<ServiceStatus> DeleteProduct(string name);
        Task<IEnumerable<Ingridient>> GetAllIngridients();
        Task<IEnumerable<IProductDto>> GetAllProducts();
        Task<ServiceStatus> UpdateProduct(string oldName, string newName, decimal newPrice, ICollection<Ingridient> newIngridients, int newCategoryId);
    }
}