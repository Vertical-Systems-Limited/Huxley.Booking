using Huxley.Itinerary.Domain.Flights;

namespace Huxley.Booking.Core.Itinerary.Flights.Data;

public class FlightRecord
{
    public string FlightId { get; set; }
    public string FileNumber { get; set; }
    public string PayeeCompanyReference { get; set; }
    public string PayeeCompany { get; set; }
    public string DepartAirport { get; set; }
    public string DepartTerminal { get; set; }
    public string ArrivalAirport { get; set; }
    public string ArrivalTerminal { get; set; }
    public string Class { get; set; }
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string DepartDate { get; set; }
    public string DepartTime { get; set; }
    public string CheckInTime { get; set; }
    public string ArrivalDate { get; set; }
    public string ArrivalTime { get; set; }
    public string Duration { get; set; }
    public string SeatAllocation { get; set; }
    public string SpecialRequests { get; set; }
    public string Remarks { get; set; }
    public string BaggageAllowance { get; set; }
}

public static class FlightRecordExtensions
{
    public static FlightSegment ToFlightSegment(this FlightRecord record)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));

        return FlightSegment.Create(
            departurePointCode: record.DepartAirport ?? "",
            departurePointName: record.DepartAirport ?? "",
            departureTerminal: record.DepartTerminal ?? "",
            departureDateTime: ParseDateTime(record.DepartDate, record.DepartTime),
            arrivalPointCode: record.ArrivalAirport ?? "",
            arrivalPointName: record.ArrivalAirport ?? "",
            arrivalTerminal: record.ArrivalTerminal ?? "",
            arrivalDateTime: ParseDateTime(record.ArrivalDate, record.ArrivalTime),
            carrier: record.Airline ?? "",
            cabinClass: record.Class ?? "",
            flightNumber: record.FlightNumber ?? "",
            baggage: record.BaggageAllowance ?? "",
            seat: record.SeatAllocation ?? "",
            meals: "",
            extras: record.Remarks ?? "",
            specialRequests: record.SpecialRequests ?? ""
        );
    }

    public static IEnumerable<Flight> ToFlights(this IEnumerable<FlightRecord> records)
    {
        // Group records by FileNumber to handle multi-segment flights
        var flightGroups = records
            .GroupBy(r => r.FileNumber)
            .Select(group => group.OrderBy(r => ParseDateTime(r.DepartDate, r.DepartTime)).ToList());

        var flights = new List<Flight>();
        
        foreach (var group in flightGroups)
        {
            var segments = group.Select(r => r.ToFlightSegment()).ToList();
            flights.Add(Flight.CreateFlight(segments));
        }

        return flights;
    }

    private static DateTime ParseDateTime(string date, string time)
    {
        var dateTimeString = $"{date} {time}".Trim();
        if (DateTime.TryParse(dateTimeString, out var result))
            return result;
        
        // Try just the date if time parsing fails
        if (DateTime.TryParse(date, out var dateOnly))
            return dateOnly;
            
        return DateTime.MinValue;
    }
}

// public static class FlightRecordExtensions
// {
//     public static Flight ToFlight(this FlightRecord flightRecord)
//     {
//         if (flightRecord == null)
//         {
//             throw new ArgumentNullException(nameof(flightRecord));
//         }
//
//         return new Flight
//         {
//             FlightId = flightRecord.FlightId,
//             FileNumber = flightRecord.FileNumber,
//             PayeeCompanyReference = flightRecord.PayeeCompanyReference,
//             PayeeCompany = flightRecord.PayeeCompany,
//             DepartAirport = flightRecord.DepartAirport,
//             DepartTerminal = flightRecord.DepartTerminal,
//             ArrivalAirport = flightRecord.ArrivalAirport,
//             ArrivalTerminal = flightRecord.ArrivalTerminal,
//             Class = flightRecord.Class,
//             FlightNumber = flightRecord.FlightNumber,
//             Airline = flightRecord.Airline,
//             DepartDate = flightRecord.DepartDate,
//             DepartTime = flightRecord.DepartTime,
//             CheckInTime = flightRecord.CheckInTime,
//             ArrivalDate = flightRecord.ArrivalDate,
//             ArrivalTime = flightRecord.ArrivalTime,
//             Duration = flightRecord.Duration,
//             SeatAllocation = flightRecord.SeatAllocation,
//             SpecialRequests = flightRecord.SpecialRequests,
//             Remarks = flightRecord.Remarks,
//             BaggageAllowance = flightRecord.BaggageAllowance
//         };
//     }
// }