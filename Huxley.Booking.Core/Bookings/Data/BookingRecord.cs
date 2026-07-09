using Huxley.Booking.Core.Bookings.Contracts;

namespace Huxley.Booking.Core.Bookings.Data;

public class BookingRecord
{
    // Page 1
    public string TarscInitials { get; set; }
    public string BranchNumber { get; set; }
    public string LeadCompany { get; set; }
    public string AnalysisCode { get; set; }
    public string SecondaryCode { get; set; }
    public string TertiaryCode { get; set; }
    public string SubTertiaryCode { get; set; }
    public string MarketingCode => $"{SecondaryCode}/{TertiaryCode}/{SubTertiaryCode}";
    public string DeparturePoint { get; set; }
    public string DestinationPoint { get; set; }
    public string DepartureDate { get; set; }
    public string Duration { get; set; }
    public string LeadCompanyReference { get; set; }
    public string SubReference { get; set; }

    // Page 2
    public string Hotel { get; set; }
    public string Board { get; set; }
    public string AccommodationRemark1 { get; set; }

    // Contact info
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Phone { get; set; }
    public string AlternativePhone { get; set; }

    // Other Info
    public string DataProtectionFlag1 { get; set; }
    public string DataProtectionFlag2 { get; set; }

    public static BookingRecord MapFromDto(CreateBookingDto dto)
    {
        return new BookingRecord()
        {
            TarscInitials = dto.AgentInitials,
            BranchNumber = dto.BranchNumber,
            LeadCompany = dto.LeadSupplier,
            AnalysisCode = dto.PrimaryCode,
            SecondaryCode = dto.SecondaryCode,
            TertiaryCode = dto.TertiaryCode,
            SubTertiaryCode = dto.SubTertiaryCode,
            DeparturePoint = dto.DeparturePoint,
            DestinationPoint = dto.ArrivalPoint,
            DepartureDate = dto.DepartureDate,
            Duration = dto.Duration,
            LeadCompanyReference = dto.Reference1,
            SubReference = dto.Reference2,
            Hotel = dto.HotelName,
            Board = dto.BoardBasis,
            AccommodationRemark1 = dto.RoomType,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            AddressLine3 = dto.AddressLine3,
            AddressLine4 = dto.AddressLine4,
            Phone = dto.PrimaryPhoneNumber,
            AlternativePhone = dto.SecondaryPhoneNumber,
            DataProtectionFlag1 = dto.DpaFlag1,
            DataProtectionFlag2 = dto.DpaFlag2
        };
    }
}