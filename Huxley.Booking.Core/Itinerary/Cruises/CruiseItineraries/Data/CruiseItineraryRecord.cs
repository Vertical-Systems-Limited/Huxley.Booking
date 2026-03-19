namespace Huxley.Booking.Core.Itinerary.Cruises.CruiseItineraries.Data;

public class CruiseItineraryRecord
{
    public string CruiseId{ get; set; }
    public string FileNumber{ get; set; }
    public string ShipStatus{ get; set; }
    public string ArrivalPort{ get; set; }
    public string ArrivalDate{ get; set; }
    public string ArrivalTime{ get; set; }
    public string ArrivalDescription{ get; set; }
    public string DepartDate{ get; set; }
    public string DepartTime{ get; set; }
    public string DepartDescription{ get; set; }
    public string CruisingDate{ get; set; }
    public string CruisingTime{ get; set; }
}