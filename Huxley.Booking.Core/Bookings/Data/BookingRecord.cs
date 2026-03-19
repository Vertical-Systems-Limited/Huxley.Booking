namespace Huxley.Booking.Core.Bookings.Data;

public class BookingRecord
{
    public string FileNumber { get; set; }
    public string BranchNumber { get; set; }
    public string TarscInitials { get; set; }
    public string BookingType { get; set; }
    public string BookingDate { get; set; }
    public string DepartureDate { get; set; }
    public string DeparturePoint { get; set; }
    public string DestinationPoint { get; set; }
    public string LeadPassengerTitle { get; set; }
    public string LeadPassengerInitial { get; set; }
    public string LeadPassengerSurname { get; set; }
    public string LeadCompany { get; set; }
    public string LeadCompanyReference { get; set; }
    public string TourOpCustomerDescription { get; set; }
    public string TourOpCustomerAccountNumber { get; set; }
    public string BrochureCode { get; set; }
    public string SubReference { get; set; }
    public string HolidayNumber { get; set; }
    public string Profile { get; set; }
    public string AnalysisCode { get; set; }
    public string MarketingCode { get; set; }
    public string NoteText1 { get; set; }
    public string NoteText2 { get; set; }
    public string CarHireCompany { get; set; }
    public string CarHireVoucherNumber { get; set; }
    public string CarHireGroup { get; set; }
    public string CarHirePlan { get; set; }
    public string CarHirePickupLocation { get; set; }
    public string Hotel { get; set; }
    public string Board { get; set; }
    public string AccommodationRemark1 { get; set; }
    public string AccommodationRemark2 { get; set; }
    public string SpecialRequest1 { get; set; }
    public string SpecialRequest2 { get; set; }
    public string Status { get; set; }
    public string Duration { get; set; }
    public string OutstandingBalance { get; set; }
    public string BalanceDueDate { get; set; }
    public string GrossCost { get; set; }
    public string TotalDiscount { get; set; }
    public string NettCost { get; set; }
    public string NettReceiptFromClient { get; set; }
    public string TotalBalancePaid { get; set; }
    public string InsuranceCost { get; set; }
    public string FactorCost { get; set; }
    public string FactorVATCost { get; set; }
    public string SaleVATCost { get; set; }
    public string PromotionCost { get; set; }
    public string ControlCost { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Phone { get; set; }
    public string AlternativePhone { get; set; }
    public string CoOpMembershipNumber { get; set; }
    public string DataProtectionFlag1 { get; set; }
    public string DataProtectionFlag2 { get; set; }
    public string PreferredMailingType { get; set; }
    public string ConfidentialFlag { get; set; }
    public string SpecialAssistanceFlag { get; set; }
    public string SpecialAssistanceType { get; set; }
    public string FileType { get; set; }
    public string DPADate { get; set; }
    public string ConfirmedFlag { get; set; }
    public string TicketedFlag { get; set; }
    public string InsuranceNoteText { get; set; }
    public string DepartureDatePassword { get; set; }
    public string BalanceDueDatePassword { get; set; }
    public string LeadCompanyChangePassword { get; set; }
    public string TarscInitialsChangePassword { get; set; }
    public string Temp { get; set; }
    public string BalanceDueDateType { get; set; }
}

public static class BookingRecordExtensions
{
    public static Booking ToBooking(this BookingRecord bookingRecord)
    {
        if (bookingRecord == null)
        {
            throw new ArgumentNullException(nameof(bookingRecord));
        }

        return new Booking
        {
            FileNumber = bookingRecord.FileNumber,
            BranchNumber = int.TryParse(bookingRecord.BranchNumber, out var branchNumber) ? branchNumber : 0,
            TarscInitials = bookingRecord.TarscInitials,
            BookingType = bookingRecord.BookingType,
            BookingDate = DateTime.TryParse(bookingRecord.BookingDate, out var bookingDate) ? bookingDate : default,
            DepartureDate = DateTime.TryParse(bookingRecord.DepartureDate, out var departureDate) ? departureDate : default,
            DeparturePoint = bookingRecord.DeparturePoint,
            DestinationPoint = bookingRecord.DestinationPoint,
            LeadPassengerTitle = bookingRecord.LeadPassengerTitle,
            LeadPassengerInitial = bookingRecord.LeadPassengerInitial,
            LeadPassengerSurname = bookingRecord.LeadPassengerSurname,
            LeadCompany = bookingRecord.LeadCompany,
            LeadCompanyReference = bookingRecord.LeadCompanyReference,
            TourOpCustomerDescription = bookingRecord.TourOpCustomerDescription,
            TourOpCustomerAccountNumber = bookingRecord.TourOpCustomerAccountNumber,
            BrochureCode = bookingRecord.BrochureCode,
            SubReference = bookingRecord.SubReference,
            HolidayNumber = bookingRecord.HolidayNumber,
            Profile = bookingRecord.Profile,
            AnalysisCode = bookingRecord.AnalysisCode,
            MarketingCode = bookingRecord.MarketingCode,
            NoteText1 = bookingRecord.NoteText1,
            NoteText2 = bookingRecord.NoteText2,
            CarHireCompany = bookingRecord.CarHireCompany,
            CarHireVoucherNumber = bookingRecord.CarHireVoucherNumber,
            CarHireGroup = bookingRecord.CarHireGroup,
            CarHirePlan = bookingRecord.CarHirePlan,
            CarHirePickupLocation = bookingRecord.CarHirePickupLocation,
            Hotel = bookingRecord.Hotel,
            Board = bookingRecord.Board,
            AccommodationRemark1 = bookingRecord.AccommodationRemark1,
            AccommodationRemark2 = bookingRecord.AccommodationRemark2,
            SpecialRequest1 = bookingRecord.SpecialRequest1,
            SpecialRequest2 = bookingRecord.SpecialRequest2,
            Status = bookingRecord.Status,
            Duration = int.TryParse(bookingRecord.Duration, out var duration) ? duration : 0,
            OutstandingBalance = decimal.TryParse(bookingRecord.OutstandingBalance, out var outstandingBalance) ? outstandingBalance : 0,
            BalanceDueDate = DateTime.TryParse(bookingRecord.BalanceDueDate, out var balanceDueDate) ? balanceDueDate : default,
            GrossCost = decimal.TryParse(bookingRecord.GrossCost, out var grossCost) ? grossCost : 0,
            TotalDiscount = decimal.TryParse(bookingRecord.TotalDiscount, out var totalDiscount) ? totalDiscount : 0,
            NettCost = decimal.TryParse(bookingRecord.NettCost, out var nettCost) ? nettCost : 0,
            NettReceiptFromClient = decimal.TryParse(bookingRecord.NettReceiptFromClient, out var nettReceiptFromClient) ? nettReceiptFromClient : 0,
            TotalBalancePaid = decimal.TryParse(bookingRecord.TotalBalancePaid, out var totalBalancePaid) ? totalBalancePaid : 0,
            InsuranceCost = decimal.TryParse(bookingRecord.InsuranceCost, out var insuranceCost) ? insuranceCost : 0,
            FactorCost = decimal.TryParse(bookingRecord.FactorCost, out var factorCost) ? factorCost : 0,
            FactorVatCost = decimal.TryParse(bookingRecord.FactorVATCost, out var factorVatCost) ? factorVatCost : 0,
            SaleVatCost = decimal.TryParse(bookingRecord.SaleVATCost, out var saleVatCost) ? saleVatCost : 0,
            PromotionCost = decimal.TryParse(bookingRecord.PromotionCost, out var promotionCost) ? promotionCost : 0,
            ControlCost = decimal.TryParse(bookingRecord.ControlCost, out var controlCost) ? controlCost : 0,
            AddressLine1 = bookingRecord.AddressLine1,
            AddressLine2 = bookingRecord.AddressLine2,
            AddressLine3 = bookingRecord.AddressLine3,
            AddressLine4 = bookingRecord.AddressLine4,
            Phone = bookingRecord.Phone,
            AlternativePhone = bookingRecord.AlternativePhone,
            DataProtectionFlag1 = bookingRecord.DataProtectionFlag1,
            DataProtectionFlag2 = bookingRecord.DataProtectionFlag2,
            PreferredMailingType = bookingRecord.PreferredMailingType,
            ConfidentialFlag = bookingRecord.ConfidentialFlag,
            SpecialAssistanceFlag = bookingRecord.SpecialAssistanceFlag,
            SpecialAssistanceType = bookingRecord.SpecialAssistanceType,
            FileType = bookingRecord.FileType,
            DpaDate = DateTime.TryParse(bookingRecord.DPADate, out var dpaDate) ? dpaDate : default,
            ConfirmedFlag = bookingRecord.ConfirmedFlag,
            TicketedFlag = bookingRecord.TicketedFlag,
            InsuranceNoteText = bookingRecord.InsuranceNoteText,
            BalanceDueDateType = bookingRecord.BalanceDueDateType
        };
    }
}