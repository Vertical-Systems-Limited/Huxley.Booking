using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.SurfaceSectors;

namespace Huxley.Booking.Core.Itinerary.SurfaceSectors.Data;

public class SurfaceSectorRecord
{
    public string SurfaceSectorId { get; set; }
    public string FileNumber { get; set; }
    public string NoteText1 { get; set; }
    public string NoteText2 { get; set; }
    public string NoteText3 { get; set; }
    public string NoteText4 { get; set; }
    public string StartDate { get; set; }
    public string StartTime { get; set; }
    public string EndDate { get; set; }
    public string EndTime { get; set; }
}

public static class SurfaceSectorRecordExtensions
{
    public static IEnumerable<SurfaceSector> ToDomainModels(this IEnumerable<SurfaceSectorRecord> records)
    {
        return records.Select(r => r.ToSurfaceSector());
    }

    public static SurfaceSector ToSurfaceSector(this SurfaceSectorRecord record)
    {
        var domain = SurfaceSector.Create(
            record.StartDate.ParseDateTime(record.StartTime),
            record.EndDate.ParseDateTime(record.EndTime),
            $"{record.NoteText1} {record.NoteText2} {record.NoteText3} {record.NoteText4}" 
        );

        domain.FileNumber = record.FileNumber;

        return domain;
    }
}