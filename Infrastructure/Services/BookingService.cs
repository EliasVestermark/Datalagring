using Infrastructure.Repositories;
using Infrastructure.Entities;
using Infrastructure.Dtos;
using System.Diagnostics;

namespace Infrastructure.Services;

public class BookingService
{
    private readonly ClientRepository _clientRepository;
    private readonly LocationRepository _locationRepository;
    private readonly BookingRepository _bookingRepository;

    public BookingService(ClientRepository clientRepository, LocationRepository locationRepository, BookingRepository bookingRepository)
    {
        _clientRepository = clientRepository;
        _locationRepository = locationRepository;
        _bookingRepository = bookingRepository;
    }

    public bool CreateBooking(CreateBookingDto booking)
    {
        try
        {
            if (!_bookingRepository.Exists(x => x.Date == booking.Date))
            {
                var clientEntity = _clientRepository.GetOne(x => x.Email == booking.Email);
                if (clientEntity == null)
                {
                    clientEntity = _clientRepository.Create(new ClientEntity { FirstName = booking.FirstName, LastName = booking.LastName, Email = booking.Email, PhoneNumber = booking.PhoneNumber });
                }

                var locationEntity = _locationRepository.GetOne(x => x.PostalCode == booking.PostalCode && x.Address == booking.Address);
                if (locationEntity == null)
                {
                    locationEntity = _locationRepository.Create(new LocationEntity { Address = booking.Address, PostalCode = booking.PostalCode, City = booking.City });
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

                var result = _bookingRepository.Create(bookingEntity);

                if (result != null)
                {
                    return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return false;
    }

    public IEnumerable<BookingDto> GetAllBookings()
    {
        var bookings = new List<BookingDto>();

        try
        {
            var result = _bookingRepository.GetAll();

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
}
