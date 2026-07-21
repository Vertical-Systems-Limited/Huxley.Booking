namespace Huxley.Booking.Core.Agents.Caching;

public class AgentCacheOptions
{
    public int CacheDurationMinutes { get; set; } = 1440;
    public int FailSafeMaxDurationMinutes { get; set; } = 10080;
}
