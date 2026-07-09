using Huxley.Booking.Core.Bookings.Contracts;
using Huxley.Booking.Core.Costs.Contracts;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Costs.Data;

public interface ICostRepository
{
    Task<bool> InsertCosts(CreateBookingDto booking, string fileNumber);
    Task<IEnumerable<CostViewDto>> GetCostsForFileNo(string fileNo);
}

public class CostRepository(
    ILogger<CostRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
    ) : ICostRepository
{
    public async Task<bool> InsertCosts(CreateBookingDto booking, string fileNumber)
    {
        var costRecords = booking.Costs.Select(c => CostRecord.MapFromDto(c, fileNumber)).ToList();
        var xml = costRecords.ToDataSet(
            dataSetName: "dsTarscBookingCostList",
            tableName: "dtCost"
            ).GetXml();
        var result = await client.ExecuteRunFile(
            context.Tenant.TarscServerAddress,
            context.Tenant.TarscServerPort,
            "TARSCWS_V6\\-ADDCOST.RUN",
            xml
        );
        
        // check for errors
        if(result.ErrorOccurred)
            throw new Exception("Error occurred while inserting costs: " + result.Message);

        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<CostViewDto>> GetCostsForFileNo(string fileNo)
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