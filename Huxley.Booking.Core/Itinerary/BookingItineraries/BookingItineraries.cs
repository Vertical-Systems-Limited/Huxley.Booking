// using Huxley.Itinerary.Domain;
// using Huxley.Itinerary.Domain.Cruises.Contracts;
// using Huxley.Itinerary.Domain.Flights.Contracts;
// using Huxley.Itinerary.Domain.Transfers.Contracts;
//
// namespace Huxley.Booking.Core.Itineraries.BookingItineraries;
//
// public class BookingItinerary
// {
//     // Domain DTOs (inheriting from ItineraryItemDto)
//     public FlightDto[] Flights { get; set; }
//     public TransferDto[] Transfers { get; set; }
//     public CruiseDto[] Cruises { get; set; }
//     
//     // For itinerary types that don't have domain models yet, keep as raw data
//     public Accommodations.Accommodation[] Accommodations { get; set; }
//     public CarHires.CarHire[] CarHires { get; set; }
//     public Excursions.Excursion[] Excursions { get; set; }
//     public ExtraNotes.ExtraNote[] ExtraNotes { get; set; }
//     public Ferries.Ferry[] Ferries { get; set; }
//     public Parking.Parking[] Parking { get; set; }
//     public Rail.Rail[] Rails { get; set; }
//     public Restaurants.Restaurant[] Restaurants { get; set; }
//     public SurfaceSectors.SurfaceSector[] SurfaceSectors { get; set; }
//     public Transport.Transport[] Transports { get; set; }
// }