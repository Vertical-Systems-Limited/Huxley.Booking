using Huxley.Booking.Core.Bookings.Contracts;
using Huxley.Booking.Core.Bookings.Data;
using Huxley.Booking.Core.Passengers.Contracts;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Passengers.Data;

public interface IPassengerRepository
{
    Task<bool> InsertPassengers(CreateBookingDto booking, string fileNumber);
    Task<IEnumerable<PassengerViewDto>>  GetPassengersByFileNo(string fileNo);
}

public class PassengerRepository(
    ILogger<PassengerRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
    ) : IPassengerRepository
{
    public async Task<bool> InsertPassengers(CreateBookingDto booking, string fileNumber)
    {
        var passengerRecords = booking.Passengers.Select(p => PassengerRecord.MapFromDto(p, fileNumber)).ToList();
        var xml = passengerRecords.ToDataSet(
            dataSetName: "dsTarscBookingPassengerList",
            tableName: "dtPassenger"
            ).GetXml();
        var result = await client.ExecuteRunFile(
            context.Tenant.TarscServerAddress,
            context.Tenant.TarscServerPort,
            "TARSCWS_V6\\-APASLST.RUN",
            xml
        );
        
        // check for errors
        if(result.ErrorOccurred)
            throw new Exception("Error occurred while inserting passengers: " + result.Message);

        var sync = await client.ExecuteRunFile(
            context.Tenant.TarscServerAddress, 
            context.Tenant.TarscServerPort,
            "TARSCWS_V6\\-SYNCPAS.RUN", 
            xml);
        if (sync.ErrorOccurred)
            throw new Exception("Error occurred while syncing passengers: " + sync.Message);
        
        return await Task.FromResult(true);
    }
    public async Task<IEnumerable<PassengerViewDto>>  GetPassengersByFileNo(string fileNo)
    {
        try
        {
            // get the passengers from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-LISTPAS.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching passengers: " + result.Message);

            // deserialize the output xml
            var passengerRecords = result.OutputXml.DeserializeXmlToEnumerableOf<PassengerRecord>();
            
            // map to passengers and return
            return passengerRecords.Select(r => r.ToPassenger()).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get passengers by file number {FileNo}", fileNo);
            throw;
        }
    }  
}