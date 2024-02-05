using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repositories;

public class BookingRepository_Test
{
    private readonly BookingCatalogContext _context = new(new DbContextOptionsBuilder<BookingCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task Create_ShouldCreateBookingEntityToDatabase_ThenReturnThatTEntityWithId()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var bookingEntity = new BookingEntity { Id = 1, ClientId = 1, LocationId = 1, Date = "2024-01-01", ParticipantsId = 1, StatusId = 1, TimeId = 1};

        // Act
        var result = await bookingRepository.Create(bookingEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task Create_ShouldCreateFalutyBookingEntityToDatabase_ThenReturnNull()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var bookingEntity = new BookingEntity();

        // Act
        var result = await bookingRepository.Create(bookingEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetListOfAllBookingsFromDatabase_ThenReturnList()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var locationEntity = new LocationEntity { Id = 1, Address = "Test", PostalCode = "12345", City = "Test"};
        var clientEntity = new ClientEntity { Id = 1, FirstName = "Test", LastName = "Test", PhoneNumber = "1234567", Email = "test@domain.com" };
        var participantsEntity = new ParticipantsEntity { Id = 1, Amount = "10-19" };
        var statusEntity = new StatusEntity { Id = 1, StatusText = "Booked" };
        var timeEntity = new TimeEntity { Id = 1, StartTime = "0800", EndTime = "1200" };
        var bookingEntity = new BookingEntity { Id = 1, Client = clientEntity, Location = locationEntity, Date = "2024-01-01", Participants = participantsEntity, Status = statusEntity, Time = timeEntity };
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingRepository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<BookingEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAll_ShouldGetEmptyListOfAllBookingsFromDatabase_ThenReturnEmptyList()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);

        // Act
        var result = await bookingRepository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOne_ShouldGetOneBookingFromListByName_ThenReturnThatBooking()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var bookingEntity = new BookingEntity { Id = 1, ClientId = 1, LocationId = 1, Date = "2024-01-01", ParticipantsId = 1, StatusId = 1, TimeId = 1 };
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingRepository.GetOne(x => x.Date == bookingEntity.Date);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookingEntity, result);
    }

    [Fact]
    public async Task GetOne_ShouldFailToGetOneBookingFromListByName_ThenReturnThatBooking()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var bookingEntity = new BookingEntity { Id = 1, ClientId = 1, LocationId = 1, Date = "2024-01-01", ParticipantsId = 1, StatusId = 1, TimeId = 1 };
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingRepository.GetOne(x => x.Date == "DateTest");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ShouldUpdateDateOfOneExistsingBookingFromListById_ThenReturnUpdatedBooking()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var locationEntity = new LocationEntity { Id = 1, Address = "Test", PostalCode = "12345", City = "Test" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "Test", LastName = "Test", PhoneNumber = "1234567", Email = "test@domain.com" };
        var participantsEntity = new ParticipantsEntity { Id = 1, Amount = "10-19" };
        var statusEntity = new StatusEntity { Id = 1, StatusText = "Booked" };
        var timeEntity = new TimeEntity { Id = 1, StartTime = "0800", EndTime = "1200" };
        var oldBookingEntity = new BookingEntity { Id = 1, Client = clientEntity, Location = locationEntity, Date = "2024-01-01", Participants = participantsEntity, Status = statusEntity, Time = timeEntity }; 
        var updatedBookingEntity = new BookingEntity { Id = 1, Client = clientEntity, Location = locationEntity, Date = "2025-05-05", Participants = participantsEntity, Status = statusEntity, Time = timeEntity };
        await bookingRepository.Create(oldBookingEntity);

        // Act
        var result = await bookingRepository.Update(1, updatedBookingEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBookingEntity.Date, result.Date);
    }

    [Fact]
    public async Task Update_ShouldTryToUpdateDateOfNoneExistsingBookingFromListById_ThenReturnNull()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var locationEntity = new LocationEntity { Id = 1, Address = "Test", PostalCode = "12345", City = "Test" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "Test", LastName = "Test", PhoneNumber = "1234567", Email = "test@domain.com" };
        var participantsEntity = new ParticipantsEntity { Id = 1, Amount = "10-19" };
        var statusEntity = new StatusEntity { Id = 1, StatusText = "Booked" };
        var timeEntity = new TimeEntity { Id = 1, StartTime = "0800", EndTime = "1200" };
        var updatedBookingEntity = new BookingEntity { Id = 1, Client = clientEntity, Location = locationEntity, Date = "2024-01-01", Participants = participantsEntity, Status = statusEntity, Time = timeEntity };
        // Act
        var result = await bookingRepository.Update(1, updatedBookingEntity);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteBookingFromListByDate_ThenReturnTrue()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var locationEntity = new LocationEntity { Id = 1, Address = "Test", PostalCode = "12345", City = "Test" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "Test", LastName = "Test", PhoneNumber = "1234567", Email = "test@domain.com" };
        var participantsEntity = new ParticipantsEntity { Id = 1, Amount = "10-19" };
        var statusEntity = new StatusEntity { Id = 1, StatusText = "Booked" };
        var timeEntity = new TimeEntity { Id = 1, StartTime = "0800", EndTime = "1200" };
        var bookingEntity = new BookingEntity { Id = 1, Client = clientEntity, Location = locationEntity, Date = "2024-01-01", Participants = participantsEntity, Status = statusEntity, Time = timeEntity }; await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingRepository.Delete(x => x.Date == "2024-01-01");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Delete_ShouldTryToDeleteNoneExistingBookingFromListByDate_ThenReturnFalse()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);

        // Act
        var result = await bookingRepository.Delete(x => x.Date == "TestDate");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfBookingExistsByDate_ThenReturnTrue()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);
        var locationEntity = new LocationEntity { Id = 1, Address = "Test", PostalCode = "12345", City = "Test" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "Test", LastName = "Test", PhoneNumber = "1234567", Email = "test@domain.com" };
        var participantsEntity = new ParticipantsEntity { Id = 1, Amount = "10-19" };
        var statusEntity = new StatusEntity { Id = 1, StatusText = "Booked" };
        var timeEntity = new TimeEntity { Id = 1, StartTime = "0800", EndTime = "1200" };
        var bookingEntity = new BookingEntity { Id = 1, Client = clientEntity, Location = locationEntity, Date = "2024-01-01", Participants = participantsEntity, Status = statusEntity, Time = timeEntity }; await bookingRepository.Create(bookingEntity);
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingRepository.Exists(x => x.Date == "2024-01-01");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exist_ShouldCheckIfNoneExistingBookingExistsByDate_ThenReturnFalse()
    {
        // Arrange
        IBookingRepository bookingRepository = new BookingRepository(_context);

        // Act
        var result = await bookingRepository.Exists(x => x.Date == "TestDate");

        // Assert
        Assert.False(result);
    }
}
