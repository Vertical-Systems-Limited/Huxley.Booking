using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Passengers.Data;

public interface IPassengerRepository
{
    Task<IEnumerable<Passenger>>  GetPassengersByFileNo(string fileNo);
}

public class PassengerRepository(
    ILogger<PassengerRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
    ) : IPassengerRepository
{
    public async Task<IEnumerable<Passenger>>  GetPassengersByFileNo(string fileNo)
    {
        try
        {
            // get the passengers from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-LISTPAS.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching passengers: " + result.Message);

            // deserialize the output xml
            var passengerRecords = result.OutputXml.DeserializeXmlToEnumerableOf<PassengerRecord>();
            
            // map to passengers and return
            return passengerRecords.Select(r => r.ToPassenger()).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get passengers by file number {FileNo}", fileNo);
            throw;
        }
    }  
}