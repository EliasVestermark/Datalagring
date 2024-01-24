using Infrastructure.Interfaces;

namespace Infrastructure.Dtos;

public class CreateBookingDto(string firstName, string lastName, string phoneNumber, string email, string address, string postalCode, string city, string date, int statusId, int participantId, int timeId) : ICreateBookingDto
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    public string Address { get; set; } = address;
    public string PostalCode { get; set; } = postalCode;
    public string City { get; set; } = city;
    public string Date { get; set; } = date;
    public int StatusId { get; set; } = statusId;
    public int ParticipantId { get; set; } = participantId;
    public int TimeId { get; set; } = timeId;
}
