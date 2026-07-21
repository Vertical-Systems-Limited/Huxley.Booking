namespace Huxley.Booking.Core.AnalysisCodes.Caching;

public enum AnalysisCodeType
{
    Primary,
    Secondary,
    Tertiary,
    SubTertiary
}

public class AnalysisCodeSnapshot
{
    public IReadOnlyDictionary<string, string> PrimaryCodes { get; init; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> SecondaryCodes { get; init; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> TertiaryCodes { get; init; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> SubTertiaryCodes { get; init; } = new Dictionary<string, string>();

    public string TryGetName(AnalysisCodeType type, string code, string tertiaryCode = null)
    {
        if (string.IsNullOrEmpty(code)) return null;

        var lookup = type switch
        {
            AnalysisCodeType.Primary => PrimaryCodes,
            AnalysisCodeType.Secondary => SecondaryCodes,
            AnalysisCodeType.Tertiary => TertiaryCodes,
            AnalysisCodeType.SubTertiary => SubTertiaryCodes,
            _ => null
        };

        if (lookup == null) return null;

        var key = type == AnalysisCodeType.SubTertiary
            ? BuildSubTertiaryKey(tertiaryCode, code)
            : code;

        return lookup.TryGetValue(key, out var name) ? name : null;
    }

    public static string BuildSubTertiaryKey(string tertiaryCode, string subTertiaryCode) => $"{tertiaryCode}|{subTertiaryCode}";
}
