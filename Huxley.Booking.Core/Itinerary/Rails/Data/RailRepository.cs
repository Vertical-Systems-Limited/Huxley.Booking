using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.Rails;

namespace Huxley.Booking.Core.Itinerary.Rails.Data;

public interface IRailRepository
{
    Task<IEnumerable<Rail>> GetRailsForFileNo(string fileNo);
}

public class RailRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IRailRepository
{
    public async Task<IEnumerable<Rail>> GetRailsForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINRAI.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching rails: " + result.Message);

            var railRecords = result.OutputXml.DeserializeXmlToEnumerableOf<RailRecord>();
            
            // Map legacy XML records to Central Domain
            return railRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get rails by file number {FileNo}", fileNo);
            throw;
        }
    }
}