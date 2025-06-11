using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Controllers;

internal class WorkerController
{
    public static async Task ViewWorkers(string heading)
    {
        var workerService = new WorkerService(new HttpClient());
        var viewWorkers = await workerService.GetWorkersAsync();
        Display.PrintWorkersTable(viewWorkers, heading);
    }

    public static async Task ViewWorker(string heading, int workerId)
    {
        var workerService = new WorkerService(new HttpClient());
        var viewWorkers = await workerService.GetWorkerAsync(workerId);
        Display.PrintWorkersTable(viewWorkers, heading);
    }

    public static async Task CreateWorker()
    {
        await ViewWorkers("Add Worker");
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var newFirstName = userInterface.ReadString("Enter First Name: ");
        var newLastName = userInterface.ReadString("Enter Last Name: ");
        var newTitle = userInterface.ReadString("Enter a Title: ");
        var worker = new WorkerDto { Title = newTitle, FirstName = newFirstName, LastName = newLastName };
        var insertResult = await workerService.InsertWorker(worker);
        try
        {
            Console.Clear();
            await ViewWorkers("Create Worker");
            Console.WriteLine("\nSuccessfully saved worker");
        }
        catch (HttpRequestException e) 
        {
            Console.Clear();
            await ViewWorkers("Create Worker");
            Console.WriteLine($"\nFailed to save worker. Request error: {e.Message}");
        }
    }

    public static async Task EditWorker()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var editWorkers = await workerService.GetWorkersAsync();
        var editWorkersDict = editWorkers.ToDictionary(x => $"{x.FirstName} {x.LastName}, {x.Title}");
        //var rule = new Rule($"[green]Edit Worker[/]");
        //rule.Justification = Justify.Left;
        //AnsiConsole.Write(rule);
        var selectedEditWorkerOption = userInterface.SelectOption("\nChoose Worker to Edit:", editWorkersDict.Keys);
        var editWorker = editWorkersDict[selectedEditWorkerOption];
        await ViewWorker("Edit Worker", editWorker.WorkerId);
        var editFirstName = userInterface.ReadString("Enter First Name: ", editWorker.FirstName);
        var editLastName = userInterface.ReadString("Enter Last Name: ", editWorker.LastName);
        var editTitle = userInterface.ReadString("Enter a Title: ", editWorker.Title);

        editWorker.FirstName = editFirstName;
        editWorker.LastName = editLastName;
        editWorker.Title = editTitle;

        var editResult = await workerService.UpdateWorker(editWorker);
        if (editResult)
        {
            Console.Clear();
            await ViewWorkers("Edit Worker");
            Console.WriteLine("\nSuccessfully edited worker");
        }
        else
        {
            Console.Clear();
            await ViewWorkers("Edit Worker");
            Console.WriteLine("\nFailed to edit worker");
        }
    }

    public static async Task DeleteWorker()
    {
        await ViewWorkers("Delete Worker");
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var deleteWorkers = await workerService.GetWorkersAsync();
        var deleteWorkersDict = deleteWorkers.ToDictionary(x => $"{x.WorkerId}. {x.FirstName} {x.LastName}");
        var selectedDeleteWorkerOption = userInterface.SelectOption("Choose Worker to delete", deleteWorkersDict.Keys);

        var deleteWorker = deleteWorkersDict[selectedDeleteWorkerOption];

        var deleteAnswer = userInterface.ReadString($"Are you sure you want to delte worker: '{deleteWorker.FirstName} {deleteWorker.LastName}'? Y/N", ["y", "n"]);
        if (deleteAnswer.ToLower() == "y")
        {
            await workerService.DeleteWorker(deleteWorker.WorkerId);
            Console.Clear();
            await ViewWorkers("Delete Worker");
            Console.WriteLine("Successfully deleted worker");
        }
    }
}
