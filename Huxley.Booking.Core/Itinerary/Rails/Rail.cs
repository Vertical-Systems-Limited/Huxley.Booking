// using Huxley.Booking.Core.Itinerary.Rails.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Rails;
//
// public class Rail
// {
//     public string RailId { get; set; }
//     public string FileNumber { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string PayeeCompany { get; set; }
//     public string DepartPoint { get; set; }
//     public string ArrivalPoint { get; set; }
//     public string Class { get; set; }
//     public string TrainNumber { get; set; }
//     public string Company { get; set; }
//     public string DepartDate { get; set; }
//     public string DepartTime { get; set; }
//     public string CheckInTime { get; set; }
//     public string ArrivalDate { get; set; }
//     public string ArrivalTime { get; set; }
//     public string Duration { get; set; }
//     public string SeatAllocation { get; set; }
//     public string SpecialRequests { get; set; }
//     public string Remarks { get; set; }
//     public string BaggageAllowance { get; set; }
// }
//
// public static class RailExtensions
// {
//     public static Rail ToRail(this RailRecord railRecord)
//     {
//         if (railRecord == null)
//         {
//             throw new ArgumentNullException(nameof(railRecord));
//         }
//
//         return new Rail
//         {
//             RailId = railRecord.RailId,
//             FileNumber = railRecord.FileNumber,
//             PayeeCompanyReference = railRecord.PayeeCompanyReference,
//             PayeeCompany = railRecord.PayeeCompany,
//             DepartPoint = railRecord.DepartPoint,
//             ArrivalPoint = railRecord.ArrivalPoint,
//             Class = railRecord.Class,
//             TrainNumber = railRecord.TrainNumber,
//             Company = railRecord.Company,
//             DepartDate = railRecord.DepartDate,
//             DepartTime = railRecord.DepartTime,
//             CheckInTime = railRecord.CheckInTime,
//             ArrivalDate = railRecord.ArrivalDate,
//             ArrivalTime = railRecord.ArrivalTime,
//             Duration = railRecord.Duration,
//             SeatAllocation = railRecord.SeatAllocation,
//             SpecialRequests = railRecord.SpecialRequests,
//             Remarks = railRecord.Remarks,
//             BaggageAllowance = railRecord.BaggageAllowance
//         };
//     }
// }