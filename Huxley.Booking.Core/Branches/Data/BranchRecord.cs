using Huxley.Booking.Core.Bookings;
using Huxley.Booking.Core.Branches.Contracts;

namespace Huxley.Booking.Core.Branches.Data;

// ReSharper disable InconsistentNaming
public class BranchRecord
{
    public string BranchNumber { get; set; }
    public string Group { get; set; }
    public string ABTANumber { get; set; }
    public string ATOLNumber { get; set; }
    public string IATANumber { get; set; }
    public string DefaultNominal { get; set; }
    public string AccountNumber { get; set; }
    public string CostCentre { get; set; }
    public string ChequeNumber { get; set; }
    public string CreditNoteNumber { get; set; }
    public string BatchNumber { get; set; }
    public string ProfileNumber { get; set; }
    public string ReceiptNumber { get; set; }
    public string GroupNumber { get; set; }
    public string InvoiceNumber { get; set; }
    public string VoucherNumber { get; set; }
    public string LinkNumber { get; set; }
    public string RetailFileNumber { get; set; }
    public string BusinessHouseFileNumber { get; set; }
    public string VATNumber { get; set; }
    public string CoopFlag { get; set; }
    public string StopFlag { get; set; }
    public string CountryCode { get; set; }
    public string CurrencyCode { get; set; }
    public string CreditAvailableDays { get; set; }
    public string DueNowDays { get; set; }
    public string OverdueWeeks { get; set; }
    public string CreditTakenOnAccountDays { get; set; }
    public string LastPaymentValue { get; set; }
    public string LastPaymentDate { get; set; }
    public string NumberOfPaymentsUsedToCalculatePay { get; set; }
    public string PrintRemittanceAdviceFlag { get; set; }
    public string RegionID { get; set; }
    public string LongName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string EmailAddress { get; set; }
    public string MMBUrl { get; set; }
    public string URL { get; set; }
    public string WebAddress { get; set; }
    public string Phone { get; set; }
    public string AlternativePhone { get; set; }
    public string SupplierType { get; set; }
    public string BankingDate { get; set; }
    public string GroupHeaderAddressLine1 { get; set; }
    public string GroupHeaderAddressLine2 { get; set; }
    public string GroupHeaderAddressLine3 { get; set; }
    public string GroupHeaderAddressLine4 { get; set; }
    public string GroupHeaderName { get; set; }
    public string CommissionRate { get; set; }
    public string AlternativeCommissionRate { get; set; }
    public string ClusterID1 { get; set; }
    public string ClusterID2 { get; set; }
    public string ClusterID3 { get; set; }
    public string ClusterID4 { get; set; }
    public string ClusterID5 { get; set; }
    public string ClusterID6 { get; set; }
    public string Password1 { get; set; }
    public string Password2 { get; set; }
    public string Password3 { get; set; }
    public string Password4 { get; set; }
    public string Password5 { get; set; }
    public string Password6 { get; set; }
    public string Password7 { get; set; }
    public string Password8 { get; set; }
    public string Password9 { get; set; }
    public string Password10 { get; set; }
    public string VATDescription1 { get; set; }
    public string VATDescription2 { get; set; }
    public string VATDescription3 { get; set; }
    public string VATDescription4 { get; set; }
    public string VATDescription5 { get; set; }
    public string VATDescription9 { get; set; }
    public string VATDescription10 { get; set; }
    public string SMSDisplayName { get; set; }
    public string SMSPhoneNumber1 { get; set; }
    public string SMSPhoneNumber2 { get; set; }
    public string VATRate1 { get; set; }
    public string VATRate2 { get; set; }
    public string VATRate3 { get; set; }
    public string VATRate4 { get; set; }
    public string VATRate5 { get; set; }
    public string VATRate6 { get; set; }
    public string VATRate7 { get; set; }
    public string VATRate8 { get; set; }
    public string VATRate9 { get; set; }
    public string VATRate10 { get; set; }
}

public static class BranchRecordExtensions
{
    public static BranchViewDto MapToDto(this BranchRecord record)
    {
        return new BranchViewDto
        {
            BranchNo = record.BranchNumber.ToInt(),
            BranchName = record.LongName,
            AddressLine1 = record.AddressLine1,
            AddressLine2 = record.AddressLine2,
            AddressLine3 = record.AddressLine3,
            AddressLine4 = record.AddressLine4,
            EmailAddress = record.EmailAddress,
            Phone = record.Phone,
            AlternativePhone = record.AlternativePhone
        };
    }
}