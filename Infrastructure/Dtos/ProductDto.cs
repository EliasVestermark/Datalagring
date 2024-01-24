using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Dtos;

public class ProductDto : IProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public ICollection<Ingridient> Ingridients { get; set; } = null!;
}
