using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repositories;

public class ProductRepository_Tests
{
    private readonly ProductCatalogContext _context = new(new DbContextOptionsBuilder<ProductCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);


    [Fact]
    public async Task Create_ShouldCreateProductEntityToDatabase_ThenReturnThatTEntityWithId()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new Product { Name = "TestName", Price = 20, CategoryId = 1, Ingridients = new List<Ingridient>() };

        // Act
        var result = await productRepository.Create(productEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductId);
    }

    [Fact]
    public async Task Create_ShouldCreateFalutyProductEntityToDatabase_ThenReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new Product();

        // Act
        var result = await productRepository.Create(productEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetListOfAllProductsFromDatabase_ThenReturnList()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };
        var productEntity = new Product { Name = "TestName", Price = 20, Category = categoryEntity, Ingridients = new List<Ingridient>() };
        await productRepository.Create(productEntity);

        // Act
        var result = await productRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Product>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetEmptyListOfAllProductsFromDatabase_ThenReturnEmptyList()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);

        // Act
        var result = await productRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOne_ShouldGetOneProductFromListByName_ThenReturnThatProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new Product { Name = "TestName", Price = 20, CategoryId = 1, Ingridients = new List<Ingridient>() };
        await productRepository.Create(productEntity);

        // Act
        var result = await productRepository.GetOne(x => x.Name == productEntity.Name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productEntity, result);
    }

    [Fact]
    public async Task GetOne_ShouldFailToGetOneProductFromListByName_ThenReturnThatProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var productEntity = new Product { Name = "TestName", Price = 20, CategoryId = 1, Ingridients = new List<Ingridient>() };
        await productRepository.Create(productEntity);

        // Act
        var result = await productRepository.GetOne(x => x.Name == "NameTest");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldUpdateNameOfOneExistsingProductFromListById_ThenReturnUpdatedProduct()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };

        var carrot = await ingridientRepository.Create(new Ingridient { Name = "Carrot" });
        var lettuce = await ingridientRepository.Create(new Ingridient { Name = "Lettuce" });
        var oldIngridients = new List<Ingridient> { carrot };
        var newIngridients = new List<Ingridient> { lettuce };

        var oldProductEntity = new Product { ProductId = 1, Name = "TestName", Price = 20, CategoryId = 1, Category = categoryEntity, Ingridients = oldIngridients };
        var updatedProductEntity = new Product { ProductId = 1, Name = "TestName2", Price = 20, CategoryId = 1, Category = categoryEntity, Ingridients = newIngridients };
        await productRepository.Create(oldProductEntity);

        // Act
        var result = await productRepository.Update(1, updatedProductEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedProductEntity.Ingridients, result.Ingridients);
        Assert.Equal(updatedProductEntity.Name, result.Name);
    }

    [Fact]
    public async Task Update_ShouldTryToUpdateNameOfNoneExistsingProductFromListById_ThenReturnNull()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };
        var updatedProductEntity = new Product { ProductId = 1, Name = "TestName2", Price = 20, CategoryId = 1, Category = categoryEntity, Ingridients = new List<Ingridient>() };

        // Act
        var result = await productRepository.Update(1, updatedProductEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteProductFromListByName_ThenReturnTrue()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };
        var productEntity = new Product { Name = "TestName", Price = 20, CategoryId = 1, Category = categoryEntity, Ingridients = new List<Ingridient>() };
        await productRepository.Create(productEntity);

        // Act
        var result = await productRepository.Delete(x => x.Name == "TestName");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_ShouldTryToDeleteNoneExistingProductFromListByName_ThenReturnFalse()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);

        // Act
        var result = await productRepository.Delete(x => x.Name == "TestName");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfProductExistsByName_ThenReturnTrue()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };
        var productEntity = new Product { Name = "TestName", Price = 20, CategoryId = 1, Category = categoryEntity, Ingridients = new List<Ingridient>() };
        await productRepository.Create(productEntity);

        // Act
        var result = await productRepository.Exists(x => x.Name == "TestName");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfNoneExistingProductExistsByName_ThenReturnFalse()
    {
        // Arrange
        IProductRepository productRepository = new ProductRepository(_context);

        // Act
        var result = await productRepository.Exists(x => x.Name == "TestName");

        // Assert
        Assert.False(result);
    }
}
