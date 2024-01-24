namespace Infrastructure.Interfaces
{
    public interface IBookingDto
    {
        string Address { get; set; }
        string City { get; set; }
        string Date { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Participants { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
        string Status { get; set; }
        string Time { get; set; }
    }
}