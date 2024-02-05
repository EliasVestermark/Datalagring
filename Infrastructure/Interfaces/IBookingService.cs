using Infrastructure.Entities;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceStatus> CreateBooking(ICreateBookingDto booking);
        Task<ServiceStatus> DeleteBooking(string date);
        Task<IEnumerable<IBookingDto>> GetAllBookings();
        Task<ServiceStatus> UpdateBooking(string newDate, string oldDate, string email, string address, string postalCode, int statusId, int participantsId, int timeId);
        Task<ServiceStatus> UpdateClient(ClientEntity client, string newEmail, string oldEmail);
        Task<ServiceStatus> UpdateLocation(LocationEntity location, string newAddress, string newPostalCode, string oldAddress, string oldPostalCode);
    }
}