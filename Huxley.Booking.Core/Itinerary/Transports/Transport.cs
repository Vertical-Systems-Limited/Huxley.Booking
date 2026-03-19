// using Huxley.Booking.Core.Itinerary.Transport.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Transports;
//
// public class Transport
// {
//     public string TransportId { get; set; }
//     public string FileNumber { get; set; }
//     public string PayeeCompany { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string DepartPoint { get; set; }
//     public string TransportType { get; set; }
//     public string ArrivalPoint { get; set; }
//     public string VehicleFuelType { get; set; }
//     public string TransportCompany { get; set; }
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
// public static class TransportExtensions
// {
//     public static Transport ToTransport(this TransportRecord transportRecord)
//     {
//         if (transportRecord == null)
//         {
//             throw new ArgumentNullException(nameof(transportRecord));
//         }
//
//         return new Transport
//         {
//             TransportId = transportRecord.TransportId,
//             FileNumber = transportRecord.FileNumber,
//             PayeeCompany = transportRecord.PayeeCompany,
//             PayeeCompanyReference = transportRecord.PayeeCompanyReference,
//             DepartPoint = transportRecord.DepartPoint,
//             TransportType = transportRecord.TransportType,
//             ArrivalPoint = transportRecord.ArrivalPoint,
//             VehicleFuelType = transportRecord.VehicleFuelType,
//             TransportCompany = transportRecord.TransportCompany,
//             VehicleDescription = transportRecord.VehicleDescription,
//             VehicleRegistration = transportRecord.VehicleRegistration,
//             VehicleLength = transportRecord.VehicleLength,
//             DepartDate = transportRecord.DepartDate,
//             DepartTime = transportRecord.DepartTime,
//             CheckInTime = transportRecord.CheckInTime,
//             ArrivalDate = transportRecord.ArrivalDate,
//             ArrivalTime = transportRecord.ArrivalTime,
//             Duration = transportRecord.Duration,
//             ReservationAllocation = transportRecord.ReservationAllocation,
//             Remarks = transportRecord.Remarks
//         };
//     }
// }