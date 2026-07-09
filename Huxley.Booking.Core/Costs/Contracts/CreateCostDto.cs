namespace Huxley.Booking.Core.Costs.Contracts;

public class CreateCostDto
{
    public string Supplier { get; set; }
    public string CostType { get; set; } //change to enum
    public string Notes { get; set; }
    public string CostCode { get; set; }
    public string Reference { get; set; }
    public string Amount { get; set; }
    public bool AutoCommissionFlag { get; set; }
    public string Vat { get; set; }
    public string Initials { get; set; }

}

