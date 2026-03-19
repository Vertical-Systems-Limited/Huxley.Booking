using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Rails;

namespace Huxley.Booking.Core.Itinerary.Rails.Data;

public class RailRecord
{
    public string RailId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string PayeeCompany { get; set; }
    public string DepartPoint { get; set; }
    public string ArrivalPoint { get; set; }
    public string Class { get; set; }
    public string TrainNumber { get; set; }
    public string Company { get; set; }
    public string DepartDate { get; set; }
    public string DepartTime { get; set; }
    public string CheckInTime { get; set; }
    public string ArrivalDate { get; set; }
    public string ArrivalTime { get; set; }
    public string Duration { get; set; }
    public string SeatAllocation { get; set; }
    public string SpecialRequests { get; set; }
    public string Remarks { get; set; }
    public string BaggageAllowance { get; set; }
}

public static class RailRecordExtensions
{
    public static IEnumerable<Rail> ToDomainModels(this IEnumerable<RailRecord> records)
    {
        return records.Select(r => r.ToRail());
    }

    public static Rail ToRail(this RailRecord record)
    {
        var domain = Rail.Create(
            record.Company,
            record.DepartPoint,
            record.DepartDate.ParseDateTime(record.DepartTime),
            record.ArrivalPoint,
            record.ArrivalDate.ParseDateTime(record.ArrivalTime)
        );

        domain.FileNumber = record.FileNumber;
        domain.TrainNumber = record.TrainNumber;
        domain.TravelClass = record.Class;
        domain.CheckInTime = record.CheckInTime.ParseTimeOnly();
        domain.SeatAllocation = record.SeatAllocation;
        domain.BaggageAllowance = record.BaggageAllowance;
        domain.SpecialRequests = record.SpecialRequests;
        domain.Remarks = record.Remarks;
        domain.PayeeCompany = record.PayeeCompany;
        domain.Reference = record.PayeeCompanyReference;
        domain.Duration = record.Duration.ParseTimeOnly();

        return domain;
    }
}