using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface IBookingService
    {
        ServiceStatus CreateBooking(ICreateBookingDto booking);
        ServiceStatus DeleteBooking(string date);
        IEnumerable<IBookingDto> GetAllBookings();
        ServiceStatus UpdateBooking(string newDate, string oldDate, string email, string address, string postalCode, int statusId, int participantsId, int timeId);
        ServiceStatus UpdateClient(ClientEntity client, string newEmail, string oldEmail);
        ServiceStatus UpdateLocation(LocationEntity location, string newAddress, string newPostalCode, string oldAddress, string oldPostalCode);
    }
}