using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.SurfaceSectors;

namespace Huxley.Booking.Core.Itinerary.SurfaceSectors.Data;

public interface ISurfaceSectorRepository
{
    Task<IEnumerable<SurfaceSector>> GetSurfaceSectorsForFileNo(string fileNo);
}

public class SurfaceSectorRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : ISurfaceSectorRepository
{
    public async Task<IEnumerable<SurfaceSector>> GetSurfaceSectorsForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINSUR.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching surface sectors: " + result.Message);

            var surfaceSectorRecords = result.OutputXml.DeserializeXmlToEnumerableOf<SurfaceSectorRecord>();
            
            // Map legacy records to Central Domain
            return surfaceSectorRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get surface sectors by file number {FileNo}", fileNo);
            throw;
        }
    }
}