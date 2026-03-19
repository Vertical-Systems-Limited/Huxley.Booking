// using Huxley.Booking.Core.Itinerary.CarHires.Data;
//
// namespace Huxley.Booking.Core.Itinerary.CarHires;
//
// public class CarHire
// {
//     public string CarHireId { get; set; }
//     public string FileNumber { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string PayeeCompany { get; set; }
//     public string DepartPoint { get; set; }
//     public string PickupDate { get; set; }
//     public string PickupTime { get; set; }
//     public string ArrivalPoint { get; set; }
//     public string DropOffDate { get; set; }
//     public string DropOffTime { get; set; }
//     public string CarHireCompany { get; set; }
//     public string CarType { get; set; }
//     public string Group { get; set; }
//     public string Plan { get; set; }
//     public string Driver { get; set; }
//     public string CDW { get; set; }
//     public string TheftProtection { get; set; }
//     public string TaxOtherCharges { get; set; }
//     public string SpecialRequests { get; set; }
//     public string Remarks { get; set; }
// }
//
// public static class CarHireExtensions
// {
//     public static CarHire ToCarHire(this CarHireRecord carHireRecord)
//     {
//         if (carHireRecord == null)
//         {
//             throw new ArgumentNullException(nameof(carHireRecord));
//         }
//
//         return new CarHire
//         {
//             CarHireId = carHireRecord.CarHireId,
//             FileNumber = carHireRecord.FileNumber,
//             PayeeCompanyReference = carHireRecord.PayeeCompanyReference,
//             PayeeCompany = carHireRecord.PayeeCompany,
//             DepartPoint = carHireRecord.DepartPoint,
//             PickupDate = carHireRecord.PickupDate,
//             PickupTime = carHireRecord.PickupTime,
//             ArrivalPoint = carHireRecord.ArrivalPoint,
//             DropOffDate = carHireRecord.DropOffDate,
//             DropOffTime = carHireRecord.DropOffTime,
//             CarHireCompany = carHireRecord.CarHireCompany,
//             CarType = carHireRecord.CarType,
//             Group = carHireRecord.Group,
//             Plan = carHireRecord.Plan,
//             Driver = carHireRecord.Driver,
//             CDW = carHireRecord.CDW,
//             TheftProtection = carHireRecord.TheftProtection,
//             TaxOtherCharges = carHireRecord.TaxOtherCharges,
//             SpecialRequests = carHireRecord.SpecialRequests,
//             Remarks = carHireRecord.Remarks
//         };
//     }
// }