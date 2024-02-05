using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repositories;

public class ClientRepository_Test
{
    private readonly BookingCatalogContext _context = new(new DbContextOptionsBuilder<BookingCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task Create_ShouldCreateClientEntityToDatabase_ThenReturnThatTEntityWithId()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };

        // Act
        var result = await clientRepository.Create(clientEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task Create_ShouldCreateFalutyClientEntityToDatabase_ThenReturnNull()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity();

        // Act
        var result = await clientRepository.Create(clientEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetListOfAllClientsFromDatabase_ThenReturnList()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        await clientRepository.Create(clientEntity);

        // Act
        var result = await clientRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ClientEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetEmptyListOfAllClientsFromDatabase_ThenReturnEmptyList()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);

        // Act
        var result = await clientRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOne_ShouldGetOneClientFromListByEmail_ThenReturnThatClient()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        await clientRepository.Create(clientEntity);

        // Act
        var result = await clientRepository.GetOne(x => x.Email == clientEntity.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientEntity, result);
    }

    [Fact]
    public async Task GetOne_ShouldFailToGetOneClientFromListByEmail_ThenReturnThatClient()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        await clientRepository.Create(clientEntity);

        // Act
        var result = await clientRepository.GetOne(x => x.Email == "WrongEmail");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldUpdateAddressOfOneExistsingClientFromListByEmail_ThenReturnUpdatedClient()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var oldClientEntity = new ClientEntity { Id = 1, FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        var updatedClientEntity = new ClientEntity { Id = 1, FirstName = "TestFirstName2", LastName = "TestLastName2", PhoneNumber = "9999999", Email = "test2@domain.com" };
        await clientRepository.Create(oldClientEntity);

        // Act
        var result = await clientRepository.Update(1, updatedClientEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedClientEntity.Email, result.Email);
    }

    [Fact]
    public async Task Update_ShouldTryToUpdateNameOfNoneExistsingClientFromListByEmail_ThenReturnNull()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity {Id = 1, FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };

        // Act
        var result = await clientRepository.Update(1, clientEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteClientFromListByEmail_ThenReturnTrue()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        await clientRepository.Create(clientEntity);

        // Act
        var result = await clientRepository.Delete(x => x.Email == clientEntity.Email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_ShouldTryToDeleteNoneExistingClientFromListByEmail_ThenReturnFalse()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);

        // Act
        var result = await clientRepository.Delete(x => x.Email == "WrongEmail");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfClientExistsByEmail_ThenReturnTrue()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);
        var clientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        await clientRepository.Create(clientEntity);

        // Act
        var result = await clientRepository.Exists(x => x.Email == clientEntity.Email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfNoneExistingClientExistsByEmail_ThenReturnFalse()
    {
        // Arrange
        IClientRepository clientRepository = new ClientRepository(_context);

        // Act
        var result = await clientRepository.Exists(x => x.Email == "test@domain.com");

        // Assert
        Assert.False(result);
    }
}
