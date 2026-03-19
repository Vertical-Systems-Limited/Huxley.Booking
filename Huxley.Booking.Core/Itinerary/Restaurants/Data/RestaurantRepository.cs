using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using Huxley.Itinerary.Domain.Restaurants;

namespace Huxley.Booking.Core.Itinerary.Restaurants.Data;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetRestaurantsForFileNo(string fileNo);
}

public class RestaurantRepository(ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context) : IRestaurantRepository
{
    public async Task<IEnumerable<Restaurant>> GetRestaurantsForFileNo(string fileNo)
    {
        try
        {
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-ITINRST.RUN",
                inputXml
            );
            
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching restaurants: " + result.Message);

            var restaurantRecords = result.OutputXml.DeserializeXmlToEnumerableOf<RestaurantRecord>();
            
            // Map legacy XML records to Central Domain via extension methods
            return restaurantRecords.ToDomainModels().ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get restaurants by file number {FileNo}", fileNo);
            throw;
        }
    }
}