using Spectre.Console;
using ShiftsLogger.Services;
using ShiftsLogger.Models;

namespace ShiftsLogger.Views;

public class UserInterface
{
    public static async Task PrintSelectionMainMenu()
    {
        var isAppRunning = true;
        while (isAppRunning)
        {
            Console.Clear();
            var options = new[] { "View/Manage/Interact with Shifts", "View & Manage Workers", "Close Shifts Logger" };

            var mainMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(options));

            switch (mainMenuChoice)
            {
                case "View & Manage Shifts":
                    await PrintSelectionShiftsMenu();
                    break;
                case "View & Manage Workers":
                    PrintSelectionWorkersMenu();
                    break;
                case "Close Shifts Logger":
                    Console.WriteLine("Goodbye!");
                    Thread.Sleep(2000);
                    isAppRunning = false;
                    break;
            }
        }

    }

    static internal void PrintSelectionWorkersMenu()
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
                    List<Worker> workers = WorkerService.GetWorkers();
                    Display.PrintWorkersTable(workers, "View Workers");
                    ReturnToPreviousMenu();
                    break;
                case "Add Worker":
                    WorkerService.InsertWorker();
                    ReturnToPreviousMenu();
                    break;
                case "Edit Worker":
                    WorkerService.UpdateWorker();
                    ReturnToPreviousMenu();
                    break;
                case "Delete Worker":
                    WorkerService.DeleteWorker();
                    ReturnToPreviousMenu();
                    break;
                case "Return to Main Menu":
                    isWorkersMenuRunning = false;
                    break;
            }
        }
    }

    static internal async Task PrintSelectionShiftsMenu()
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
                    List<Shift> shifts = ShiftService.GetShifts();
                    Display.PrintShiftsTable(shifts, "View Shifts");
                    ReturnToPreviousMenu();
                    break;
                case "Add Shift":
                    ShiftService.InsertShift();
                    ReturnToPreviousMenu();
                    break;
                case "Edit Shift":
                    ShiftService.UpdateShift();
                    ReturnToPreviousMenu();
                    break;
                case "Delete Shift":
                    ShiftService.DeleteShift();
                    ReturnToPreviousMenu();
                    break;
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
}