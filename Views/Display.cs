using ShiftsLogger.Models;
using Spectre.Console;

namespace ShiftsLogger.Views;

public class Display
{
    public static void PrintShiftsTable(List<Shift> shifts, string heading)
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

        foreach (Shift shift in shifts)
        {
            table.AddRow(shift.ShiftName, shift.StartTime.ToString(), shift.EndTime.ToString(), shift.Duration.ToString(), shift.Worker.FirstName);
        }

        AnsiConsole.Write(table);
    }

    static internal void PrintWorkersTable(List<Worker> workers, string heading)
    {
        Console.Clear();

        var rule = new Rule($"[green]{heading}[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var table = new Table();
        table.AddColumn("[dodgerblue1]Worker Name[/]");

        foreach (Worker worker in workers)
        {
            table.AddRow(worker.ToString());
        }

        AnsiConsole.Write(table);
    }
}
