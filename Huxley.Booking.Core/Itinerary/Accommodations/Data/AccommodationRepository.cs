using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Itinerary.Domain.Accommodations;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Accommodations.Data;

public interface IAccommodationRepository
{
    Task<IEnumerable<Accommodation>> GetAccommodationsByFileNo(string fileNo);
}

public class AccommodationRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IAccommodationRepository
{
    public async Task<IEnumerable<Accommodation>> GetAccommodationsByFileNo(string fileNo)
    {
        try
        {
            // get the accommodations from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINACC.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching accommodations: " + result.Message);

            // deserialize the output xml
            var flightRecords = result.OutputXml.DeserializeXmlToEnumerableOf<AccommodationRecord>();
            
            // map to accommodations and return
            return flightRecords.ToAccommodations().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get accommodations by file number {FileNo}", fileNo);
            throw;
        }
    }
}