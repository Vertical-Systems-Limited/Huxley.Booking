using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Transports;

namespace Huxley.Booking.Core.Itinerary.Transports.Data;

public class TransportRecord
{
    public string TransportId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompany { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string DepartPoint { get; set; }
    public string TransportType { get; set; }
    public string ArrivalPoint { get; set; }
    public string VehicleFuelType { get; set; }
    public string TransportCompany { get; set; }
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

public static class TransportRecordExtensions
{
    public static IEnumerable<Transport> ToDomainModels(this IEnumerable<TransportRecord> records)
    {
        return records.Select(r => r.ToTransport());
    }

    public static Transport ToTransport(this TransportRecord record)
    {
        var domain = Transport.Create(
            record.TransportCompany,
            record.DepartPoint,
            record.DepartDate.ParseDateTime(record.DepartTime),
            record.ArrivalPoint,
            record.ArrivalDate.ParseDateTime(record.ArrivalTime)
        );

        domain.TransportType = record.TransportType;
        domain.VehicleDescription = record.VehicleDescription;
        domain.VehicleRegistration = record.VehicleRegistration;
        domain.VehicleFuelType = record.VehicleFuelType;
        domain.CheckInTime = record.CheckInTime.ParseTimeOnly();
        domain.Remarks = record.Remarks;
        domain.FileNumber = record.FileNumber;
        domain.PayeeCompany = record.PayeeCompany;
        domain.Reference = record.PayeeCompanyReference;
        domain.Duration = record.Duration.ParseTimeOnly();
        domain.Reservation = record.ReservationAllocation;
        domain.VehicleLength = record.VehicleLength;

        return domain;
    }
}
