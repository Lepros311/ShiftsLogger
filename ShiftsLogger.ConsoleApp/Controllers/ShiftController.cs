using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Controllers;

internal class ShiftController
{
    public static async Task ViewShifts(string heading, int? shiftId = null)
    {
        var shiftService = new ShiftService(new HttpClient());
        List<ShiftDto> viewShifts;
        if (shiftId.HasValue)
        {
            var worker = await shiftService.GetShiftAsync(shiftId.Value);
            viewShifts = worker is not null ? new List<ShiftDto> { worker } : new List<ShiftDto>();

        }
        else
        {
            viewShifts = await shiftService.GetShiftsAsync();
        }

        Display.PrintShiftsTable(viewShifts, heading);
    }
}
