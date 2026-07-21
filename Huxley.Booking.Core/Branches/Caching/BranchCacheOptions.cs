namespace Huxley.Booking.Core.Branches.Caching;

public class BranchCacheOptions
{
    public int CacheDurationMinutes { get; set; } = 1440;
    public int FailSafeMaxDurationMinutes { get; set; } = 10080;
}
