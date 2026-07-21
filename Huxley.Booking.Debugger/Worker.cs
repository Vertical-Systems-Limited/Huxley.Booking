using Huxley.Booking.Core.Configuration;
using Huxley.Hosting.Core;
using Huxley.Hosting.Core.Data;
using Microsoft.Extensions.Options;

namespace Huxley.Booking.Debugger;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory scopeFactory,
    ITenantReader tenantReader,
    IOptions<BookingOptions> bookingOptions
        )
    : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tenantId = new Guid("802c9d49-abb3-402c-aaf4-0958578b370a");
        var apiId = bookingOptions.Value.ApiId;
        // create a scope on the DI container so we can get the HuxleyContext
        using var scope = scopeFactory.CreateScope();

        // setup the huxley context
        var context = scope.ServiceProvider.GetRequiredService<HuxleyContext>();
        context.Tenant = await tenantReader.Read(tenantId);

        var provider = scope.ServiceProvider.GetRequiredService<IBookingApiTenantSettingsProvider>();

        var settings = await provider.GetSettingsForAllTenants();
    }
}