using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using ShiftsLogger.ConsoleApp.Models;

namespace ShiftsLogger.ConsoleApp.Controllers;

internal class WorkerController
{
    public static async Task ViewWorkers()
    {
        var workerService = new WorkerService(new HttpClient());
        var viewWorkers = await workerService.GetWorkersAsync();
        Display.PrintWorkersTable(viewWorkers, "View Workers");
    }

    public static async Task CreateWorker()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var newTitle = userInterface.ReadString("Enter a Title");
        var newFirstName = userInterface.ReadString("Enter First Name");
        var newLastName = userInterface.ReadString("Enter Last Name");
        var worker = new WorkerDto { Title = newTitle, FirstName = newFirstName, LastName = newLastName };
        var insertResult = await workerService.InsertWorker(worker);
        if (insertResult)
        {
            Console.WriteLine("Successfully saved worker");
        }
        else
        {
            Console.WriteLine("Failed to save worker");
        }
    }

    public static async Task EditWorker()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var editWorkers = await workerService.GetWorkersAsync();
        var editWorkersDict = editWorkers.ToDictionary(x => $"{x.WorkerId}. {x.FirstName} {x.LastName}");
        var selectedEditWorkerOption = userInterface.SelectOption("Choose Worker to edit", editWorkersDict.Keys);
        var editWorker = editWorkersDict[selectedEditWorkerOption];

        var editTitle = userInterface.ReadString("Enter a Title", editWorker.Title);
        var editFirstName = userInterface.ReadString("Enter First Name", editWorker.FirstName);
        var editLastName = userInterface.ReadString("Enter Last Name", editWorker.LastName);

        editWorker.FirstName = editFirstName;
        editWorker.LastName = editLastName;
        editWorker.Title = editTitle;

        var editResult = await workerService.UpdateWorker(editWorker);
        if (editResult)
        {
            Console.WriteLine("Successfully updated worker");
        }
        else
        {
            Console.WriteLine("Failed to update worker");
        }
    }

    public static async Task DeleteWorker()
    {
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
            Console.WriteLine("Successfully deleted worker");
        }
    }
}
