using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repositories;

public class LocationRepository_Test
{
    private readonly BookingCatalogContext _context = new(new DbContextOptionsBuilder<BookingCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);


    [Fact]
    public async Task Create_ShouldCreateLocationEntityToDatabase_ThenReturnThatTEntityWithId()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var locationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12312", City = "TestCity"};

        // Act
        var result = await locationRepository.Create(locationEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task Create_ShouldCreateFalutyLocationEntityToDatabase_ThenReturnNull()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var locationEntity = new LocationEntity();

        // Act
        var result = await locationRepository.Create(locationEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetListOfAllLocationsFromDatabase_ThenReturnList()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var locationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12312", City = "TestCity" };
        await locationRepository.Create(locationEntity);

        // Act
        var result = await locationRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<LocationEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetEmptyListOfAllLocationsFromDatabase_ThenReturnEmptyList()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);

        // Act
        var result = await locationRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOne_ShouldGetOneLocationFromListByAddressAndPostalCode_ThenReturnThatLocation()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var locationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12312", City = "TestCity" };
        await locationRepository.Create(locationEntity);

        // Act
        var result = await locationRepository.GetOne(x => x.PostalCode == locationEntity.PostalCode && x.Address == locationEntity.Address);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(locationEntity, result);
    }

    [Fact]
    public async Task GetOne_ShouldFailToGetOneLocationFromListByAddressAndPostalCode_ThenReturnThatLocation()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var locationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12312", City = "TestCity" };
        await locationRepository.Create(locationEntity);

        // Act
        var result = await locationRepository.GetOne(x => x.PostalCode == "WrongPostalCode" && x.Address == "WrongAddress");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldUpdateAddressOfOneExistsingLocationFromListByAddressAndPostalCode_ThenReturnUpdatedLocation()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var oldLocationEntity = new LocationEntity { Id = 1, Address = "TestAddress", PostalCode = "12312", City = "TestCity" };
        var updatedLocationEntity = new LocationEntity { Id = 1, Address = "TestAddress2", PostalCode = "99999", City = "TestCity2" };
        await locationRepository.Create(oldLocationEntity);

        // Act
        var result = await locationRepository.Update(1, updatedLocationEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedLocationEntity.Address, result.Address);
    }

    [Fact]
    public async Task Update_ShouldTryToUpdateNameOfNoneExistsingLocationFromListByAddressAndPostalCode_ThenReturnNull()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var updatedLocationEntity = new LocationEntity {Id = 1, Address = "TestAddress", PostalCode = "12312", City = "TestCity" };

        // Act
        var result = await locationRepository.Update(1, updatedLocationEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteLocationFromListByAddressAndPostalCode_ThenReturnTrue()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var locationEntity = new LocationEntity {Address = "TestAddress", PostalCode = "12312", City = "TestCity" };
        await locationRepository.Create(locationEntity);

        // Act
        var result = await locationRepository.Delete(x => x.PostalCode == "12312" && x.Address == "TestAddress");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_ShouldTryToDeleteNoneExistingLocationFromListByAddressAndPostalCode_ThenReturnFalse()
    {
        // Arrange
        ILocationRepository LocationRepository = new LocationRepository(_context);

        // Act
        var result = await LocationRepository.Delete(x => x.PostalCode == "12312" && x.Address == "TestAddress");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfLocationExistsByAddressAndPostalCode_ThenReturnTrue()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);
        var categoryEntity = new Category { CategoryId = 1, Name = "Test" };
        var locationEntity = new LocationEntity {Address = "TestAddress", PostalCode = "12312", City = "TestCity" };
        await locationRepository.Create(locationEntity);

        // Act
        var result = await locationRepository.Exists(x => x.PostalCode == "12312" && x.Address == "TestAddress");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfNoneExistingLocationExistsByAddressAndPostalCode_ThenReturnFalse()
    {
        // Arrange
        ILocationRepository locationRepository = new LocationRepository(_context);

        // Act
        var result = await locationRepository.Exists(x => x.PostalCode == "12312" && x.Address == "TestAddress");

        // Assert
        Assert.False(result);
    }
}
