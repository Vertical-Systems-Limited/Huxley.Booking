using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.ExtraNotes;

namespace Huxley.Booking.Core.Itinerary.ExtraNotes.Data;

public class ExtraNoteRecord
{
    public string FileNumber { get; set; }
    public string NoteText1 { get; set; }
    public string NoteText2 { get; set; }
    public string NoteText3 { get; set; }
    public string NoteText4 { get; set; }
    public string NoteText5 { get; set; }
    public string NoteText6 { get; set; }
    public string NoteDate { get; set; }
    public string NoteTime { get; set; }
    public string Block { get; set; }
}

public static class ExtraNoteRecordExtensions
{
    public static IEnumerable<ExtraNote> ToDomainModels(this IEnumerable<ExtraNoteRecord> records)
    {
        return records.Select(r => r.ToExtraNote());
    }

    public static ExtraNote ToExtraNote(this ExtraNoteRecord record)
    {
        // Combine Notes
        var notes =
            $"{record.NoteText1} {record.NoteText2} {record.NoteText3} {record.NoteText4} {record.NoteText5} {record.NoteText6}";

        var domain = ExtraNote.Create(record.NoteDate.ParseDateTime(record.NoteTime), notes ?? string.Empty);

        domain.FileNumber = record.FileNumber;
        domain.Block = record.Block;

        return domain;
    }
}