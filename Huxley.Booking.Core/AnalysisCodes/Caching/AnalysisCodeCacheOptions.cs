namespace Huxley.Booking.Core.AnalysisCodes.Caching;

public class AnalysisCodeCacheOptions
{
    public int CacheDurationMinutes { get; set; } = 360;
    public int FailSafeMaxDurationMinutes { get; set; } = 1440;
    public int MaxSubTertiaryFetchConcurrency { get; set; } = 8;
    public int MaxTenantPreloadConcurrency { get; set; } = 4;
}
