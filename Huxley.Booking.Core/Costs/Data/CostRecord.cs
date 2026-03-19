using Huxley.Booking.Core.Helpers;

namespace Huxley.Booking.Core.Costs.Data;

public class CostRecord
{
    public string FileNumber { get; set; }
    public string SaleDate { get; set; }
    public string Company { get; set; }
    public string Code { get; set; }
    public string CodeType { get; set; }
    public string AnalysisCode { get; set; }
    public string CostType { get; set; }
    public string TicketCostType { get; set; }
    public string Notes { get; set; }
    public string Value { get; set; }
    public string VAT { get; set; }
    public string VATCode { get; set; }
    public string VATDate { get; set; }
    public string AutoCommissionFlag { get; set; }
    public string SubTotal { get; set; }
    public string Total { get; set; }
    public string Reference { get; set; }
    public string Initials { get; set; }
    public string DiscountPassword { get; set; }
    public string NegativeFactorPassword { get; set; }
    public string RestrictedDiscountPassword { get; set; }
    public string FailureDiscountPassword { get; set; }
}

public static class CostRecordExtensions
{
    public static Cost ToCost(this CostRecord costRecord)
    {
        if (costRecord == null)
        {
            throw new ArgumentNullException(nameof(costRecord));
        }

        return new Cost
        {
            FileNumber = costRecord.FileNumber,
            SaleDate = costRecord.SaleDate.ParseDateOnly(), // DateOnly.TryParse(costRecord.SaleDate, out var saleDate) ? saleDate : default,
            Company = costRecord.Company,
            Code = costRecord.Code,
            CodeType = costRecord.CodeType,
            AnalysisCode = costRecord.AnalysisCode,
            CostType = costRecord.CostType,
            TicketCostType = costRecord.TicketCostType,
            Notes = costRecord.Notes,
            Value = decimal.TryParse(costRecord.Value, out var value) ? value : 0,
            Vat = decimal.TryParse(costRecord.VAT, out var vat) ? vat : 0,
            VatCode = costRecord.VATCode,
            VatDate = costRecord.VATDate.ParseDateOnly(), //DateOnly.TryParse(costRecord.VATDate, out var vatDate) ? vatDate : default,
            AutoCommissionFlag = costRecord.AutoCommissionFlag,
            SubTotal = decimal.TryParse(costRecord.SubTotal, out var subTotal) ? subTotal : 0,
            Total = decimal.TryParse(costRecord.Total, out var total) ? total : 0,
            Reference = costRecord.Reference,
            Initials = costRecord.Initials
        };
    }
}