using Huxley.Booking.Core.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Huxley.Booking.Core.AnalysisCodes.Caching;

public class AnalysisCodeCachePreloadService(
    IBookingApiTenantSettingsProvider settingsProvider,
    IAnalysisCodeNameCache cache,
    IOptions<AnalysisCodeCacheOptions> options,
    ILogger<AnalysisCodeCachePreloadService> logger
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<BookingApiTenantSettings> tenantsToPreload;
        try
        {
            var allSettings = await settingsProvider.GetSettingsForAllTenants();
            tenantsToPreload = allSettings.Where(s => s.PreLoadAnalysisCodes).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to load tenant settings for analysis code cache preload; skipping preload");
            return;
        }

        logger.LogInformation("Preloading analysis code cache for {Count} tenant(s)", tenantsToPreload.Count);

        await Parallel.ForEachAsync(tenantsToPreload,
            new ParallelOptions { MaxDegreeOfParallelism = options.Value.MaxTenantPreloadConcurrency, CancellationToken = stoppingToken },
            async (tenantSettings, _) =>
            {
                try
                {
                    await cache.GetSnapshot(tenantSettings.TenantId);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to preload analysis code cache for tenant {TenantId}", tenantSettings.TenantId);
                }
            });
    }
}
