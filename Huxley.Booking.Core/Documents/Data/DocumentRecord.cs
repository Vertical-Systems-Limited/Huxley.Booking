namespace Huxley.Booking.Core.Documents.Data;

public class DocumentRecord
{
    public string FileNumber { get; set; }
    public string LetterDate { get; set; }
    public string Description { get; set; }
    public string LetterType { get; set; }
    public string TemplateName { get; set; }
    public string Location { get; set; }
    public string ReceiptNumber { get; set; }
}

public static class DocumentRecordExtensions
{
    public static Document ToDocument(this DocumentRecord documentRecord)
    {
        if (documentRecord == null)
        {
            throw new ArgumentNullException(nameof(documentRecord));
        }

        return new Document
        {
            FileNumber = documentRecord.FileNumber,
            LetterDate = documentRecord.LetterDate,
            Description = documentRecord.Description,
            LetterType = documentRecord.LetterType,
            TemplateName = documentRecord.TemplateName,
            Location = documentRecord.Location,
            ReceiptNumber = documentRecord.ReceiptNumber
        };
    }
}