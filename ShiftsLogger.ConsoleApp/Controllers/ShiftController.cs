using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using Spectre.Console;
using System.Globalization;

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

    public static async Task CreateShift()
    {
        await ViewShifts("Add Worker");
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var shiftService = new ShiftService(new HttpClient());
        var newShiftName = userInterface.ReadString("Enter Shift (1st, 2nd, or 3rd): ");
        var newDate = userInterface.PromptForDate();
        var newStartTime = userInterface.PromptForTime("start");
        var newEndTime = userInterface.PromptForTime("end");
        while (Validation.ValidateStartTimeIsLessThanEndTime(newStartTime, newEndTime) == false)
        {
            Console.WriteLine("The end time must be later than the start time.");
            newEndTime = userInterface.PromptForTime("end");
        }

        var workerService = new WorkerService(new HttpClient());
        var shiftWorkers = await workerService.GetWorkersAsync();
        var shiftWorkersDict = shiftWorkers.ToDictionary(x => $"{x.FirstName} {x.LastName}, {x.Title}");

        var selectedShiftWorkerOption = userInterface.SelectOption("\nChoose Worker for Shift:", shiftWorkersDict.Keys);

        var newShiftWorker = shiftWorkersDict[selectedShiftWorkerOption];






        var shift = new ShiftDto { Date = newDate, ShiftName = newShiftName, StartTime = newStartTime, EndTime = newEndTime,  WorkerName = $"{newShiftWorker.FirstName} {newShiftWorker.LastName}", WorkerTitle = newShiftWorker.Title};
        var insertResult = await shiftService.InsertShiftAsync(shift);
        try
        {
            Console.Clear();
            await ViewShifts("Create Shift");
            Console.WriteLine("\nSuccessfully saved shift");
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            await ViewShifts("Create Shift");
            Console.WriteLine($"\nFailed to save shift. Request error: {e.Message}");
        }
    }
}
