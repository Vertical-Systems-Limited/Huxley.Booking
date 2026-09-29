namespace Huxley.Booking.Core.Configuration;

public class DistributedCacheOptions
{
    public bool RedisEnabled { get; set; } = true;
    public string RedisConnectionString { get; set; }
}
