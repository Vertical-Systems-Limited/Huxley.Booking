using Huxley.Booking.Core.Costs.Contracts;
using Huxley.Booking.Core.Passengers.Contracts;
using Huxley.Itinerary.Domain;

namespace Huxley.Booking.Core.Bookings.Contracts;

public class CreateBookingDto
{
    // Page 1
    public string AgentInitials { get; set; }
    public string BranchNumber { get; set; }
    public string LeadSupplier { get; set; }
    public string PrimaryCode { get; set; }
    public string SecondaryCode { get; set; }
    public string TertiaryCode { get; set; }
    public string SubTertiaryCode { get; set; }
    public string DeparturePoint { get; set; }
    public string ArrivalPoint { get; set; }
    public string DepartureDate { get; set; }
    public string Duration { get; set; }
    public string Reference1 { get; set; }
    public string Reference2 { get; set; }

    // Page 2
    public string HotelName { get; set; }
    public string BoardBasis { get; set; }
    public string RoomType { get; set; }

    // Contact info
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string PrimaryPhoneNumber { get; set; }
    public string SecondaryPhoneNumber { get; set; }

    // Other Info
    public string DpaFlag1 { get; set; }
    public string DpaFlag2 { get; set; }
    
    // Collections
    public CreatePassengerDto[] Passengers { get; set; }
    public CreateCostDto[] Costs { get; set; }
    
    public ItineraryItemDto[] Itinerary { get; set; }
    
}



