namespace Huxley.Booking.Core.Bookings;

public static class TypeExtensions
{
    public static int ToInt(this object value, int fallback = 0)
    {
        if (value == null)
            return fallback;

        return int.TryParse(value.ToString(), out var result) ? result : fallback;
    }
}