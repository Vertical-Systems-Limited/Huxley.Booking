namespace Huxley.Booking.Core.Branches.Contracts;

public class BranchViewDto
{
    public int BranchNo { get; set; }
    public string BranchName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public string AlternativePhone { get; set; }
}