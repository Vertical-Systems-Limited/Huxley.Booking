namespace Huxley.Booking.Core.Passengers;

public class Passenger
{
    public int PassengerId { get; set; }
    public string FileNumber { get; set; }
    public string Title { get; set; }
    public string Firstname { get; set; }
    public string Middlename { get; set; }
    public string Surname { get; set; }
    public DateOnly DOB { get; set; }
    public int BookingAge { get; set; }
    public int DepartureAge { get; set; }
    public string Insurance { get; set; }
    public string EmailAddress { get; set; }
    public string PassportName { get; set; }
    public string PassportNumber { get; set; }
    public string PassportType { get; set; }
    public string Nationality { get; set; }
    public string PassportCountryOfIssue { get; set; }
    public string PassportDateOfIssue { get; set; }
    public DateOnly PassportDateOfExpiry { get; set; }
    public string CountryOfResidence { get; set; }
    public string Gender { get; set; }
    public string InsuranceForm { get; set; }
    public string InsurancePolicy { get; set; }
}

