using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.Excursions; 

namespace Huxley.Booking.Core.Itinerary.Excursions.Data;

public interface IExcursionRepository
{
    Task<IEnumerable<Excursion>> GetExcursionsForFileNo(string fileNo);
}

public class ExcursionRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IExcursionRepository
{
    public async Task<IEnumerable<Excursion>> GetExcursionsForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINEXC.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching excursions: " + result.Message);

            var excursionRecords = result.OutputXml.DeserializeXmlToEnumerableOf<ExcursionRecord>();
            
            // Map the legacy records to the central domain models
            return excursionRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get excursions by file number {FileNo}", fileNo);
            throw;
        }
    }
}