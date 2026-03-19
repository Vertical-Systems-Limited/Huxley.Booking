using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.CarHires; 

namespace Huxley.Booking.Core.Itinerary.CarHires.Data;

public interface ICarHireRepository
{
    Task<IEnumerable<CarHire>> GetCarHiresForFileNo(string fileNo);
}

public class CarHireRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : ICarHireRepository
{
    public async Task<IEnumerable<CarHire>> GetCarHiresForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINCAR.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching car hires: " + result.Message);

            var carHireRecords = result.OutputXml.DeserializeXmlToEnumerableOf<CarHireRecord>();
            
            // Use the new mapping extension defined below
            return carHireRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get car hires by file number {FileNo}", fileNo);
            throw;
        }
    }
}