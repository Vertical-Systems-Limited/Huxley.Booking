using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Itinerary.Domain.ExtraNotes;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.ExtraNotes.Data;

public interface IExtraNoteRepository
{
    Task<IEnumerable<ExtraNote>> GetExtraNotesForFileNo(string fileNo);
}

public class ExtraNoteRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IExtraNoteRepository
{
    public async Task<IEnumerable<ExtraNote>> GetExtraNotesForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINEXT.RUN",
                inputXml
            );
        
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching Extra Notes: " + result.Message);

            var extraNotesRecords = result.OutputXml.DeserializeXmlToEnumerableOf<ExtraNoteRecord>();
        
            // Use the new mapping extension
            return extraNotesRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get Extra Notes by file number {FileNo}", fileNo);
            throw;
        }
    }
}