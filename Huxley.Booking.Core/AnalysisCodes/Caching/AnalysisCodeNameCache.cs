using System.Collections.Concurrent;
using Huxley.Booking.Core.AnalysisCodes.Contracts;
using Huxley.Booking.Core.AnalysisCodes.Data;
using Huxley.Hosting.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Huxley.Booking.Core.AnalysisCodes.Caching;

public interface IAnalysisCodeNameCache
{
    Task<AnalysisCodeSnapshot> GetSnapshot(Guid tenantId);
}

public class AnalysisCodeNameCache(
    IFusionCache cache,
    IServiceScopeFactory scopeFactory,
    IOptions<AnalysisCodeCacheOptions> options,
    ILogger<AnalysisCodeNameCache> logger
    ) : IAnalysisCodeNameCache
{
    public Task<AnalysisCodeSnapshot> GetSnapshot(Guid tenantId)
    {
        return cache.GetOrSetAsync(
            BuildCacheKey(tenantId),
            _ => BuildSnapshot(tenantId),
            new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(options.Value.CacheDurationMinutes),
                IsFailSafeEnabled = true,
                FailSafeMaxDuration = TimeSpan.FromMinutes(options.Value.FailSafeMaxDurationMinutes),
                EagerRefreshThreshold = 0.8f
            }).AsTask();
    }

    public static string BuildCacheKey(Guid tenantId) => $"analysis-codes:{tenantId}";

    private async Task<AnalysisCodeSnapshot> BuildSnapshot(Guid tenantId)
    {
        using var scope = scopeFactory.CreateScope();
        var contextWriter = scope.ServiceProvider.GetRequiredService<IHuxleyContextWriter>();
        var resolved = await contextWriter.SetContextFromTenantId(tenantId);
        if (!resolved)
            throw new InvalidOperationException($"Unable to resolve tenant {tenantId} while building analysis code cache");

        var repository = scope.ServiceProvider.GetRequiredService<IAnalysisCodeRepository>();

        var primaryTask = repository.GetPrimaryCodes();
        var secondaryTask = repository.GetSecondaryCodes();
        var tertiaryCodes = (await repository.GetTertiaryCodes()).ToList();

        var primaryCodes = await primaryTask;
        var secondaryCodes = await secondaryTask;

        var subTertiaryCodes = new ConcurrentDictionary<string, string>();
        await Parallel.ForEachAsync(tertiaryCodes,
            new ParallelOptions { MaxDegreeOfParallelism = options.Value.MaxSubTertiaryFetchConcurrency },
            async (tertiaryCode, _) =>
            {
                var subCodes = await repository.GetSubTertiaryCodes(tertiaryCode.Code);
                foreach (var subCode in subCodes)
                {
                    subTertiaryCodes[AnalysisCodeSnapshot.BuildSubTertiaryKey(tertiaryCode.Code, subCode.Code)] = subCode.Name;
                }
            });

        logger.LogInformation("Built analysis code snapshot for tenant {TenantId}: {PrimaryCount} primary, {SecondaryCount} secondary, {TertiaryCount} tertiary, {SubTertiaryCount} sub-tertiary codes",
            tenantId, primaryCodes.Count(), secondaryCodes.Count(), tertiaryCodes.Count, subTertiaryCodes.Count);

        return new AnalysisCodeSnapshot
        {
            PrimaryCodes = DistinctByCode(primaryCodes),
            SecondaryCodes = DistinctByCode(secondaryCodes),
            TertiaryCodes = DistinctByCode(tertiaryCodes),
            SubTertiaryCodes = subTertiaryCodes
        };
    }

    private static IReadOnlyDictionary<string, string> DistinctByCode(IEnumerable<AnalysisCodeViewDto> codes) =>
        codes.GroupBy(c => c.Code).ToDictionary(g => g.Key, g => g.First().Name);
}
