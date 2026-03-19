using Huxley.Itinerary.Domain.Accommodations;

namespace Huxley.Booking.Core.Itinerary.Accommodations.Data;

public class AccommodationRecord
{
    public string FileNumber { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string PayeeCompany { get; set; }
    public string HotelName { get; set; }
    public string RoomType { get; set; }
    public string BoardBasis { get; set; }
    public string CheckInDate { get; set; }
    public string CheckInTime { get; set; }
    public string CheckOutDate { get; set; }
    public string CheckOutTime { get; set; }
    public string Nights { get; set; }
    public string SpecialRequests { get; set; }
    public string Remarks { get; set; }
    public string AccommodationType { get; set; }
    public string Flag { get; set; }
}

public static class AccommodationRecordExtensions
{
    public static IEnumerable<Accommodation> ToAccommodations(this IEnumerable<AccommodationRecord> records)
    {
        var accommodationGroups = records
            .GroupBy(r => new { r.FileNumber, r.HotelName, r.CheckInDate });

        foreach (var group in accommodationGroups)
        {
            var firstRecord = group.First();
            
            var accommodation = Accommodation.Create(
                name: firstRecord.HotelName ?? "",
                resort: "", // Record doesn't seem to have Resort, using empty string
                checkInDate: ParseDate(firstRecord.CheckInDate),
                nights: int.TryParse(firstRecord.Nights, out var n) ? n : 0
            );

            foreach (var record in group)
            {
                accommodation.AddRoom(
                    roomType: record.RoomType ?? "",
                    board: record.BoardBasis ?? "",
                    price: 0, 
                    specialRequests: record.SpecialRequests ?? ""
                );
            }

            yield return accommodation;
        }
    }

    private static DateOnly ParseDate(string dateString)
    {
        if (DateTime.TryParse(dateString, out var result))
        {
            return DateOnly.FromDateTime(result);
        }
        return DateOnly.FromDateTime(DateTime.MinValue);
    }
}