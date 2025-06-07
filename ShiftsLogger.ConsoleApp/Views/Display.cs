using Spectre.Console;
using ShiftsLogger.ConsoleApp.Models;

namespace ShiftsLogger.App.Views;

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
            .AddColumn(new TableColumn("[dodgerblue1]Phone Number[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Email Address[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Duration[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Worker[/]"));

        foreach (ShiftDto shift in shifts)
        {
            table.AddRow(shift.ShiftName, shift.StartTime.ToString(), shift.EndTime.ToString(), shift.Duration.ToString(), shift.Worker.FirstName);
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
        table.AddColumn("[dodgerblue1]Worker Name[/]");

        foreach (WorkerDto worker in workers)
        {
            table.AddRow(worker.ToString());
        }

        AnsiConsole.Write(table);
    }
}
