using Huxley.Booking.Core.Itinerary.Cruises.CruiseDetails.Data;
using Huxley.Booking.Core.Itinerary.Cruises.CruiseItineraries.Data;
using Huxley.Itinerary.Domain.Cruises;

namespace Huxley.Booking.Core.Itinerary.Cruises;

public interface ICruiseService
{
    Task<IEnumerable<Cruise>> GetCruisesForFileNo(string fileNo);
}

public class CruiseService(
    ICruiseDetailsRepository cruiseDetailsRepository,
    ICruiseItineraryRepository cruiseItineraryRepository
    ) : ICruiseService
{
    public async Task<IEnumerable<Cruise>> GetCruisesForFileNo(string fileNo)
    {
        // Fetch both details and itineraries in parallel
        var detailsTask = cruiseDetailsRepository.GetCruiseDetailsRecordsForFileNo(fileNo);
        var itinerariesTask = cruiseItineraryRepository.GetCruiseItineraryRecordsForFileNo(fileNo);

        await Task.WhenAll(detailsTask, itinerariesTask);

        var detailsRecords = detailsTask.Result;
        var itineraryRecords = itinerariesTask.Result;

        // Use the extension method to combine and map to domain models
        return detailsRecords.ToCruises(itineraryRecords);
    }
}