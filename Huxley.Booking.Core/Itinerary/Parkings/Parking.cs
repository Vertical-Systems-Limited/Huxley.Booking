// using Huxley.Booking.Core.Itinerary.Parking.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Parking;
//
// public class Parking
// {
//     public string ParkingId { get; set; }
//     public string FileNumber { get; set; }
//     public string PayeeCompany { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string ArrivalDate { get; set; }
//     public string ArrivalTime { get; set; }
//     public string DepartDate { get; set; }
//     public string DepartTime { get; set; }
//     public string ParkingCompany { get; set; }
//     public string AddressLine1 { get; set; }
//     public string AddressLine2 { get; set; }
//     public string AddressLine3 { get; set; }
//     public string AddressLine4 { get; set; }
//     public string CarCollectionNumber { get; set; }
// }
//
// public static class ParkingExtensions
// {
//     public static Parking ToParking(this ParkingRecord parkingRecord)
//     {
//         if (parkingRecord == null)
//         {
//             throw new ArgumentNullException(nameof(parkingRecord));
//         }
//
//         return new Parking
//         {
//             ParkingId = parkingRecord.ParkingId,
//             FileNumber = parkingRecord.FileNumber,
//             PayeeCompany = parkingRecord.PayeeCompany,
//             PayeeCompanyReference = parkingRecord.PayeeCompanyReference,
//             ArrivalDate = parkingRecord.ArrivalDate,
//             ArrivalTime = parkingRecord.ArrivalTime,
//             DepartDate = parkingRecord.DepartDate,
//             DepartTime = parkingRecord.DepartTime,
//             ParkingCompany = parkingRecord.ParkingCompany,
//             AddressLine1 = parkingRecord.AddressLine1,
//             AddressLine2 = parkingRecord.AddressLine2,
//             AddressLine3 = parkingRecord.AddressLine3,
//             AddressLine4 = parkingRecord.AddressLine4,
//             CarCollectionNumber = parkingRecord.CarCollectionNumber
//         };
//     }
// }