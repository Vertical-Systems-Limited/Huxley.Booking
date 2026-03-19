using Huxley.Booking.Core.Itinerary.Cruises.CruiseDetails.Data;
using Huxley.Booking.Core.Itinerary.Cruises.CruiseItineraries.Data;
using Huxley.Itinerary.Domain.Cruises;

namespace Huxley.Booking.Core.Itinerary.Cruises;

public static class CruiseRecordExtensions
{
    public static IEnumerable<Cruise> ToCruises(
        this IEnumerable<CruiseDetailsRecord> detailsRecords,
        IEnumerable<CruiseItineraryRecord> itineraryRecords)
    {
        var cruises = new List<Cruise>();

        foreach (var details in detailsRecords)
        {
            var cruise = Cruise.Create(
                shipName: details.ShipName ?? "",
                itineraryName: details.ItineraryTitle ?? "",
                duration: int.TryParse(details.Duration, out var dur) ? dur : 0,
                departureDate: ParseDateOnly(details.DepartDate)
            );

            // Add cabin information
            if (!string.IsNullOrWhiteSpace(details.CabinCategory) || !string.IsNullOrWhiteSpace(details.CabinNumber))
            {
                var cabin = CruiseCabin.Create(
                    cabinType: details.CabinCategory ?? "",
                    cabinNumber: details.CabinNumber ?? "",
                    deck: details.DeckName ?? "",
                    boardBasis: "" // Not in TARSC data
                );
                cruise.AddCabin(cabin);
            }

            // Add notes
            if (!string.IsNullOrWhiteSpace(details.SpecialRequests))
                cruise.SetEmbarkationNotes(details.SpecialRequests);
            
            if (!string.IsNullOrWhiteSpace(details.Remarks))
                cruise.SetDisembarkationNotes(details.Remarks);

            // Find and add matching itinerary items
            var matchingItineraries = itineraryRecords
                .Where(i => i.FileNumber == details.FileNumber)
                .OrderBy(i => ParseDateOnly(i.ArrivalDate));

            int day = 1;
            foreach (var itinRecord in matchingItineraries)
            {
                var itinerary = CruiseItinerary.Create(
                    day: day++,
                    port: itinRecord.ArrivalPort ?? "At Sea",
                    arrivalTime: ParseTimeOnly(itinRecord.ArrivalTime),
                    departureTime: ParseTimeOnly(itinRecord.DepartTime),
                    date: ParseDateOnly(itinRecord.ArrivalDate)
                );
                cruise.AddItineraryItem(itinerary);
            }

            cruises.Add(cruise);
        }

        return cruises;
    }

    private static DateOnly ParseDateOnly(string date)
    {
        if (DateOnly.TryParse(date, out var result))
            return result;
        return DateOnly.MinValue;
    }

    private static TimeOnly? ParseTimeOnly(string time)
    {
        if (string.IsNullOrWhiteSpace(time))
            return null;
            
        if (TimeOnly.TryParse(time, out var result))
            return result;
        return null;
    }
}