using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Transfers;

namespace Huxley.Booking.Core.Itinerary.Transfers.Data;

public class TransferRecord
{
    public string TransferId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompany { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string DepartPoint { get; set; }
    public string ArrivalPoint { get; set; }
    public string ArrivalAddressLine1 { get; set; }
    public string ArrivalAddressLine2 { get; set; }
    public string ArrivalAddressLine3 { get; set; }
    public string ArrivalAddressLine4 { get; set; }
    public string PickupDate { get; set; }
    public string PickupTime { get; set; }
    public string TransportType { get; set; }
    public string LocalAgent { get; set; }
    public string SpecialRequests { get; set; }
    public string Remarks { get; set; }
}

public static class TransferRecordExtensions
{
    public static Transfer ToTransfer(this TransferRecord record)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));

        return Transfer.Create(
            pickUpLocation: record.DepartPoint ?? "",
            dropOffLocation: record.ArrivalPoint ?? "",
            pickUpDateTime: record.PickupDate.ParseDateOnly(),
            vehicleType: record.TransportType ?? "",
            timeFrom: record.PickupTime.ParseTimeOnly()
        );
    }

    public static IEnumerable<Transfer> ToTransfers(this IEnumerable<TransferRecord> records)
    {
        return records.Select(r => r.ToTransfer());
    }
}