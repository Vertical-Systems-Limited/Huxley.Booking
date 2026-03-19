using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Parkings;

namespace Huxley.Booking.Core.Itinerary.Parkings.Data;

public class ParkingRecord
{
    public string ParkingId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompany { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string ArrivalDate { get; set; }
    public string ArrivalTime { get; set; }
    public string DepartDate { get; set; }
    public string DepartTime { get; set; }
    public string ParkingCompany { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string CarCollectionNumber { get; set; }
}

public static class ParkingRecordExtensions
{
    public static IEnumerable<Parking> ToDomainModels(this IEnumerable<ParkingRecord> records)
    {
        return records.Select(r => r.ToParking());
    }

    public static Parking ToParking(this ParkingRecord record)
    {
        var domain = Parking.Create(
            record.ParkingCompany,
            record.ArrivalDate.ParseDateTime(record.ArrivalTime),
            record.DepartDate.ParseDateTime(record.DepartTime)
        );

        domain.AddressLine1 = record.AddressLine1;
        domain.AddressLine2 = record.AddressLine2;
        domain.AddressLine3 = record.AddressLine3;
        domain.AddressLine4 = record.AddressLine4;
        domain.CarCollectionNumber = record.CarCollectionNumber;
        domain.FileNumber = record.FileNumber;
        domain.PayeeCompany = record.PayeeCompany;
        domain.Reference = record.PayeeCompanyReference;

        return domain;
    }
}