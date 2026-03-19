using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Itinerary.Domain.Flights;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Flights.Data;

public interface IFlightRepository
{
    Task<IEnumerable<Flight>> GetFlightsByFileNo(string fileNo);
}

public class FlightRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context ) : IFlightRepository
{
    public async Task<IEnumerable<Flight>> GetFlightsByFileNo(string fileNo)
    {
        try
        {
            // get the flights from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINFLI.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching flight: " + result.Message);

            // deserialize the output xml
            var flightRecords = result.OutputXml.DeserializeXmlToEnumerableOf<FlightRecord>();
            
            // map to flights and return
            return flightRecords.ToFlights().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get flights by file number {FileNo}", fileNo);
            throw;
        }
    }
}