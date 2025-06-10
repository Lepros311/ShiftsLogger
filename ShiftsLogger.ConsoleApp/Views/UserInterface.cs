using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using Spectre.Console;

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



    public async Task<ShiftDto> PromptForNewShift()
    {
        Console.Clear();
        var shifts = await _shiftService.GetShiftsAsync();
        Display.PrintShiftsTable(shifts, "Add Shift");

        ShiftDto shiftDto = new ShiftDto();



        return shiftDto;
    }
}