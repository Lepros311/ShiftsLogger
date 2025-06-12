using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Controllers;

internal class WorkerController
{
    public static async Task ViewWorkers(string heading, int? workerId = null)
    {
        var workerService = new WorkerService(new HttpClient());
        List<WorkerDto> viewWorkers;
        if (workerId.HasValue)
        {
            var worker = await workerService.GetWorkerAsync(workerId.Value);
            viewWorkers = worker is not null ? new List<WorkerDto> { worker } : new List<WorkerDto>();
            
        }
        else
        {
            viewWorkers = await workerService.GetWorkersAsync();
        }

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
        
        var rule = new Rule("[green]Edit Worker[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);
        
        var selectedEditWorkerOption = userInterface.SelectOption("\nChoose Worker to Edit:", editWorkersDict.Keys);
        
        var editWorker = editWorkersDict[selectedEditWorkerOption];

        await ViewWorkers("Edit Worker", editWorker.WorkerId);

        Console.WriteLine();
        editWorker.FirstName = AnsiConsole.Confirm("Update worker's first name?", false) ? AnsiConsole.Ask<string>("Worker's new first name:") : editWorker.FirstName;
        Console.WriteLine();
        editWorker.LastName = AnsiConsole.Confirm("Update worker's last name?", false) ? AnsiConsole.Ask<string>("Worker's new last name:") : editWorker.LastName;
        Console.WriteLine();
        editWorker.Title = AnsiConsole.Confirm("Update worker's title?", false) ? AnsiConsole.Ask<string>("Worker's new title:") : editWorker.Title;

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
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var deleteWorkers = await workerService.GetWorkersAsync();
        var deleteWorkersDict = deleteWorkers.ToDictionary(x => $"{x.FirstName} {x.LastName}, {x.Title}");

        var rule = new Rule("[green]Delete Worker[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var selectedDeleteWorkerOption = userInterface.SelectOption("\nChoose Worker to Delete:", deleteWorkersDict.Keys);

        var deleteWorker = deleteWorkersDict[selectedDeleteWorkerOption];

        await ViewWorkers("Delete Worker", deleteWorker.WorkerId);

        if (AnsiConsole.Confirm($"[yellow]Do you really want to delete {deleteWorker.FirstName} {deleteWorker.LastName}, {deleteWorker.Title}?[/]", false))
        {
            await workerService.DeleteWorker(deleteWorker.WorkerId);
            await ViewWorkers("Delete Worker");
            Console.WriteLine("\nSuccessfully deleted worker");
        }
        else
        {
            Console.WriteLine("\nContact not deleted.");
            return;
        }
    }
}
