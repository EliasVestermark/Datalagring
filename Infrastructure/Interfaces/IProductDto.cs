using Infrastructure.Entities;

namespace Infrastructure.Interfaces
{
    public interface IProductDto
    {
        string Category { get; set; }
        ICollection<Ingridient> Ingridients { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
    }
}