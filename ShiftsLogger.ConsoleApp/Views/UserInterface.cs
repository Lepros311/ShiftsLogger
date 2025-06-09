using Spectre.Console;
using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using System.Text.Json;
using ShiftsLogger.API.Controllers;

namespace ShiftsLogger.App.Views;

public class UserInterface
{
    private readonly ShiftService _shiftService;

    private readonly WorkerService _workerService;

    public UserInterface(ShiftService shiftService, WorkerService workerService)
    {
        _shiftService = shiftService;
        _workerService = workerService;
    }
    public static async Task PrintSelectionMainMenu()
    {
        var isAppRunning = true;
        while (isAppRunning)
        {
            Console.Clear();
            var options = new[] { "View & Manage Shifts", "View & Manage Workers", "Close Shifts Logger" };

            var mainMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(options));

            var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));

            switch (mainMenuChoice)
            {
                case "View & Manage Shifts":
                    await userInterface.PrintSelectionShiftsMenu();
                    break;
                case "View & Manage Workers":
                    await userInterface.PrintSelectionWorkersMenu();
                    break;
                case "Close Shifts Logger":
                    Console.WriteLine("Goodbye!");
                    Thread.Sleep(2000);
                    isAppRunning = false;
                    break;
            }
        }

    }

    internal async Task PrintSelectionWorkersMenu()
    {
        var isWorkersMenuRunning = true;
        while (isWorkersMenuRunning)
        {
            Console.Clear();

            var options = new[] { "View Workers", "Add Worker", "Edit Worker", "Delete Worker", "Return to Main Menu" };

            var workersMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("WORKERS MENU")
                .PageSize(10)
                .AddChoices(options));

            switch (workersMenuChoice)
            {
                case "View Workers":
                    var httpClient = new HttpClient();
                    httpClient.BaseAddress = new Uri("https://localhost:7150");
                    var response = await httpClient.GetStringAsync("/api/Workers");
                    var workers = JsonSerializer.Deserialize<List<WorkerDto>>(response);
                    Display.PrintWorkersTable(workers, "View Workers");
                    ReturnToPreviousMenu();
                    break;
                //case "Add Worker":
                //    await _workerService.InsertWorker();
                //    ReturnToPreviousMenu();
                //    break;
                //case "Edit Worker":
                //    await _workerService.UpdateWorker();
                //    ReturnToPreviousMenu();
                //    break;
                //case "Delete Worker":
                //    await _workerService.DeleteWorker();
                //    ReturnToPreviousMenu();
                //    break;
                case "Return to Main Menu":
                    isWorkersMenuRunning = false;
                    break;
            }
        }
    }

    internal async Task PrintSelectionShiftsMenu()
    {
        var isShiftsMenuRunning = true;
        while (isShiftsMenuRunning)
        {
            Console.Clear();

            var options = new[] { "View Shifts", "Add Shift", "Edit Shift", "Delete Shift", "Return to Main Menu" };

            var shiftsMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("SHIFTS MENU")
                .PageSize(10)
                .AddChoices(options));

            switch (shiftsMenuChoice)
            {
                case "View Shifts":
                    var shifts = await _shiftService.GetShiftsAsync();
                    Display.PrintShiftsTable(shifts, "View Shifts");
                    ReturnToPreviousMenu();
                    break;
                case "Add Shift":
                    ShiftDto shift = await PromptForNewShift();
                    await _shiftService.CreateShiftAsync(shift);
                    ReturnToPreviousMenu();
                    break;
                //case "Edit Shift":
                //    _shiftService.UpdateShift();
                //    ReturnToPreviousMenu();
                //    break;
                //case "Delete Shift":
                //    _shiftService.DeleteShift();
                //    ReturnToPreviousMenu();
                //    break;
                case "Return to Main Menu":
                    isShiftsMenuRunning = false;
                    break;
            }
        }
    }

    public static void ReturnToPreviousMenu()
    {
        Console.Write("\nPress any key to return to the previous menu...");
        Console.ReadKey();
    }

    public async Task<ShiftDto> PromptForNewShift()
    {
        Console.Clear();
        var shifts = await _shiftService.GetShiftsAsync();
        Display.PrintShiftsTable(shifts, "Add Shift");

        ShiftDto shiftDto = new ShiftDto();


        return shiftDto;
    }
}