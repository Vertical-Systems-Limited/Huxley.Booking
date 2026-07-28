using System.Diagnostics;
using Microsoft.Extensions.Http;

namespace Huxley.Booking.Performance;

public class Worker(
    IHttpClientFactory httpClientFactory,
    ILogger<Worker> logger
    )
    : BackgroundService
{
    private const string BaseUrl = "https://api-beta.verticalsystems.co.uk/hx-booking/api/";
    private const string ApiKey = "f48ad0e5-7f56-4bda-bbca-bcd73c1f0e1a";
    private const string ApiAuth = "P1kczamW3iWDhSOKQRy7hlYk3expjQAXxWYsicF8WvVzOPSSmTlwnsHzBUldxMh8";
    private static readonly TimeSpan DelayBetweenRuns = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var fileNos = new [] {
            "251003149",
            "50236879",
            "197006102",
            "257001071",
            "50236996",
            "6266141"
        };

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("X-VSL-ApiKey", ApiKey);
        client.DefaultRequestHeaders.Add("X-VSL-ApiAuth", ApiAuth);

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var fileNo in fileNos)
            {
                var url = $"{BaseUrl}bookings/GetBookingByFileNo?FileNumber={fileNo}";

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    var response = await client.GetAsync(url, stoppingToken);
                    stopwatch.Stop();

                    var machineName = response.Headers.GetValues("X-Server-Name").FirstOrDefault() ?? "Unknown";
                    
                    logger.LogInformation("FileNo {FileNo}: {machineName} {StatusCode} in {ElapsedMs}ms",
                        fileNo, machineName, (int)response.StatusCode, stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    logger.LogError(ex, "FileNo {FileNo}: request failed after {ElapsedMs}ms", fileNo, stopwatch.ElapsedMilliseconds);
                }
            }

            await Task.Delay(DelayBetweenRuns, stoppingToken);
        }
    }
}