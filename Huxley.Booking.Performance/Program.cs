using Serilog;
using Serilog.Events;

namespace Huxley.Booking.Performance;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSerilog(cfg =>
        {
            cfg.MinimumLevel.Information()
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Extensions.Http", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: "{Timestamp:dd/MM/yyyy HH:mm:ss} {Level:u4}: {Message:lj}{NewLine}",
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File("logs/performance-.log", rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:dd/MM/yyyy HH:mm:ss} {Level:u4}: {Message:lj}{NewLine}",
                    restrictedToMinimumLevel: LogEventLevel.Information);
        });

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(30)
            });
        });
        builder.Services.AddHttpClient();
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}