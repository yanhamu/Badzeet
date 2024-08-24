namespace System;

public static class DateOnlyExtensions
{
    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    }

    public static DateTime? ToDateTime(this DateOnly? dateOnly)
    {
        if (dateOnly == null)
            return null;

        var dateValue = dateOnly.Value;
        return new DateTime(dateValue.Year, dateValue.Month, dateValue.Day);
    }
}