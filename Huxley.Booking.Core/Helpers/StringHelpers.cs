namespace Huxley.Booking.Core.Helpers;

public static class StringHelpers
{
    public static string BoolToString(this bool value)
    {
        return value ? "Y" : "N";
    }

    public static bool StringToBool(this string value)
    {
        return value == "Y";
    }
    
    public static TimeOnly ParseTimeOnly(this string time)
    {
        var modifiedTime = time.Replace("T", "").Replace("Z", "");
        if (TimeOnly.TryParse(modifiedTime, out var result))
        {
            return result;
        }
        return TimeOnly.MinValue;
    }
    
    
    public static DateOnly ParseDateOnly(this string date)
    {
        if (DateOnly.TryParse(date, out var result))
        {
            return result;
        }
        return DateOnly.MinValue;
    }
    
    public static DateTime ParseDateTime(this string date, string time)
    {
        if (DateTime.TryParse($"{date} {time}", out var result))
        {
            return result;
        }
        return DateTime.MinValue;
    }
}