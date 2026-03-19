using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.Ferries;

namespace Huxley.Booking.Core.Itinerary.Ferries.Data;

public interface IFerryRepository
{
    Task<IEnumerable<Ferry>> GetFerriesForFileNo(string fileNo);
}

public class FerryRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IFerryRepository
{
    public async Task<IEnumerable<Ferry>> GetFerriesForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINFRY.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching ferries: " + result.Message);

            var ferryRecords = result.OutputXml.DeserializeXmlToEnumerableOf<FerryRecord>();
            
            // Map legacy XML records to Central Domain
            return ferryRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get ferries by file number {FileNo}", fileNo);
            throw;
        }
    }
}