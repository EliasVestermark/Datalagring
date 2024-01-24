using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Dtos;

public class CreateProductDto(string name, decimal price, ICollection<Ingridient> ingridients, int categoryId) : ICreateProductDto
{
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public ICollection<Ingridient> Ingridients { get; set; } = ingridients;
    public int CategoryId { get; set; } = categoryId;
}
