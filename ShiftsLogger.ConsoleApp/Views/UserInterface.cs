using Microsoft.Extensions.Options;
using ShiftsLogger.API.Controllers;
using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using Spectre.Console;
using System.Net.Http.Json;
using System.Text.Json;

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

            var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient { BaseAddress = new Uri("https://localhost:7150/api/") }));

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
                    var viewWorkers = await _workerService.GetWorkersAsync();
                    Display.PrintWorkersTable(viewWorkers, "View Workers");
                    ReturnToPreviousMenu();
                    break;
                case "Add Worker":
                    var newTitle = ReadString("Enter a Title");
                    var newFirstName = ReadString("Enter First Name");
                    var newLastName = ReadString("Enter Last Name");
                    var worker = new WorkerDto { Title = newTitle, FirstName = newFirstName, LastName = newLastName };
                    var insertResult = await _workerService.InsertWorker(worker);
                    if (insertResult)
                    {
                        Console.WriteLine("Successfully saved worker");
                    }
                    else
                    {
                        Console.WriteLine("Failed to save worker");
                    }

                    ReturnToPreviousMenu();
                    break;
                case "Edit Worker":
                    var editWorkers = await _workerService.GetWorkersAsync();
                    var editWorkersDict = editWorkers.ToDictionary(x => $"{x.WorkerId}. {x.FirstName} {x.LastName}");
                    var selectedEditWorkerOption = SelectOption("Choose Worker to edit", editWorkersDict.Keys);
                    var editWorker = editWorkersDict[selectedEditWorkerOption];

                    var editTitle = ReadString("Enter a Title", editWorker.Title);
                    var editFirstName = ReadString("Enter First Name", editWorker.FirstName);
                    var editLastName = ReadString("Enter Last Name", editWorker.LastName);

                    editWorker.FirstName = editFirstName;
                    editWorker.LastName = editLastName;
                    editWorker.Title = editTitle;

                    var editResult = await _workerService.UpdateWorker(editWorker);
                    if (editResult)
                    {
                        Console.WriteLine("Successfully updated worker");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update worker");
                    }

                    ReturnToPreviousMenu();
                    break;
                case "Delete Worker":
                    var deleteWorkers = await _workerService.GetWorkersAsync();
                    var deleteWorkersDict = deleteWorkers.ToDictionary(x => $"{x.WorkerId}. {x.FirstName} {x.LastName}");
                    var selectedDeleteWorkerOption = SelectOption("Choose Worker to delete", deleteWorkersDict.Keys);

                    var deleteWorker = deleteWorkersDict[selectedDeleteWorkerOption];

                    var deleteAnswer = ReadString($"Are you sure you want to delte worker: '{deleteWorker.FirstName} {deleteWorker.LastName}'? Y/N", ["y", "n"]);
                    if (deleteAnswer.ToLower() == "y")
                    {
                        await _workerService.DeleteWorker(deleteWorker.WorkerId);
                        Console.WriteLine("Successfully deleted worker");
                    }
                    ReturnToPreviousMenu();
                    break;
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


    private string ReadString(string question)
    {
        Console.WriteLine(question);
        var answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || string.IsNullOrWhiteSpace(answer))
        {
            Console.WriteLine("Invalid input");
            answer = ReadString(question);
        }

        return answer;
    }


    private string ReadString(string question, string[] options)
    {
        Console.WriteLine(question);
        var answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || string.IsNullOrWhiteSpace(answer))
        {
            Console.WriteLine("Invalid input");
            answer = ReadString(question, options);
        }

        if (!options.Contains(answer.ToLower()))
        {
            Console.WriteLine("Invalid option");
            answer = ReadString(question, options);
        }

        return answer;
    }

    private string ReadString(string question, string currentValue)
    {
        Console.WriteLine(question);
        Console.WriteLine("Current Value: " + currentValue);
        Console.WriteLine("New Value: ");
        var answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || string.IsNullOrWhiteSpace(answer))
        {
            answer = currentValue;
        }

        return answer;
    }

    private string SelectOption(string title, IEnumerable<string> choices)
    {
        var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(title)
                .PageSize(10)
                .AddChoices(choices));

        return option;
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