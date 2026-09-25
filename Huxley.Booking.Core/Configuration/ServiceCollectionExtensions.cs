using FluentValidation;
using Huxley.Booking.Core.Agents.Caching;
using Huxley.Booking.Core.AnalysisCodes.Caching;
using Huxley.Booking.Core.Branches.Caching;
using Huxley.Core;
using Huxley.Hosting.Configuration;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Huxley.Booking.Core.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingCore(this IServiceCollection services, IConfiguration configuration)
    {
        // add options
        services.AddOptions<BookingOptions>().Bind(configuration.GetSection(nameof(BookingOptions)));
        // explicitly retrieve options
        var bookingOptions = configuration.GetSection(nameof(BookingOptions)).Get<BookingOptions>();
        
        // add mediatr
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<BookingCoreRoot>());
        // add runtime client
        services.AddHuxleyVslRuntimeApiClient(cfg =>
        {
            cfg.Url = bookingOptions.RuntimeApiUrl;
        });
        
        // add huxley hosting
        services.AddHuxleyWebHosting(configuration.GetSection(nameof(HostingConfiguration)));
        
        // add booking api tenant settings provider
        services.AddTransient<IBookingApiTenantSettingsProvider, BookingApiTenantSettingsProvider>();

        // add shared cache infrastructure (L1 in-memory, optional L2 Redis + backplane via FusionCache)
        // used by the analysis code, branch, and agent caches below
        services.AddOptions<DistributedCacheOptions>().Bind(configuration.GetSection(nameof(DistributedCacheOptions)));
        var distributedCacheOptions = configuration.GetSection(nameof(DistributedCacheOptions)).Get<DistributedCacheOptions>() ?? new DistributedCacheOptions();
        var fusionCacheBuilder = services.AddFusionCache()
            .WithSerializer(new FusionCacheSystemTextJsonSerializer());
        if (distributedCacheOptions.RedisEnabled && !string.IsNullOrWhiteSpace(distributedCacheOptions.RedisConnectionString))
        {
            fusionCacheBuilder
                .WithDistributedCache(new RedisCache(new RedisCacheOptions { Configuration = distributedCacheOptions.RedisConnectionString }))
                .WithBackplane(new RedisBackplane(new RedisBackplaneOptions { Configuration = distributedCacheOptions.RedisConnectionString }));
        }

        // add analysis code name cache (per-tenant)
        services.AddOptions<AnalysisCodeCacheOptions>().Bind(configuration.GetSection(nameof(AnalysisCodeCacheOptions)));
        services.AddSingleton<IAnalysisCodeNameCache, AnalysisCodeNameCache>();
        services.AddHostedService<AnalysisCodeCachePreloadService>();

        // add branch cache options (reuses the same FusionCache instance registered above; no preload, on-demand only)
        services.AddOptions<BranchCacheOptions>().Bind(configuration.GetSection(nameof(BranchCacheOptions)));

        // add agent cache options (reuses the same FusionCache instance registered above; no preload, on-demand only)
        services.AddOptions<AgentCacheOptions>().Bind(configuration.GetSection(nameof(AgentCacheOptions)));

        // add validators
        services.AddValidatorsFromAssemblyContaining<BookingCoreRoot>();
        // add transient services by convention
        services.AddTransientFromAssemblyContaining<BookingCoreRoot>();

        return services;
    }
    public static IServiceCollection AddBookingCoreLogging(this IServiceCollection services, string runningContext)
    {
        services.AddSerilog(cfg =>
        {
            cfg.MinimumLevel.Information()
                .Enrich.WithProperty("Machine", Environment.MachineName)
                .WriteTo.Console(outputTemplate: "{Timestamp:dd/MM/yyyy HH:mm:ss} {Level:u4}: {Message:lj}{NewLine}",
                    restrictedToMinimumLevel: LogEventLevel.Information);
        
            var options = services.BuildServiceProvider().GetRequiredService<IOptions<BookingOptions>>().Value;
            if (!string.IsNullOrEmpty(options.LokiUrl))
            {
                cfg.WriteTo.GrafanaLoki(options.LokiUrl, [
                        new LokiLabel { Key = "app", Value = "Booking" },
                        new LokiLabel { Key = "context", Value = runningContext }
                    ], 
                    restrictedToMinimumLevel: LogEventLevel.Information);
            }
        });
     
        return services;
    }
}

// root class for nothing more than assembly reference when registering services by convention
public abstract class BookingCoreRoot { }