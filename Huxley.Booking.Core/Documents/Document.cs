namespace Huxley.Booking.Core.Documents;

public class Document
{
    public string FileNumber { get; set; }
    public string LetterDate { get; set; }
    public string Description { get; set; }
    public string LetterType { get; set; }
    public string TemplateName { get; set; }
    public string Location { get; set; }
    public string ReceiptNumber { get; set; }
}

public class DocumentData
{
    public Byte[] Contents { get; set; }
}