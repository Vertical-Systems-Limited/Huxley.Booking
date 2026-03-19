using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.Transports;

namespace Huxley.Booking.Core.Itinerary.Transports.Data;

public interface ITransportRepository
{
    Task<IEnumerable<Transport>> GetTransportsForFileNo(string fileNo);
}

public class TransportRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : ITransportRepository
{
    public async Task<IEnumerable<Transport>> GetTransportsForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINTRP.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching transports: " + result.Message);

            var transportRecords = result.OutputXml.DeserializeXmlToEnumerableOf<TransportRecord>();
            
            // Map legacy XML records to Central Domain
            return transportRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get transports by file number {FileNo}", fileNo);
            throw;
        }
    }
}