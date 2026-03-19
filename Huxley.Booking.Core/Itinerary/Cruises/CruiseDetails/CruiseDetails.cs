// using Huxley.Booking.Core.Itineraries.CruiseDetails.Data;
//
// namespace Huxley.Booking.Core.Itineraries.CruiseDetails;
//
// public class CruiseDetails
// {
//     public string FileNumber { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string PayeeCompany { get; set; }
//     public string ShipName { get; set; }
//     public string ItineraryTitle { get; set; }
//     public string Duration { get; set; }
//     public string CabinCategory { get; set; }
//     public string CabinNumber { get; set; }
//     public string DeckName { get; set; }
//     public string DiningTime { get; set; }
//     public string TableSize { get; set; }
//     public string Services { get; set; }
//     public string FareCode { get; set; }
//     public string OnBoardCredit { get; set; }
//     public string CreditAmount { get; set; }
//     public string DepartDate { get; set; }
//     public string CheckInTime { get; set; }
//     public string SpecialRequests { get; set; }
//     public string Remarks { get; set; }
// }
//
// public static class CruiseDetailsExtensions
// {
//     public static CruiseDetails ToCruiseDetails(this CruiseDetailsRecord cruiseDetailsRecord)
//     {
//         if (cruiseDetailsRecord == null)
//         {
//             throw new ArgumentNullException(nameof(cruiseDetailsRecord));
//         }
//
//         return new CruiseDetails
//         {
//             FileNumber = cruiseDetailsRecord.FileNumber,
//             PayeeCompanyReference = cruiseDetailsRecord.PayeeCompanyReference,
//             PayeeCompany = cruiseDetailsRecord.PayeeCompany,
//             ShipName = cruiseDetailsRecord.ShipName,
//             ItineraryTitle = cruiseDetailsRecord.ItineraryTitle,
//             Duration = cruiseDetailsRecord.Duration,
//             CabinCategory = cruiseDetailsRecord.CabinCategory,
//             CabinNumber = cruiseDetailsRecord.CabinNumber,
//             DeckName = cruiseDetailsRecord.DeckName,
//             DiningTime = cruiseDetailsRecord.DiningTime,
//             TableSize = cruiseDetailsRecord.TableSize,
//             Services = cruiseDetailsRecord.Services,
//             FareCode = cruiseDetailsRecord.FareCode,
//             OnBoardCredit = cruiseDetailsRecord.OnBoardCredit,
//             CreditAmount = cruiseDetailsRecord.CreditAmount,
//             DepartDate = cruiseDetailsRecord.DepartDate,
//             CheckInTime = cruiseDetailsRecord.CheckInTime,
//             SpecialRequests = cruiseDetailsRecord.SpecialRequests,
//             Remarks = cruiseDetailsRecord.Remarks
//         }; 
//     }
// }