using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.Parkings;

namespace Huxley.Booking.Core.Itinerary.Parkings.Data;

public interface IParkingRepository
{
    Task<IEnumerable<Parking>> GetParkingForFileNo(string fileNo);
}

public class ParkingRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IParkingRepository
{
    public async Task<IEnumerable<Parking>> GetParkingForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINPRK.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching parkings: " + result.Message);

            var parkingRecords = result.OutputXml.DeserializeXmlToEnumerableOf<ParkingRecord>();
            
            // Map legacy XML records to Central Domain
            return parkingRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get parkings by file number {FileNo}", fileNo);
            throw;
        }
    }
}