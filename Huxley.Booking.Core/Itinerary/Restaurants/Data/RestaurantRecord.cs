using Huxley.Booking.Core.Helpers;
using Huxley.Itinerary.Domain.Restaurants;

namespace Huxley.Booking.Core.Itinerary.Restaurants.Data;

public class RestaurantRecord
{
    public string RestaurantId { get; set; }
    public string FileNumber { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantReference { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string AddressLine5 { get; set; }
    public string Phone { get; set; }
    public string TableSize { get; set; }
    public string BookedByName { get; set; }
    public string SpecialRequests { get; set; }
    public string DiningDate { get; set; }
    public string DiningTime { get; set; }
}

public static class RestaurantRecordExtensions
{
    public static IEnumerable<Restaurant> ToDomainModels(this IEnumerable<RestaurantRecord> records)
    {
        return records.Select(r => r.ToRestaurant());
    }

    public static Restaurant ToRestaurant(this RestaurantRecord record)
    {
        var domain = Restaurant.Create(
            record.RestaurantName,
            record.DiningDate.ParseDateTime(record.DiningTime),
            record.TableSize
        );

        domain.AddressLine1 = record.AddressLine1;
        domain.AddressLine2 = record.AddressLine2;
        domain.AddressLine3 = record.AddressLine3;
        domain.AddressLine4 = record.AddressLine4;
        domain.AddressLine5 = record.AddressLine5;
        domain.Phone = record.Phone;
        domain.BookedByName = record.BookedByName;
        domain.SpecialRequests = record.SpecialRequests;
        domain.FileNumber = record.FileNumber;
        domain.Contact = record.RestaurantReference;

        return domain;
    }
}