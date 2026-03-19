// using Huxley.Booking.Core.Itineraries.Transfers.Data;
//
// namespace Huxley.Booking.Core.Itineraries.Transfers;
//
// public class Transfer
// {
//     public string TransferId { get; set; }
//     public string FileNumber { get; set; }
//     public string PayeeCompany { get; set; }
//     public string PayeeCompanyReference { get; set; }
//     public string DepartPoint { get; set; }
//     public string ArrivalPoint { get; set; }
//     public string ArrivalAddressLine1 { get; set; }
//     public string ArrivalAddressLine2 { get; set; }
//     public string ArrivalAddressLine3 { get; set; }
//     public string ArrivalAddressLine4 { get; set; }
//     public string PickupDate { get; set; }
//     public string PickupTime { get; set; }
//     public string TransportType { get; set; }
//     public string LocalAgent { get; set; }
//     public string SpecialRequests { get; set; }
//     public string Remarks { get; set; }
// }
//
// public static class TransferExtensions
// {
//     public static Transfer ToTransfer(this TransferRecord transferRecord)
//     {
//         if (transferRecord == null)
//         {
//             throw new ArgumentNullException(nameof(transferRecord));
//         }
//
//         return new Transfer
//         {
//             TransferId = transferRecord.TransferId,
//             FileNumber = transferRecord.FileNumber,
//             PayeeCompany = transferRecord.PayeeCompany,
//             PayeeCompanyReference = transferRecord.PayeeCompanyReference,
//             DepartPoint = transferRecord.DepartPoint,
//             ArrivalPoint = transferRecord.ArrivalPoint,
//             ArrivalAddressLine1 = transferRecord.ArrivalAddressLine1,
//             ArrivalAddressLine2 = transferRecord.ArrivalAddressLine2,
//             ArrivalAddressLine3 = transferRecord.ArrivalAddressLine3,
//             ArrivalAddressLine4 = transferRecord.ArrivalAddressLine4,
//             PickupDate = transferRecord.PickupDate,
//             PickupTime = transferRecord.PickupTime,
//             TransportType = transferRecord.TransportType,
//             LocalAgent = transferRecord.LocalAgent,
//             SpecialRequests = transferRecord.SpecialRequests,
//             Remarks = transferRecord.Remarks
//         };
//     }
// }