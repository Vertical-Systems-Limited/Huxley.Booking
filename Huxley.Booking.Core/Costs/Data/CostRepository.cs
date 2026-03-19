using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Costs.Data;

public interface ICostRepository
{
    Task<IEnumerable<Cost>> GetCostsForFileNo(string fileNo);
}

public class CostRepository(
    ILogger<CostRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
    ) : ICostRepository
{
    public async Task<IEnumerable<Cost>> GetCostsForFileNo(string fileNo)
    {
        try
        {
            // get the costs from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-COSTLST.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching costs: " + result.Message);

            // deserialize the output xml
            var costRecords = result.OutputXml.DeserializeXmlToEnumerableOf<CostRecord>();
            
            // map to costs and return
            return costRecords.Select(cr => cr.ToCost()).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get costs for file number {FileNo}", fileNo);
            throw;
        }
    }
}