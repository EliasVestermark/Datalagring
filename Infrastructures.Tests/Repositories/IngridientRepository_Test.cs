using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repositories;

public class IngridientRepository_Test
{
    private readonly ProductCatalogContext _context = new(new DbContextOptionsBuilder<ProductCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);


    [Fact]
    public async Task Create_ShouldCreateIngridientEntityToDatabase_ThenReturnThatEntityWithId()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity = new Ingridient { Name = "TestName" };

        // Act
        var result = await ingridientRepository.Create(ingridientEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.IngridientId);
    }

    [Fact]
    public async Task Create_ShouldCreateDuplicateIngridientEntityToDatabase_ThenReturnNull()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity1 = new Ingridient { IngridientId = 1 };
        var ingridientEntity2 = new Ingridient { IngridientId = 1 };

        // Act
        await ingridientRepository.Create(ingridientEntity1);
        var result = await ingridientRepository.Create(ingridientEntity2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetListOfAllIngridientsFromDatabase_ThenReturnList()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity = new Ingridient { Name = "TestName"};
        await ingridientRepository.Create(ingridientEntity);

        // Act
        var result = await ingridientRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Ingridient>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetEmptyListOfAllIngridientsFromDatabase_ThenReturnEmptyList()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);

        // Act
        var result = await ingridientRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOne_ShouldGetOneIngridientFromListByName_ThenReturnThatIngridient()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity = new Ingridient { Name = "TestName"};
        await ingridientRepository.Create(ingridientEntity);

        // Act
        var result = await ingridientRepository.GetOne(x => x.Name == ingridientEntity.Name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ingridientEntity, result);
    }

    [Fact]
    public async Task GetOne_ShouldFailToGetOneIngridientFromListByName_ThenReturnThatIngridient()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity = new Ingridient { Name = "TestName"};
        await ingridientRepository.Create(ingridientEntity);

        // Act
        var result = await ingridientRepository.GetOne(x => x.Name == "NameTest");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldUpdateNameOfOneExistsingIngridientFromListById_ThenReturnUpdatedIngridient()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var oldIngridientEntity = new Ingridient { Name = "TestName"};
        var updatedIngridientEntity = new Ingridient { IngridientId = 1, Name = "TestName2"};
        await ingridientRepository.Create(oldIngridientEntity);

        // Act
        var result = await ingridientRepository.Update(1, updatedIngridientEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedIngridientEntity.Name, result.Name);
    }

    [Fact]
    public async Task Update_ShouldTryToUpdateNameOfNoneExistsingIngridientFromListById_ThenReturnNull()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };
        var updatedProductEntity = new Ingridient {IngridientId = 1, Name = "TestName2" };

        // Act
        var result = await ingridientRepository.Update(1, updatedProductEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteIngridientFromListByName_ThenReturnTrue()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity = new Ingridient { Name = "TestName"};
        await ingridientRepository.Create(ingridientEntity);

        // Act
        var result = await ingridientRepository.Delete(x => x.Name == "TestName");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_ShouldTryToDeleteNoneExistingIngridientFromListByName_ThenReturnFalse()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);

        // Act
        var result = await ingridientRepository.Delete(x => x.Name == "TestName");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfIngridientExistsByName_ThenReturnTrue()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);
        var ingridientEntity = new Ingridient { Name = "TestName"};
        await ingridientRepository.Create(ingridientEntity);

        // Act
        var result = await ingridientRepository.Exists(x => x.Name == "TestName");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfNoneExistingIngridientExistsByName_ThenReturnFalse()
    {
        // Arrange
        IIngridientRepository ingridientRepository = new IngridientRepository(_context);

        // Act
        var result = await ingridientRepository.Exists(x => x.Name == "TestName");

        // Assert
        Assert.False(result);
    }
}
