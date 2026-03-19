//
// using Huxley.Booking.Core.Itinerary.Ferries.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Ferries;
//
// public class Ferry
// {
//     public string FerryId { get; set; }
//     public string FileNumber { get; set; }
//     public string PayeeCompany { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string DepartPort { get; set; }
//     public string DepartTerminal { get; set; }
//     public string ArrivalPort { get; set; }
//     public string ArrivalTerminal { get; set; }
//     public string FerryCompany { get; set; }
//     public string VehicleDescription { get; set; }
//     public string VehicleRegistration { get; set; }
//     public string VehicleLength { get; set; }
//     public string DepartDate { get; set; }
//     public string DepartTime { get; set; }
//     public string CheckInTime { get; set; }
//     public string ArrivalDate { get; set; }
//     public string ArrivalTime { get; set; }
//     public string Duration { get; set; }
//     public string ReservationAllocation { get; set; }
//     public string Remarks { get; set; }
// }
//
// public static class FerryExtensions
// {
//     public static Ferry ToFerry(this FerryRecord ferryRecord)
//     {
//         if (ferryRecord == null)
//         {
//             throw new ArgumentNullException(nameof(ferryRecord));
//         }
//
//         return new Ferry
//         {
//             FerryId = ferryRecord.FerryId,
//             FileNumber = ferryRecord.FileNumber,
//             PayeeCompany = ferryRecord.PayeeCompany,
//             PayeeCompanyReference = ferryRecord.PayeeCompanyReference,
//             DepartPort = ferryRecord.DepartPort,
//             DepartTerminal = ferryRecord.DepartTerminal,
//             ArrivalPort = ferryRecord.ArrivalPort,
//             ArrivalTerminal = ferryRecord.ArrivalTerminal,
//             FerryCompany = ferryRecord.FerryCompany,
//             VehicleDescription = ferryRecord.VehicleDescription,
//             VehicleRegistration = ferryRecord.VehicleRegistration,
//             VehicleLength = ferryRecord.VehicleLength,
//             DepartDate = ferryRecord.DepartDate,
//             DepartTime = ferryRecord.DepartTime,
//             CheckInTime = ferryRecord.CheckInTime,
//             ArrivalDate = ferryRecord.ArrivalDate,
//             ArrivalTime = ferryRecord.ArrivalTime,
//             Duration = ferryRecord.Duration,
//             ReservationAllocation = ferryRecord.ReservationAllocation,
//             Remarks = ferryRecord.Remarks
//         };
//     }
// }