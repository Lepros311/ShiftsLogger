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

        var rule = new Rule("[green]Edit Shift[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var editShift = userInterface.SelectShift("\nChoose Shift to Edit:", editShifts);

        await ViewShifts("Edit Shift", editShift.ShiftId);

        Console.WriteLine();
        editShift.Date = AnsiConsole.Confirm("Update shift's date?", false) ? AnsiConsole.Ask<DateOnly>("Shift's new date:") : editShift.Date;
        Console.WriteLine();
        editShift.ShiftName = AnsiConsole.Confirm("Update shift name?", false) ? AnsiConsole.Ask<string>("Shifts's new  name:") : editShift.ShiftName;
        Console.WriteLine();
        editShift.StartTime = AnsiConsole.Confirm("Update shift's start time?", false) ? AnsiConsole.Ask<TimeOnly>("Shift's new start time:") : editShift.StartTime;
        Console.WriteLine();
        editShift.EndTime = AnsiConsole.Confirm("Update shift's end time?", false) ? AnsiConsole.Ask<TimeOnly>("Shift's new end time:") : editShift.EndTime;
        Console.WriteLine();
        var workerService = new WorkerService(new HttpClient());
        var shiftWorkers = await workerService.GetWorkersAsync();
        editShift.Worker = AnsiConsole.Confirm("Update shift's worker?", false) ? userInterface.SelectWorker("\nChoose Worker for Shift:", shiftWorkers) : editShift.Worker;
        editShift.WorkerId = editShift.Worker.WorkerId;

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

    public static async Task DeleteShift()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var shiftService = new ShiftService(new HttpClient());
        var deleteShifts = await shiftService.GetShiftsAsync();

        var rule = new Rule("[green]Delete Shift[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var deleteShift = userInterface.SelectShift("\nChoose Shift to Delete:", deleteShifts);

        await ViewShifts("Delete Shift", deleteShift.ShiftId);

        if (AnsiConsole.Confirm("[yellow]Do you really want to delete this shift?[/]", false))
        {
            await shiftService.DeleteShift(deleteShift.ShiftId);
            await ViewShifts("Delete Shift");
            Console.WriteLine("\nSuccessfully deleted shift");
        }
        else
        {
            Console.WriteLine("\nShift not deleted.");
            return;
        }
    }
}
