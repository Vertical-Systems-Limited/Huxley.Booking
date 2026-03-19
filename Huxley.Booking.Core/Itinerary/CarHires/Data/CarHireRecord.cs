using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.CarHires;

namespace Huxley.Booking.Core.Itinerary.CarHires.Data;

public class CarHireRecord
{
    public string CarHireId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string PayeeCompany { get; set; }
    public string DepartPoint { get; set; }
    public string PickupDate { get; set; }
    public string PickupTime { get; set; }
    public string ArrivalPoint { get; set; }
    public string DropOffDate { get; set; }
    public string DropOffTime { get; set; }
    public string CarHireCompany { get; set; }
    public string CarType { get; set; }
    public string Group { get; set; }
    public string Plan { get; set; }
    public string Driver { get; set; }
    public string CDW { get; set; }
    public string TheftProtection { get; set; }
    public string TaxOtherCharges { get; set; }
    public string SpecialRequests { get; set; }
    public string Remarks { get; set; }
}

public static class CarHireRecordExtensions
{
    public static IEnumerable<CarHire> ToDomainModels(this IEnumerable<CarHireRecord> records)
    {
        return records.Select(r => r.ToDomain());
    }

    public static CarHire ToDomain(this CarHireRecord record)
    {
        var domain = CarHire.Create(
            record.CarHireCompany,
            record.CarType,
            record.DepartPoint,
            record.PickupDate.ParseDateTime(record.PickupTime),
            record.ArrivalPoint,
            record.DropOffDate.ParseDateTime(record.DropOffTime)
        );

        domain.Group = record.Group;
        domain.Plan = record.Plan;
        domain.Driver = record.Driver;
        domain.FileNumber = record.FileNumber;
        domain.SpecialRequests = record.SpecialRequests;
        domain.Remarks = record.Remarks;
        domain.PayeeCompany = record.PayeeCompany;
        domain.Reference = record.PayeeCompanyReference;
        domain.Cdw = record.CDW.StringToBool();
        domain.TheftProtection = record.TheftProtection.StringToBool();
        domain.TaxOtherCharges = record.TaxOtherCharges.StringToBool();
        return domain;
    }
}