using Huxley.Booking.Core.Passengers.Data;

namespace Huxley.Booking.Core.Passengers.Contracts;

public class CreatePassengerDto
{
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Dob { get; set; }
}