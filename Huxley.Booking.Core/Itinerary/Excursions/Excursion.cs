//
// using Huxley.Booking.Core.Itinerary.Excursions.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Excursions;
//
// public class Excursion
// {
//     public string ExcursionId { get; set; }
//     public string FileNumber { get; set; }
//     public string Description { get; set; }
//     public string PayeeCompany { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string ExcursionName { get; set; }
//     public string LocalAgent { get; set; }
//     public string NoteText1 { get; set; }
//     public string NoteText2 { get; set; }
//     public string NoteText3 { get; set; }
//     public string DeparturePoint { get; set; }
//     public string ArrivalPoint { get; set; }
//     public string StartDate { get; set; }
//     public string StartTime { get; set; }
//     public string EndDate { get; set; }
//     public string EndTime { get; set; }
// }
//
// public static class ExcursionExtensions
// {
//     public static Excursion ToExcursion(this ExcursionRecord excursionRecord)
//     {
//         if (excursionRecord == null)
//         {
//             throw new ArgumentNullException(nameof(excursionRecord));
//         }
//
//         return new Excursion
//         {
//             ExcursionId = excursionRecord.ExcursionId,
//             FileNumber = excursionRecord.FileNumber,
//             Description = excursionRecord.Description,
//             PayeeCompany = excursionRecord.PayeeCompany,
//             PayeeCompanyReference = excursionRecord.PayeeCompanyReference,
//             ExcursionName = excursionRecord.ExcursionName,
//             LocalAgent = excursionRecord.LocalAgent,
//             NoteText1 = excursionRecord.NoteText1,
//             NoteText2 = excursionRecord.NoteText2,
//             NoteText3 = excursionRecord.NoteText3,
//             DeparturePoint = excursionRecord.DeparturePoint,
//             ArrivalPoint = excursionRecord.ArrivalPoint,
//             StartDate = excursionRecord.StartDate,
//             StartTime = excursionRecord.StartTime,
//             EndDate = excursionRecord.EndDate,
//             EndTime = excursionRecord.EndTime
//         };
//     }
// }