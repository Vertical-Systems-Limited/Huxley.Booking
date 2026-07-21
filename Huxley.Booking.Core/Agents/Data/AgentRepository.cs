using Huxley.Booking.Core.Agents.Caching;
using Huxley.Booking.Core.Agents.Contracts;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Huxley.Booking.Core.Agents.Data;

public interface IAgentRepository
{
    Task<AgentViewDto> GetAgent(string initials);
}

public class AgentRepository(
    ILogger<AgentRepository> logger,
    HuxleyContext context,
    IHuxleyVslRuntimeApiClient client,
    IFusionCache cache,
    IOptions<AgentCacheOptions> options
    ) : IAgentRepository
{
    public async Task<AgentViewDto> GetAgent(string initials)
    {
        // -USRLIST.RUN returns every agent regardless of filter, so cache the whole list per
        // tenant once and look up locally, rather than re-fetching it on every single lookup.
        var key = $"agents:{context.Tenant.TenantId}";
        var agents = await cache.GetOrSetAsync(
            key,
            _ => FetchAllAgents(),
            new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(options.Value.CacheDurationMinutes),
                IsFailSafeEnabled = true,
                FailSafeMaxDuration = TimeSpan.FromMinutes(options.Value.FailSafeMaxDurationMinutes)
            });

        if (!agents.TryGetValue(initials, out var agent))
            throw new KeyNotFoundException($"Agent with initials {initials} not found.");

        return agent;
    }

    private async Task<IReadOnlyDictionary<string, AgentViewDto>> FetchAllAgents()
    {
        try
        {
            var inputXml = "<ROOT><FILTER>*</FILTER></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-USRLIST.RUN",
                inputXml
            );

            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching user: " + result.Message);

            // deserialize the output xml
            var agentRecords = result.OutputXml.DeserializeXmlToEnumerableOf<AgentRecord>();

            return agentRecords
                .GroupBy(a => a.TarscInitials, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().MapToDto(), StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to fetch agent list");
            throw;
        }
    }
}
