// using Huxley.Booking.Core.Itinerary.Accommodations.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Accommodations;
//
// public class Accommodation
// {
//     public string FileNumber { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string PayeeCompany { get; set; }
//     public string HotelName { get; set; }
//     public string RoomType { get; set; }
//     public string BoardBasis { get; set; }
//     public string CheckInDate { get; set; }
//     public string CheckInTime { get; set; }
//     public string CheckOutDate { get; set; }
//     public string CheckOutTime { get; set; }
//     public string Nights { get; set; }
//     public string SpecialRequests { get; set; }
//     public string Remarks { get; set; }
//     public string AccommodationType { get; set; }
//     public string Flag { get; set; }
// }
//
// public static class AccommodationExtensions
// {
//     public static Accommodation ToAccommodation(this AccommodationRecord accommodationRecord)
//     {
//         if (accommodationRecord == null)
//         {
//             throw new ArgumentNullException(nameof(accommodationRecord));
//         }
//
//         return new Accommodation
//         {
//             FileNumber = accommodationRecord.FileNumber,
//             PayeeCompanyReference = accommodationRecord.PayeeCompanyReference,
//             PayeeCompany = accommodationRecord.PayeeCompany,
//             HotelName = accommodationRecord.HotelName,
//             RoomType = accommodationRecord.RoomType,
//             BoardBasis = accommodationRecord.BoardBasis,
//             CheckInDate = accommodationRecord.CheckInDate,
//             CheckInTime = accommodationRecord.CheckInTime,
//             CheckOutDate = accommodationRecord.CheckOutDate,
//             CheckOutTime = accommodationRecord.CheckOutTime,
//             Nights = accommodationRecord.Nights,
//             SpecialRequests = accommodationRecord.SpecialRequests,
//             Remarks = accommodationRecord.Remarks,
//             AccommodationType = accommodationRecord.AccommodationType,
//             Flag = accommodationRecord.Flag
//         };
//     }
// }