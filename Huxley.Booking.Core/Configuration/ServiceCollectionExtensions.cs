using FluentValidation;
using Huxley.Core;
using Huxley.Hosting.Configuration;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace Huxley.Booking.Core.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingCore(this IServiceCollection services, IConfiguration configuration)
    {
        // add options
        services.AddOptions<BookingOptions>().Bind(configuration);
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