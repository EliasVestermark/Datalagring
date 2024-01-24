using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly ProductRepository _productRepository;
    private readonly IngridientRepository _ingridientRepository;
    private readonly ProductCatalogContext _context;

    public ProductService(ProductRepository productRepository, IngridientRepository ingridientRepository, ProductCatalogContext context)
    {
        _productRepository = productRepository;
        _ingridientRepository = ingridientRepository;
        _context = context;
    }

    public ServiceStatus CreateProduct(ICreateProductDto product)
    {
        try
        {
            if (!_productRepository.Exists(x => x.Name == product.Name))
            {
                var ingridientsFromDb = new List<Ingridient>();

                foreach (var ingridient in product.Ingridients)
                {
                    var existingIngridient = _context.Set<Ingridient>().FirstOrDefault(i => i.Name == ingridient.Name);
                    ingridientsFromDb.Add(existingIngridient!);
                }

                var productEntity = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Ingridients = ingridientsFromDb
                };

                var result = _productRepository.Create(productEntity);

                if (result != null)
                {
                    return ServiceStatus.SUCCESS;
                }
            }
            else
            {
                return ServiceStatus.ALREADY_EXISTS;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return ServiceStatus.FAILED;
    }

    public IEnumerable<IProductDto> GetAllProducts()
    {
        var products = new List<IProductDto>();

        try
        {
            var result = _productRepository.GetAll();

            foreach (var item in result)
            {
                products.Add(new ProductDto
                {

                    Name = item.Name,
                    Price = item.Price,
                    Category = item.Category.Name!,
                    Ingridients = item.Ingridients
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return products;
    }

    public ServiceStatus UpdateProduct(string oldName, string newName, decimal newPrice, ICollection<Ingridient> newIngridients, int newCategoryId)
    {
        try
        {
            if (!_productRepository.Exists(x => x.Name == newName))
            {
                var ingridientsFromDb = new List<Ingridient>();

                foreach (var ingridient in newIngridients)
                {
                    var existingIngridient = _context.Set<Ingridient>().FirstOrDefault(i => i.Name == ingridient.Name);
                    ingridientsFromDb.Add(existingIngridient!);
                }

                var productEntity = _productRepository.GetOne(x => x.Name == oldName);

                productEntity = _productRepository.Update(productEntity.ProductId, new Product
                {
                    ProductId = productEntity.ProductId,
                    Name = newName,
                    Price = newPrice,
                    Ingridients = newIngridients,
                    CategoryId = newCategoryId
                });

                return ServiceStatus.UPDATED;
            }
            else
            {
                return ServiceStatus.ALREADY_EXISTS;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return ServiceStatus.FAILED;
    }

    public ServiceStatus DeleteProduct(string name)
    {
        try
        {
            if (_productRepository.Exists(x => x.Name == name))
            {
                var result = _productRepository.Delete(x => x.Name == name);

                if (result)
                {
                    return ServiceStatus.DELETED;
                }
                else
                {
                    return ServiceStatus.FAILED;
                }
            }
            else
            {
                return ServiceStatus.NOT_FOUND;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return ServiceStatus.FAILED;
    }

    public IEnumerable<Ingridient> GetAllIngridients()
    {
        var ingridients = new List<Ingridient>();

        try
        {
            var result = _ingridientRepository.GetAll();

            foreach (var item in result)
            {
                ingridients.Add(new Ingridient { Name = item.Name! });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return ingridients;
    }
}
