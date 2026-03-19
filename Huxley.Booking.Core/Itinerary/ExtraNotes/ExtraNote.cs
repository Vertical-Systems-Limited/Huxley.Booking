// using Huxley.Booking.Core.Itinerary.ExtraNotes.Data;
//
// namespace Huxley.Booking.Core.Itinerary.ExtraNotes;
//
// public class ExtraNote
// {
//     public string FileNumber { get; set; }
//     public string NoteText1 { get; set; }
//     public string NoteText2 { get; set; }
//     public string NoteText3 { get; set; }
//     public string NoteText4 { get; set; }
//     public string NoteText5 { get; set; }
//     public string NoteText6 { get; set; }
//     public string NoteDate { get; set; }
//     public string NoteTime { get; set; }
//     public string Block { get; set; }
// }
//
// public static class ExtraNoteExtensions
// {
//     public static ExtraNote ToExtraNote(this ExtraNoteRecord extraNoteRecord)
//     {
//         if (extraNoteRecord == null)
//         {
//             throw new ArgumentNullException(nameof(extraNoteRecord));
//         }
//         
//         return new ExtraNote
//         {
//             FileNumber = extraNoteRecord.FileNumber,
//             NoteText1 = extraNoteRecord.NoteText1,
//             NoteText2 = extraNoteRecord.NoteText2,
//             NoteText3 = extraNoteRecord.NoteText3,
//             NoteText4 = extraNoteRecord.NoteText4,
//             NoteText5 = extraNoteRecord.NoteText5,
//             NoteText6 = extraNoteRecord.NoteText6,
//             NoteDate = extraNoteRecord.NoteDate,
//             NoteTime = extraNoteRecord.NoteTime,
//             Block = extraNoteRecord.Block
//         }; 
//     }
// }