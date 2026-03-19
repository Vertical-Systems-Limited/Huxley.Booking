// using Huxley.Booking.Core.Itinerary.SurfaceSectors.Data;
//
// namespace Huxley.Booking.Core.Itinerary.SurfaceSectors;
//
// public class SurfaceSector
// {
//     public string SurfaceSectorId { get; set; }
//     public string FileNumber { get; set; }
//     public string NoteText1 { get; set; }
//     public string NoteText2 { get; set; }
//     public string NoteText3 { get; set; }
//     public string NoteText4 { get; set; }
//     public string StartDate { get; set; }
//     public string StartTime { get; set; }
//     public string EndDate { get; set; }
//     public string EndTime { get; set; }
// }
//
// public static class SurfaceSectorExtensions
// {
//     public static SurfaceSector ToSurfaceSector(this SurfaceSectorRecord surfaceSectorRecord)
//     {
//         if (surfaceSectorRecord == null)
//         {
//             throw new ArgumentNullException(nameof(surfaceSectorRecord));
//         }
//
//         return new SurfaceSector
//         {
//             SurfaceSectorId = surfaceSectorRecord.SurfaceSectorId,
//             FileNumber = surfaceSectorRecord.FileNumber,
//             NoteText1 = surfaceSectorRecord.NoteText1,
//             NoteText2 = surfaceSectorRecord.NoteText2,
//             NoteText3 = surfaceSectorRecord.NoteText3,
//             NoteText4 = surfaceSectorRecord.NoteText4,
//             StartDate = surfaceSectorRecord.StartDate,
//             StartTime = surfaceSectorRecord.StartTime,
//             EndDate = surfaceSectorRecord.EndDate,
//             EndTime = surfaceSectorRecord.EndTime
//         };
//     }
// }