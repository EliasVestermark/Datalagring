using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Services;

public class ProductService_Test
{
    private readonly ProductCatalogContext _context = new(new DbContextOptionsBuilder<ProductCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task CreateProduct_ShouldCreateProductIfNoProductWithSameNameExist_ThenReturnServiceStatusSuccess()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto("testName", 10, ingridients, 1);

        //Act
        var result = await productService.CreateProduct(product);

        //Assert
        Assert.Equal(ServiceStatus.SUCCESS, result);
    }

    [Fact]
    public async Task CreateProduct_ShouldNotCreateProductSinceProductWithSameNameExist_ThenReturnServiceStatusAlreadyExists()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto("testName", 10, ingridients, 1);

        //Act
        await productService.CreateProduct(product);
        var result = await productService.CreateProduct(product);

        //Assert
        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result);
    }

    [Fact]
    public async Task CreateProduct_ShouldNotCreateProductDueToNullVariable_ThenReturnServiceStatusFailed()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto(null!, 10, ingridients, 1);

        //Act
        var result = await productService.CreateProduct(product);

        //Assert
        Assert.Equal(ServiceStatus.FAILED, result);
    }

    [Fact]
    public async Task GetAllProducts_ShouldGetAllProductsFromTheDataBase_ThenReturnAsIEnumerable()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto("testName", 10, ingridients, 1);
        await productService.CreateProduct(product);

        //Act
        var result = await productService.GetAllProducts();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<IProductDto>>(result);
    }

    [Fact]
    public async Task GetAllProducts_ShouldGetEmptyListFromTheDataBase_ThenReturnEmptyIEnumerable()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);

        //Act
        var result = await productService.GetAllProducts();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProductIfNoProductWithSameNameExist_ThenReturnServiceStatusUpdated()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto("testName", 10, ingridients, 1);
        await productService.CreateProduct(product);

        // Act
        var result = await productService.UpdateProduct("testName", "testName2", 20, ingridients, 2);

        //Assert
        Assert.Equal(ServiceStatus.UPDATED, result);
    }

    [Fact]
    public async Task UpdateProduct_ShouldNotUpdateProductSinceProductWithSameNameExist_ThenReturnServiceStatusAlreadyExist()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto("testName", 10, ingridients, 1);
        await productService.CreateProduct(product);

        // Act
        var result = await productService.UpdateProduct("testName", "testName", 20, ingridients, 2);

        //Assert
        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result);
    }

    [Fact]
    public async Task UpdateProduct_ShouldFailToUpdateProductDueToNoExistingProduct_ThenReturnServiceStatusFailed()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        // Act
        var result = await productService.UpdateProduct("testName", "testName", 20, ingridients, 2);

        //Assert
        Assert.Equal(ServiceStatus.FAILED, result);
    }

    [Fact]
    public async Task DeleteProduct_ShouldDeleteProductIfProductWithSameNameExist_ThenReturnServiceStatusDeleted()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);
        var ingridients = new List<Ingridient>();

        var product = new CreateProductDto("testName", 10, ingridients, 1);
        await productService.CreateProduct(product);

        // Act
        var result = await productService.DeleteProduct("testName");

        //Assert
        Assert.Equal(ServiceStatus.DELETED, result);
    }

    [Fact]
    public async Task DeleteProduct_ShouldNotDeleteProductSinceNoProductWithSameNameExist_ThenReturnServiceStatusNotFound()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);

        // Act
        var result = await productService.DeleteProduct("testName");

        //Assert
        Assert.Equal(ServiceStatus.NOT_FOUND, result);
    }

    [Fact]
    public async Task GetAllIngridients_ShouldGetAllIngridientsFromTheDataBase_ThenReturnAsIEnumerable()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);

        var ingridient = new Ingridient { Name = "testIngridient"};
        await ingridientRepository.Create(ingridient);

        //Act
        var result = await productService.GetAllIngridients();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<Ingridient>>(result);
    }

    [Fact]
    public async Task GetAllIngridients_ShouldGetEmptyListFromTheDataBase_ThenReturnEmptyIEnumerable()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var ingridientRepository = new IngridientRepository(_context);
        var productService = new ProductService(productRepository, ingridientRepository, _context);

        //Act
        var result = await productService.GetAllIngridients();

        //Assert
        Assert.Empty(result);
    }
}
