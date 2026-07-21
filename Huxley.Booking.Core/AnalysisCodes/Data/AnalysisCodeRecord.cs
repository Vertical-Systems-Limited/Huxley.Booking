using Huxley.Booking.Core.AnalysisCodes.Contracts;

namespace Huxley.Booking.Core.AnalysisCodes.Data;

public class AnalysisCodeRecord
{
    public string Code { get; set; }
    public string Description { get; set; }
    
    public static AnalysisCodeViewDto MapToDto(AnalysisCodeRecord record) => new()
    {
        Code = record.Code,
        Name = record.Description
    };
}