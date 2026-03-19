// using Huxley.Booking.Core.Itinerary.Restaurants.Data;
//
// namespace Huxley.Booking.Core.Itinerary.Restaurants;
//
// public class Restaurant
// {
//     public string RestaurantId { get; set; }
//     public string FileNumber { get; set; }
//     public string RestaurantName { get; set; }
//     public string RestaurantReference { get; set; }
//     public string AddressLine1 { get; set; }
//     public string AddressLine2 { get; set; }
//     public string AddressLine3 { get; set; }
//     public string AddressLine4 { get; set; }
//     public string AddressLine5 { get; set; }
//     public string Phone { get; set; }
//     public string TableSize { get; set; }
//     public string BookedByName { get; set; }
//     public string SpecialRequests { get; set; }
//     public string DiningDate { get; set; }
//     public string DiningTime { get; set; }
// }
//
// public static class RestaurantExtensions
// {
//     public static Restaurant ToRestaurant(this RestaurantRecord restaurantRecord)
//     {
//         if (restaurantRecord == null)
//         {
//             throw new ArgumentNullException(nameof(restaurantRecord));
//         }
//
//         return new Restaurant
//         {
//             RestaurantId = restaurantRecord.RestaurantId,
//             FileNumber = restaurantRecord.FileNumber,
//             RestaurantName = restaurantRecord.RestaurantName,
//             RestaurantReference = restaurantRecord.RestaurantReference,
//             AddressLine1 = restaurantRecord.AddressLine1,
//             AddressLine2 = restaurantRecord.AddressLine2,
//             AddressLine3 = restaurantRecord.AddressLine3,
//             AddressLine4 = restaurantRecord.AddressLine4,
//             AddressLine5 = restaurantRecord.AddressLine5,
//             Phone = restaurantRecord.Phone,
//             TableSize = restaurantRecord.TableSize,
//             BookedByName = restaurantRecord.BookedByName,
//             SpecialRequests = restaurantRecord.SpecialRequests,
//             DiningDate = restaurantRecord.DiningDate,
//             DiningTime = restaurantRecord.DiningTime
//         };
//     }
// }