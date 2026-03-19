using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Ferries;

namespace Huxley.Booking.Core.Itinerary.Ferries.Data;

public class FerryRecord
{
    public string FerryId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompany { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string DepartPort { get; set; }
    public string DepartTerminal { get; set; }
    public string ArrivalPort { get; set; }
    public string ArrivalTerminal { get; set; }
    public string FerryCompany { get; set; }
    public string VehicleDescription { get; set; }
    public string VehicleRegistration { get; set; }
    public string VehicleLength { get; set; }
    public string DepartDate { get; set; }
    public string DepartTime { get; set; }
    public string CheckInTime { get; set; }
    public string ArrivalDate { get; set; }
    public string ArrivalTime { get; set; }
    public string Duration { get; set; }
    public string ReservationAllocation { get; set; }
    public string Remarks { get; set; }
}

public static class FerryRecordExtensions
{
    public static IEnumerable<Ferry> ToDomainModels(this IEnumerable<FerryRecord> records)
    {
        return records.Select(r => r.ToFerry());
    }

    public static Ferry ToFerry(this FerryRecord record)
    {
        var domain = Ferry.Create(
            record.FerryCompany,
            record.DepartPort,
            record.DepartDate.ParseDateTime(record.DepartTime),
            record.ArrivalPort,
            record.ArrivalDate.ParseDateTime(record.ArrivalTime)
        );

        domain.FileNumber = record.FileNumber;
        domain.DepartTerminal = record.DepartTerminal;
        domain.ArrivalTerminal = record.ArrivalTerminal;
        domain.CheckInTime = record.CheckInTime.ParseTimeOnly();
        domain.VehicleDescription = record.VehicleDescription;
        domain.VehicleRegistration = record.VehicleRegistration;
        domain.Remarks = record.Remarks;
        domain.PayeeCompany = record.PayeeCompany;
        domain.Reference = record.PayeeCompanyReference;
        domain.VehicleLength = record.VehicleLength;
        domain.Duration = record.Duration.ParseTimeOnly();
        domain.Reservation = record.ReservationAllocation;

        return domain;
    }
}