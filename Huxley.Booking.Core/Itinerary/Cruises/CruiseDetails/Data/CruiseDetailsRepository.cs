using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Cruises.CruiseDetails.Data;


public interface ICruiseDetailsRepository
{
    Task<IEnumerable<CruiseDetailsRecord>> GetCruiseDetailsRecordsForFileNo(string fileNo);   
}

public class CruiseDetailsRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : ICruiseDetailsRepository
{
    public async Task<IEnumerable<CruiseDetailsRecord>> GetCruiseDetailsRecordsForFileNo(string fileNo)
    {
        try
        {
            // get the cruise details from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINCFO.RUN",
                inputXml
            );
        
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching cruise details: " + result.Message);

            // deserialize the output xml
            var cruiseDetailsRecords = result.OutputXml.DeserializeXmlToEnumerableOf<CruiseDetailsRecord>();
        
            return cruiseDetailsRecords.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get cruise details by file number {FileNo}", fileNo);
            throw;
        }
    }
}