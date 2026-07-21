using Huxley.Booking.Core.AnalysisCodes.Contracts;
using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.AnalysisCodes.Data;

public interface IAnalysisCodeRepository
{
    Task<IEnumerable<AnalysisCodeViewDto>> GetPrimaryCodes();
    Task<IEnumerable<AnalysisCodeViewDto>> GetSecondaryCodes();
    Task<IEnumerable<AnalysisCodeViewDto>> GetTertiaryCodes();
    Task<IEnumerable<AnalysisCodeViewDto>> GetSubTertiaryCodes(string tertiaryCode);
}

public class AnalysisCodeRepository(
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
    ) : IAnalysisCodeRepository
{
    public async Task<IEnumerable<AnalysisCodeViewDto>> GetPrimaryCodes()
    {
        return await GetListInternal(1);
    }
    public async Task<IEnumerable<AnalysisCodeViewDto>> GetSecondaryCodes()
    {
        return await GetListInternal(2);
    }
    public async Task<IEnumerable<AnalysisCodeViewDto>> GetTertiaryCodes()
    {
        return await GetListInternal(3);
    }
    private async Task<IEnumerable<AnalysisCodeViewDto>> GetListInternal(int level)
    {
        var xml = $"<ROOT><LEVEL>{level}</LEVEL></ROOT>";
        var result = await client.ExecuteRunFile(
            context.Tenant.TarscServerAddress,
            context.Tenant.TarscServerPort,
            "TARSCWS_V6\\-ANALLST.RUN",
            xml
        );
        
        // check for errors
        if(result.ErrorOccurred)
            throw new Exception($"Error Occured get analysis codes level {level}: {result.Message}");

        // unpack and map the output to DTOs
        var output = result.OutputXml
            .DeserializeXmlToEnumerableOf<AnalysisCodeRecord>()
            .Select(AnalysisCodeRecord.MapToDto);
        
        return output;
    }

    public async Task<IEnumerable<AnalysisCodeViewDto>> GetSubTertiaryCodes(string tertiaryCode)
    {
        var xml = $"<ROOT><ANALYSIS_CODE>{tertiaryCode}</ANALYSIS_CODE></ROOT>";
        var result = await client.ExecuteRunFile(
            context.Tenant.TarscServerAddress,
            context.Tenant.TarscServerPort,
            "TARSCWS_V6\\-SUBANAL.RUN",
            xml
        );
        
        // check for errors
        if(result.ErrorOccurred)
            throw new Exception($"Error occurred while getting sub-tertiary codes: {result.Message}");

        // unpack and map the output to DTOs
        var output = result.OutputXml
            .DeserializeXmlToEnumerableOf<AnalysisCodeRecord>()
            .Select(AnalysisCodeRecord.MapToDto);
        
        return output;
    }
    
}