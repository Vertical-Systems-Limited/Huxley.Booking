using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Bookings.Data;

public interface IBookingRepository
{
    Task<Booking> GetBookingByFileNo(string fileNo);
}

public class BookingRepository(
    ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
    ) : IBookingRepository
{
    public async Task<Booking> GetBookingByFileNo(string fileNo)
    {
        try
        {
            // get the booking from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-BOOKLTE.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching booking: " + result.Message);

            // deserialize the output xml
            var bookingRecord = result.OutputXml.DeserializeXmlToTypeOf<BookingRecord>();
            
            // map to booking and return
            return bookingRecord.ToBooking();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get booking by file number {FileNo}", fileNo);
            throw;
        }
    }
}