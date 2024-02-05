using Infrastructure.Repositories;
using Infrastructure.Entities;
using Infrastructure.Dtos;
using System.Diagnostics;
using Infrastructure.Interfaces;
using Infrastructure.Enums;

namespace Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IClientRepository _clientRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IBookingRepository _bookingRepository;

    public BookingService(ClientRepository clientRepository, LocationRepository locationRepository, BookingRepository bookingRepository)
    {
        _clientRepository = clientRepository;
        _locationRepository = locationRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<ServiceStatus> CreateBooking(ICreateBookingDto booking)
    {
        try
        {
            if (!await _bookingRepository.Exists(x => x.Date == booking.Date))
            {
                var clientEntity = await _clientRepository.GetOne(x => x.Email == booking.Email);
                if (clientEntity == null)
                {
                    clientEntity = await _clientRepository.Create(new ClientEntity { FirstName = booking.FirstName, LastName = booking.LastName, Email = booking.Email, PhoneNumber = booking.PhoneNumber });
                }

                var locationEntity = await _locationRepository.GetOne(x => x.PostalCode == booking.PostalCode && x.Address == booking.Address);
                if (locationEntity == null)
                {
                    locationEntity = await _locationRepository.Create(new LocationEntity { Address = booking.Address, PostalCode = booking.PostalCode, City = booking.City });
                }

                var bookingEntity = new BookingEntity
                {
                    Date = booking.Date,
                    StatusId = booking.StatusId,
                    ClientId = clientEntity.Id,
                    ParticipantsId = booking.ParticipantId,
                    TimeId = booking.TimeId,
                    LocationId = locationEntity.Id
                };

                var result = await _bookingRepository.Create(bookingEntity);

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

    public async Task<IEnumerable<IBookingDto>> GetAllBookings()
    {
        var bookings = new List<IBookingDto>();

        try
        {
            var result = await _bookingRepository.GetAll();

            foreach (var item in result)
            {
                bookings.Add(new BookingDto
                {

                    FirstName = item.Client.FirstName,
                    LastName = item.Client.LastName,
                    Email = item.Client.Email,
                    PhoneNumber = item.Client.PhoneNumber,
                    Address = item.Location.Address,
                    PostalCode = item.Location.PostalCode,
                    City = item.Location.City,
                    Date = item.Date,
                    Status = item.Status.StatusText,
                    Participants = item.Participants.Amount,
                    Time = $"{item.Time.StartTime}-{item.Time.EndTime}"
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return bookings;
    }

    public async Task<ServiceStatus> UpdateClient(ClientEntity client, string newEmail, string oldEmail)
    {
        try
        {
            if (!await _clientRepository.Exists(x => x.Email == newEmail))
            {
                var clientEntity = await _clientRepository.GetOne(x => x.Email == oldEmail);

                clientEntity = await _clientRepository.Update(clientEntity.Id, new ClientEntity
                {
                    Id = clientEntity.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber
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

    public async Task<ServiceStatus> UpdateLocation(LocationEntity location, string newAddress, string newPostalCode, string oldAddress, string oldPostalCode)
    {
        try
        {
            if (!await _locationRepository.Exists(x => x.PostalCode == newPostalCode && x.Address == newAddress))
            {
                var locationEntity = await _locationRepository.GetOne(x => x.PostalCode == oldPostalCode && x.Address == oldAddress);

                locationEntity = await _locationRepository.Update(locationEntity.Id, new LocationEntity
                {
                    Id = locationEntity.Id,
                    Address = location.Address,
                    PostalCode = location.PostalCode,
                    City = location.City
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

    public async Task<ServiceStatus> UpdateBooking(string newDate, string oldDate, string email, string address, string postalCode, int statusId, int participantsId, int timeId)
    {
        try
        {
            if (!await _bookingRepository.Exists(x => x.Date == newDate))
            {
                var clientEntity = await _clientRepository.GetOne(x => x.Email == email);

                var locationEntity = await _locationRepository.GetOne(x => x.PostalCode == postalCode && x.Address == address);

                var bookingEntity = await _bookingRepository.GetOne(x => x.Date == oldDate);

                bookingEntity = await _bookingRepository.Update(bookingEntity.Id, new BookingEntity
                {
                    Id = bookingEntity.Id,
                    Date = newDate,
                    StatusId = statusId,
                    ClientId = clientEntity.Id,
                    ParticipantsId = participantsId,
                    TimeId = timeId,
                    LocationId = locationEntity.Id
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

    public async Task<ServiceStatus> DeleteBooking(string date)
    {
        try
        {
            if (await _bookingRepository.Exists(x => x.Date == date))
            {
                var result = await _bookingRepository.Delete(x => x.Date == date);

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
}
