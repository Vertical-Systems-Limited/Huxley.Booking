// using Huxley.Booking.Core.Itineraries.CruiseItineraries.Data;
//
// namespace Huxley.Booking.Core.Itineraries.CruiseItineraries;
//
// public class CruiseItinerary
// {
//     public string CruiseId{ get; set; }
//     public string FileNumber{ get; set; }
//     public string ShipStatus{ get; set; }
//     public string ArrivalPort{ get; set; }
//     public string ArrivalDate{ get; set; }
//     public string ArrivalTime{ get; set; }
//     public string ArrivalDescription{ get; set; }
//     public string DepartDate{ get; set; }
//     public string DepartTime{ get; set; }
//     public string DepartDescription{ get; set; }
//     public string CruisingDate{ get; set; }
//     public string CruisingTime{ get; set; }
// }
//
// public static class CruiseItineraryExtensions
// {
//     public static CruiseItinerary ToCruiseItinerary(this CruiseItineraryRecord cruiseItineraryRecord)
//     {
//         if (cruiseItineraryRecord == null)
//         {
//             throw new ArgumentNullException(nameof(cruiseItineraryRecord));
//         }
//         
//         return new CruiseItinerary
//         {
//             CruiseId = cruiseItineraryRecord.CruiseId,
//             FileNumber = cruiseItineraryRecord.FileNumber,
//             ShipStatus = cruiseItineraryRecord.ShipStatus,
//             ArrivalPort = cruiseItineraryRecord.ArrivalPort,
//             ArrivalDate = cruiseItineraryRecord.ArrivalDate,
//             ArrivalTime = cruiseItineraryRecord.ArrivalTime,
//             ArrivalDescription = cruiseItineraryRecord.ArrivalDescription,
//             DepartDate = cruiseItineraryRecord.DepartDate,
//             DepartTime = cruiseItineraryRecord.DepartTime,
//             DepartDescription = cruiseItineraryRecord.DepartDescription,
//             CruisingDate = cruiseItineraryRecord.CruisingDate,
//             CruisingTime = cruiseItineraryRecord.CruisingTime
//         }; 
//     }
// }