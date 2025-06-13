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

        var newShiftWorker = userInterface.SelectWorker("\nChoose Worker for Shift:", shiftWorkers);

        var shift = new ShiftDto { Date = newDate, ShiftName = newShiftName, StartTime = newStartTime, EndTime = newEndTime, WorkerId = newShiftWorker.WorkerId, Worker = newShiftWorker};
        try
        {
            var insertResult = await shiftService.InsertShiftAsync(shift);

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

    public static async Task EditShift()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var shiftService = new ShiftService(new HttpClient());
        var editShifts = await shiftService.GetShiftsAsync();
        var editShiftsDict = editShifts.ToDictionary(x => $"{x.Date} {x.ShiftName} {x.StartTime} {x.EndTime} {x.Duration} - {x.Worker.ToString()}");

        var rule = new Rule("[green]Edit Shift[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var editShift = userInterface.SelectShift("\nChoose Shift to Edit:", editShifts);

        await ViewShifts("Edit Shift", editShift.ShiftId);

        Console.WriteLine();
        editShift.Date = AnsiConsole.Confirm("Update shift's date?", false) ? AnsiConsole.Ask<DateOnly>("Worker's new first name:") : editShift.Date;
        Console.WriteLine();
        editShift.ShiftName = AnsiConsole.Confirm("Update shift name?", false) ? AnsiConsole.Ask<string>("Worker's new last name:") : editShift.ShiftName;
        Console.WriteLine();
        editShift.StartTime = AnsiConsole.Confirm("Update shift's start time?", false) ? AnsiConsole.Ask<TimeOnly>("Worker's new title:") : editShift.StartTime;
        Console.WriteLine();
        editShift.EndTime = AnsiConsole.Confirm("Update shift's end time?", false) ? AnsiConsole.Ask<TimeOnly>("Worker's new title:") : editShift.EndTime;
        Console.WriteLine();
        var workerService = new WorkerService(new HttpClient());
        var shiftWorkers = await workerService.GetWorkersAsync();
        editShift.Worker = AnsiConsole.Confirm("Update shift's worker?", false) ? userInterface.SelectWorker("\nChoose Worker for Shift:", shiftWorkers) : editShift.Worker;

        try
        {
            var editResult = await shiftService.UpdateShift(editShift);

            Console.Clear();
            await ViewShifts("Edit Worker");
            Console.WriteLine("\nSuccessfully edited shift");
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            await ViewShifts("Edit Worker");
            Console.WriteLine($"\nFailed to edit shift. Request error: {e. Message}");
        }
    }
}
