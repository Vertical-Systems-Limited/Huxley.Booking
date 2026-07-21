using Huxley.Booking.Core.Configuration;
using Huxley.Hosting.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Huxley.Booking.Debugger;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddSingleton<ILogger>(Log.Logger);
        builder.Services.AddHuxleyBotHosting(builder.Configuration.GetSection("HostingConfiguration"));
        builder.Services.AddBookingCore(builder.Configuration);
        builder.Services.AddBookingCoreLogging("Booking.WebApi");
        builder.Services.AddHostedService<Worker>();
        var host = builder.Build();
        
        host.Run();
    }
}