using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Cruises.CruiseItineraries.Data;

public interface ICruiseItineraryRepository
{
    Task<IEnumerable<CruiseItineraryRecord>> GetCruiseItineraryRecordsForFileNo(string fileNo);
}

public class CruiseItineraryRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : ICruiseItineraryRepository
{
    public async Task<IEnumerable<CruiseItineraryRecord>> GetCruiseItineraryRecordsForFileNo(string fileNo)
    {
        try
        {
            // get the cruise itinerary from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINCIT.RUN",
                inputXml
            );
        
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching cruise itinerary: " + result.Message);

            // deserialize the output xml
            var cruiseItineraryRecords = result.OutputXml.DeserializeXmlToEnumerableOf<CruiseItineraryRecord>();
        
            return cruiseItineraryRecords.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get cruise itinerary by file number {FileNo}", fileNo);
            throw;
        }
    }
}