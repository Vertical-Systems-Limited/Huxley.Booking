namespace Huxley.Booking.Core.Passengers.Data;

public class PassengerRecord
{
    public string PassengerId { get; set; }
    public string FileNumber { get; set; }
    public string Title { get; set; }
    public string Firstname { get; set; }
    public string Middlename { get; set; }
    public string Surname { get; set; }
    public string DOB { get; set; }
    public string BookingAge { get; set; }
    public string DepartureAge { get; set; }
    public string Insurance { get; set; }
    public string EmailAddress { get; set; }
    public string PassportName { get; set; }
    public string PassportNumber { get; set; }
    public string PassportType { get; set; }
    public string Nationality { get; set; }
    public string PassportCountryOfIssue { get; set; }
    public string PassportDateOfIssue { get; set; }
    public string PassportDateOfExpiry { get; set; }
    public string CountryOfResidence { get; set; }
    public string Gender { get; set; }
    public string InsuranceForm { get; set; }
    public string InsurancePolicy { get; set; }
}

public static class PassengerRecordExtensions
{
    public static Passenger ToPassenger(this PassengerRecord passengerRecord)
    {
        if (passengerRecord == null)
        {
            throw new ArgumentNullException(nameof(passengerRecord));
        }

        return new Passenger
        {
            PassengerId = int.TryParse(passengerRecord.PassengerId, out var passengerId) ? passengerId : 0,
            FileNumber = passengerRecord.FileNumber,
            Title = passengerRecord.Title,
            Firstname = passengerRecord.Firstname,
            Middlename = passengerRecord.Middlename,
            Surname = passengerRecord.Surname,
            DOB = DateOnly.TryParse(passengerRecord.DOB, out var dob) ? dob : default,
            BookingAge = int.TryParse(passengerRecord.BookingAge, out var bookingAge) ? bookingAge : 0,
            DepartureAge = int.TryParse(passengerRecord.DepartureAge, out var departureAge) ? departureAge : 0,
            Insurance = passengerRecord.Insurance,
            EmailAddress = passengerRecord.EmailAddress,
            PassportName = passengerRecord.PassportName,
            PassportNumber = passengerRecord.PassportNumber,
            PassportType = passengerRecord.PassportType,
            Nationality = passengerRecord.Nationality,
            PassportCountryOfIssue = passengerRecord.PassportCountryOfIssue,
            PassportDateOfIssue = passengerRecord.PassportDateOfIssue,
            PassportDateOfExpiry = DateOnly.TryParse(passengerRecord.PassportDateOfExpiry, out var passportDateOfExpiry) ? passportDateOfExpiry : default,
            CountryOfResidence = passengerRecord.CountryOfResidence,
            Gender = passengerRecord.Gender,
            InsuranceForm = passengerRecord.InsuranceForm,
            InsurancePolicy = passengerRecord.InsurancePolicy
        };
    }
}