using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Excursions;

namespace Huxley.Booking.Core.Itinerary.Excursions.Data;

public class ExcursionRecord
{
    public string ExcursionId { get; set; }
    public string FileNumber { get; set; }
    public string Description { get; set; }
    public string PayeeCompany { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string ExcursionName { get; set; }
    public string LocalAgent { get; set; }
    public string NoteText1 { get; set; }
    public string NoteText2 { get; set; }
    public string NoteText3 { get; set; }
    public string DeparturePoint { get; set; }
    public string ArrivalPoint { get; set; }
    public string StartDate { get; set; }
    public string StartTime { get; set; }
    public string EndDate { get; set; }
    public string EndTime { get; set; }
}

public static class ExcursionRecordExtensions
{
    public static IEnumerable<Excursion> ToDomainModels(this IEnumerable<ExcursionRecord> records)
    {
        return records.Select(r => r.ToDomain());
    }

    public static Excursion ToDomain(this ExcursionRecord record)
    {
        var domain = Excursion.Create(
            record.ExcursionName,
            record.DeparturePoint,
            record.StartDate.ParseDateTime(record.StartTime),
            record.ArrivalPoint,
            record.EndDate.ParseDateTime(record.EndTime)
        );
        
        domain.Description = record.Description;
        domain.LocalAgent = record.LocalAgent;
        domain.FileNumber = record.FileNumber;
        domain.PayeeCompany = record.PayeeCompany;
        domain.NotesText = $"{record.NoteText1} {record.NoteText2} {record.NoteText3}";
        domain.Reference = record.PayeeCompanyReference;

        return domain;
    }
}