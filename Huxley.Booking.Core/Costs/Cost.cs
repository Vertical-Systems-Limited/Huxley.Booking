namespace Huxley.Booking.Core.Costs;

public class Cost
{
    public string FileNumber { get; set; }
    public DateOnly SaleDate { get; set; }
    public string Company { get; set; }
    public string Code { get; set; }
    public string CodeType { get; set; }
    public string AnalysisCode { get; set; }
    public string CostType { get; set; }
    public string TicketCostType { get; set; }
    public string Notes { get; set; }
    public decimal Value { get; set; }
    public decimal Vat { get; set; }
    public string VatCode { get; set; }
    public DateOnly VatDate { get; set; }
    public string AutoCommissionFlag { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public string Reference { get; set; }
    public string Initials { get; set; }
}