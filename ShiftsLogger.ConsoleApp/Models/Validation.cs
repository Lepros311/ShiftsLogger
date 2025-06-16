using System.Globalization;

namespace ShiftsLogger.ConsoleApp.Models;

public class Validation
{
    public static DateOnly? ValidateDate(string dateInput)
    {
        string format = "MM/dd/yyyy";
        if (!DateOnly.TryParseExact(dateInput, format, out DateOnly date))
        {
            return null;
        }
        else
        {
            return date;
        }
    }

    public static TimeOnly? ValidateTime(string timeInput)
    {
        if (!TimeOnly.TryParseExact(timeInput, "h:mm tt", out TimeOnly time))
        {
            return null;
        }
        else
        {
            return time;
        }
    }

    public static bool ValidateStartTimeIsLessThanEndTime(TimeOnly? start, TimeOnly? end)
    {
        if (start < end)
            return true;
        else
            return false;
    }
}
