using Spectre.Console;
using ShiftsLogger.ConsoleApp.Models;

namespace ShiftsLogger.ConsoleApp.Views;

public class Display
{
    public static void PrintShiftsTable(List<ShiftDto> shifts, string heading)
    {
        Console.Clear();

        var rule = new Rule($"[green]{heading}[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("[dodgerblue1]Shift Name[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Start Time[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]End Time[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Duration[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Worker[/]"));

        foreach (ShiftDto shift in shifts)
        {
            Console.WriteLine($"Debug: {shift.ShiftId}, {shift.ShiftName}, {shift.StartTime}, {shift.EndTime}, {shift.Duration}, {shift.WorkerName}");

            table.AddRow(shift.ShiftName ?? "N/A", 
                        shift.StartTime.ToString() ?? "N/A", 
                        shift.EndTime.ToString() ?? "N/A", 
                        shift.Duration.ToString() ?? "N/A", 
                        shift.WorkerName ?? "N/A");
        }

        AnsiConsole.Write(table);
    }

    static internal void PrintWorkersTable(List<WorkerDto> workers, string heading)
    {
        Console.Clear();

        var rule = new Rule($"[green]{heading}[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var table = new Table();
        table.AddColumn("[dodgerblue1]First Name[/]");
        table.AddColumn("[dodgerblue1]Last Name[/]");
        table.AddColumn("[dodgerblue1]Title[/]");

        foreach (WorkerDto worker in workers)
        {
            table.AddRow(worker.FirstName, worker.LastName, worker.Title);
        }

        AnsiConsole.Write(table);
    }
}
