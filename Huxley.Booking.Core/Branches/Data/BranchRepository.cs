using Huxley.Booking.Core.AnalysisCodes.Caching;
using Huxley.Booking.Core.Bookings.Data;
using Huxley.Booking.Core.Branches.Caching;
using Huxley.Booking.Core.Branches.Contracts;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Huxley.Booking.Core.Branches.Data;

public interface IBranchRepository
{
    Task<BranchViewDto> GetBranch(int branchNo);
}

public class BranchRepository(
    ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context,
    IFusionCache cache,
    IOptions<BranchCacheOptions> options
    ) : IBranchRepository
{
    public Task<BranchViewDto> GetBranch(int branchNo)
    {
        // branch details rarely change, so cache per-tenant/branch with a long duration; no preload needed, populated on-demand
        var key = $"branch:{context.Tenant.TenantId}:{branchNo}";
        return cache.GetOrSetAsync(
            key,
            _ => FetchBranch(branchNo),
            new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(options.Value.CacheDurationMinutes),
                IsFailSafeEnabled = true,
                FailSafeMaxDuration = TimeSpan.FromMinutes(options.Value.FailSafeMaxDurationMinutes)
            }).AsTask();
    }

    private async Task<BranchViewDto> FetchBranch(int branchNo)
    {
        try
        {
            // get the booking from the runtime
            var inputXml = $"<ROOT><BRANCH_NUMBER>{branchNo}</BRANCH_NUMBER></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-BRINFO.RUN",
                inputXml
            );

            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching branch: " + result.Message);

            // deserialize the output xml
            var branchRecord = result.OutputXml.DeserializeXmlToTypeOf<BranchRecord>();

            // map to dto
            var branch = branchRecord.MapToDto();

            return branch;

        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get Branch {BranchNo}", branchNo);
            throw;
        }

    }
}