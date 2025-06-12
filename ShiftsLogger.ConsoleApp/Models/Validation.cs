using System.Globalization;

namespace ShiftsLogger.ConsoleApp.Models;

public class Validation
{
    //public static bool validateTime(string time)
    //{
    //    if (time.Split(":")[0].Length == 1)
    //    {
    //        time = "0" + time;
    //    }
    //    bool res = DateTime.TryParseExact(time, "HH:mm", null, DateTimeStyles.None, out DateTime UserTime);
    //    return res;
    //}
    //public static bool validateDate(string date)
    //{
    //    bool res = DateTime.TryParseExact(date, "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime UserTime);
    //    return res;
    //}

    public static DateOnly? ValidateDate(string dateInput)
    {
        string format = "MM/dd/yyyy";
        CultureInfo provider = CultureInfo.InvariantCulture;
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
        CultureInfo provider = CultureInfo.InvariantCulture;
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
