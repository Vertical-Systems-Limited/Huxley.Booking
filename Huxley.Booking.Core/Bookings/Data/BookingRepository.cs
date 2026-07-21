using System.Data;
using Huxley.Booking.Core.Agents.Data;
using Huxley.Booking.Core.AnalysisCodes.Caching;
using Huxley.Booking.Core.Bookings.Contracts;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Hosting.Core.Data;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;
using IBranchRepository = Huxley.Booking.Core.Branches.Data.IBranchRepository;

namespace Huxley.Booking.Core.Bookings.Data;

public interface IBookingRepository
{
    Task<string> InsertBooking(CreateBookingDto booking);
    Task<BookingViewDto> GetBookingByFileNo(string fileNo);
    Task<bool> ConfirmBooking(string fileNumber);
}

public class BookingRepository(
    ILogger<BookingRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context,
    IAnalysisCodeNameCache analysisCodeNameCache,
    IBranchRepository branchRepository,
    IAgentRepository agentRepository
    ) : IBookingRepository
{
    public async Task<string> InsertBooking(CreateBookingDto booking)
    {
        var record = BookingRecord.MapFromDto(booking);
        var xml = record.ToDataSet(dataSetName:"dsTarscBooking", tableName:"dtBooking").GetXml();
        var result = await client.ExecuteRunFile(
            context.Tenant.TarscServerAddress,
            context.Tenant.TarscServerPort,
            "TARSCWS_V6\\-CREATEF.RUN",
            xml
        );
        
        // check for errors
        if(result.ErrorOccurred)
            throw new Exception("Error occurred while inserting booking: " + result.Message);

        // unpack the new File Number from the output xml
        var createBookingOutput = result.OutputXml.DeserializeXmlToTypeOf<CreateBookingOutput>();
        
        // return the new File Number
        return createBookingOutput.FileNumber;
    }
    public async Task<BookingViewDto> GetBookingByFileNo(string fileNo)
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
            var bookingRecord = result.OutputXml.DeserializeXmlToTypeOf<BookingRecordView>();

            // map to booking
            var booking = bookingRecord.ToBooking();
            
            // resolve agent name from the agent repository
            var agent = await agentRepository.GetAgent(booking.TarscInitials);
            booking.AgentName = agent.Name;
            
            // get the branch name from the branch repository
            var branch = await branchRepository.GetBranch(booking.BranchNumber);
            booking.BranchName = branch.BranchName;

            // resolve analysis code names from the per-tenant cache (falls back to null names on failure)
            try
            {
                var analysisCodes = await analysisCodeNameCache.GetSnapshot(context.Tenant.TenantId);
                booking.PrimaryCodeName = analysisCodes.TryGetName(AnalysisCodeType.Primary, booking.PrimaryCode);
                booking.SecondaryCodeName = analysisCodes.TryGetName(AnalysisCodeType.Secondary, booking.SecondaryCode);
                booking.TertiaryCodeName = analysisCodes.TryGetName(AnalysisCodeType.Tertiary, booking.TertiaryCode);
                booking.SubTertiaryCodeName = analysisCodes.TryGetName(AnalysisCodeType.SubTertiary, booking.SubTertiaryCode, booking.TertiaryCode);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to resolve analysis code names for file number {FileNo}", fileNo);
            }

            return booking;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get booking by file number {FileNo}", fileNo);
            throw;
        }
    }

    public async Task<bool> ConfirmBooking(string fileNumber)
    {
        try
        {
            // confirm the booking in the runtime
            var inputXml = $"<ROOT><FILENO>{fileNumber}</FILENO></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-AUTOCONF.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while confirming booking: " + result.Message);

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to confirm booking by file number {FileNo}", fileNumber);
            throw;
        }
    }
}

internal class CreateBookingOutput
{
    public string FileNumber { get; set; }
}