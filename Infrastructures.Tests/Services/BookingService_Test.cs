using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Services;

public class BookingService_Test
{
    private readonly BookingCatalogContext _context = new(new DbContextOptionsBuilder<BookingCatalogContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task CreateBooking_ShouldCreateBookingIfNoBookingWithSameDateExist_ThenReturnServiceStatusSuccess()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var booking = new CreateBookingDto("TestName", "TestName", "TestPhoneNumber", "TestEmail", "TestAddress", "TestPostalCode", "TestCity", "TestDate", 1, 1, 1);

        //Act
        var result = await bookingService.CreateBooking(booking);

        //Assert
        Assert.Equal(ServiceStatus.SUCCESS, result);
    }

    [Fact]
    public async Task CreateBooking_ShouldNotCreateBookingSinceBookingWithSameDateExist_ThenReturnServiceStatusAlreadyExists()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var booking = new CreateBookingDto("TestName", "TestName", "TestPhoneNumber", "TestEmail", "TestAddress", "TestPostalCode", "TestCity", "TestDate", 1, 1, 1);

        //Act
        await bookingService.CreateBooking(booking);
        var result = await bookingService.CreateBooking(booking);

        //Assert
        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result);
    }

    [Fact]
    public async Task CreateBooking_ShouldNotCreateBookingDueToNullVariable_ThenReturnServiceStatusFailed()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var booking = new CreateBookingDto(null!, "TestName", "TestPhoneNumber", "TestEmail", "TestAddress", "TestPostalCode", "TestCity", "TestDate", 1, 1, 1);

        //Act
        var result = await bookingService.CreateBooking(booking);

        //Assert
        Assert.Equal(ServiceStatus.FAILED, result);
    }

    [Fact]
    public async Task GetAllBookings_ShouldGetAllBookingsFromTheDataBase_ThenReturnAsIEnumerable()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var booking = new CreateBookingDto("TestName", "TestName", "TestPhoneNumber", "TestEmail", "TestAddress", "TestPostalCode", "TestCity", "TestDate", 1, 1, 1);
        await bookingService.CreateBooking(booking);

        //Act
        var result = await bookingService.GetAllBookings();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<IBookingDto>>(result);
    }

    [Fact]
    public async Task GetAllBookings_ShouldGetEmptyListFromTheDataBase_ThenReturnEmptyIEnumerable()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        //Act
        var result = await bookingService.GetAllBookings();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateClient_ShouldUpdateClientIfNoClientWithSameEmailExist_ThenReturnServiceStatusUpdated()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var oldClientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        var newClientEntity = new ClientEntity { FirstName = "TestFirstName2", LastName = "TestLastName2", PhoneNumber = "9999999", Email = "test2@domain.com" };
        
        await clientRepository.Create(oldClientEntity);

        // Act
        var result = await bookingService.UpdateClient(newClientEntity, newClientEntity.Email, oldClientEntity.Email);

        //Assert
        Assert.Equal(ServiceStatus.UPDATED, result);
    }

    [Fact]
    public async Task UpdateClient_ShouldNotUpdateClientSinceClientWithSameEmailExist_ThenReturnServiceStatusAlreadyExist()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var oldClientEntity = new ClientEntity { FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        var newClientEntity = new ClientEntity { FirstName = "TestFirstName2", LastName = "TestLastName2", PhoneNumber = "9999999", Email = "test@domain.com" };

        await clientRepository.Create(oldClientEntity);

        // Act
        var result = await bookingService.UpdateClient(newClientEntity, newClientEntity.Email, oldClientEntity.Email);

        //Assert
        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result);
    }

    [Fact]
    public async Task UpdateClient_ShouldFailToUpdateClientDueToNoExistingClient_ThenReturnServiceStatusFailed()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var newClientEntity = new ClientEntity { FirstName = "TestFirstName2", LastName = "TestLastName2", PhoneNumber = "9999999", Email = "test2@domain.com" };

        // Act
        var result = await bookingService.UpdateClient(newClientEntity, newClientEntity.Email, "oldEmail");

        //Assert
        Assert.Equal(ServiceStatus.FAILED, result);
    }

    [Fact]
    public async Task UpdateLocation_ShouldUpdateLocationIfNoLocationWithSameAddressAndPostalCodeExist_ThenReturnServiceStatusUpdated()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var oldLocationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12345", City = "TestCity" };
        var newLocationEntity = new LocationEntity { Address = "TestAddress2", PostalCode = "99999", City = "TestCity2" };

        await locationRepository.Create(oldLocationEntity);

        // Act
        var result = await bookingService.UpdateLocation(newLocationEntity, newLocationEntity.Address, newLocationEntity.PostalCode, oldLocationEntity.Address, oldLocationEntity.PostalCode);

        //Assert
        Assert.Equal(ServiceStatus.UPDATED, result);
    }

    [Fact]
    public async Task UpdateLocation_ShouldNotUpdateLocationSinceLocationWithSameAddressAndPostalCodeExist_ThenReturnServiceStatusAlreadyExist()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var oldLocationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12345", City = "TestCity" };
        var newLocationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12345", City = "TestCity2" };

        await locationRepository.Create(oldLocationEntity);

        // Act
        var result = await bookingService.UpdateLocation(newLocationEntity, newLocationEntity.Address, newLocationEntity.PostalCode, oldLocationEntity.Address, oldLocationEntity.PostalCode);

        //Assert
        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result);
    }

    [Fact]
    public async Task UpdateLocation_ShouldFailToUpdateLocationDueToNoExistingLocation_ThenReturnServiceStatusFailed()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var newLocationEntity = new LocationEntity { Address = "TestAddress", PostalCode = "12345", City = "TestCity2" };

        // Act
        var result = await bookingService.UpdateLocation(newLocationEntity, newLocationEntity.Address, newLocationEntity.PostalCode, "testAddress", "testPostalCode");

        //Assert
        Assert.Equal(ServiceStatus.FAILED, result);
    }

    [Fact]
    public async Task UpdateBooking_ShouldUpdateBookingIfNoBookingWithSameDateExist_ThenReturnServiceStatusUpdated()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var locationEntity = new LocationEntity {Id = 1, Address = "TestAddress", PostalCode = "12345", City = "TestCity" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        var bookingEntity = new BookingEntity { Id = 1, ClientId = 1, LocationId = 1, Date = "2024-01-01", ParticipantsId = 1, StatusId = 1, TimeId = 1 };

        await clientRepository.Create(clientEntity);
        await locationRepository.Create(locationEntity);
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingService.UpdateBooking("2025-05-05",bookingEntity.Date, clientEntity.Email, locationEntity.Address, locationEntity.PostalCode, 2, 2, 2);

        //Assert
        Assert.Equal(ServiceStatus.UPDATED, result);
    }

    [Fact]
    public async Task UpdateBooking_ShouldNotUpdateBookingSinceBookingWithSameDateExist_ThenReturnServiceStatusAlreadyExist()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var locationEntity = new LocationEntity { Id = 1, Address = "TestAddress", PostalCode = "12345", City = "TestCity" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        var bookingEntity = new BookingEntity { Id = 1, ClientId = 1, LocationId = 1, Date = "2024-01-01", ParticipantsId = 1, StatusId = 1, TimeId = 1 };

        await clientRepository.Create(clientEntity);
        await locationRepository.Create(locationEntity);
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingService.UpdateBooking("2024-01-01", bookingEntity.Date, clientEntity.Email, locationEntity.Address, locationEntity.PostalCode, 2, 2, 2);

        //Assert
        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result);
    }

    [Fact]
    public async Task UpdateBooking_ShouldFailToUpdateBookingDueToNoExistingBooking_ThenReturnServiceStatusFailed()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        // Act
        var result = await bookingService.UpdateBooking("2025-05-05", "2024-01-01", "testEmail", "testAddress", "testPostalCode", 2, 2, 2);

        //Assert
        Assert.Equal(ServiceStatus.FAILED, result);
    }

    [Fact]
    public async Task DeleteBooking_ShouldDeleteBookingIfBookingWithSameDateExist_ThenReturnServiceStatusDeleted()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        var locationEntity = new LocationEntity { Id = 1, Address = "TestAddress", PostalCode = "12345", City = "TestCity" };
        var clientEntity = new ClientEntity { Id = 1, FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567", Email = "test@domain.com" };
        var bookingEntity = new BookingEntity { Id = 1, ClientId = 1, LocationId = 1, Date = "2024-01-01", ParticipantsId = 1, StatusId = 1, TimeId = 1 };

        await clientRepository.Create(clientEntity);
        await locationRepository.Create(locationEntity);
        await bookingRepository.Create(bookingEntity);

        // Act
        var result = await bookingService.DeleteBooking("2024-01-01");

        //Assert
        Assert.Equal(ServiceStatus.DELETED, result);
    }

    [Fact]
    public async Task DeleteBooking_ShouldNotDeleteBookingSinceNoBookingWithSameDateExist_ThenReturnServiceStatusNotFound()
    {
        //Arrange
        var clientRepository = new ClientRepository(_context);
        var locationRepository = new LocationRepository(_context);
        var bookingRepository = new BookingRepository(_context);
        var bookingService = new BookingService(clientRepository, locationRepository, bookingRepository);

        // Act
        var result = await bookingService.DeleteBooking("2025-01-01");

        //Assert
        Assert.Equal(ServiceStatus.NOT_FOUND, result);
    }
}
