using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Itinerary.Domain.Transfers;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Transfers.Data;

public interface ITransferRepository
{
    Task<IEnumerable<Transfer>> GetTransfersForFileNo(string fileNo);
}

public class TransferRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : ITransferRepository
{
    public async Task<IEnumerable<Transfer>> GetTransfersForFileNo(string fileNo)
    {
        try
        {
            // get the transfers from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINTRA.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching transfers: " + result.Message);

            // deserialize the output xml
            var transferRecords = result.OutputXml.DeserializeXmlToEnumerableOf<TransferRecord>();
            
            // map to transfers and return
            return transferRecords.ToTransfers();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get transfers by file number {FileNo}", fileNo);
            throw;
        }
    }
}