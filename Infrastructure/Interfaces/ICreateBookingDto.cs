namespace Infrastructure.Interfaces
{
    public interface ICreateBookingDto
    {
        string Address { get; set; }
        string City { get; set; }
        string Date { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int ParticipantId { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
        int StatusId { get; set; }
        int TimeId { get; set; }
    }
}