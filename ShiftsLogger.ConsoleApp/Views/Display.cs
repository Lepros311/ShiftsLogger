using ShiftsLogger.ConsoleApp.Models;
using Spectre.Console;

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
            .AddColumn(new TableColumn("[dodgerblue1]Date[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Shift[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Start Time[/]").RightAligned())
            .AddColumn(new TableColumn("[dodgerblue1]End Time[/]").RightAligned())
            .AddColumn(new TableColumn("[dodgerblue1]Duration[/]").RightAligned())
            .AddColumn(new TableColumn("[dodgerblue1]Worker[/]"))
            .AddColumn(new TableColumn("[dodgerblue1]Title[/]"));

        var sortedShifts = shifts.OrderByDescending(s => s.Date).ThenByDescending(s => s.StartTime).ToList();

        foreach (ShiftDto shift in sortedShifts)
        {
            table.AddRow(shift.Date.ToString() ?? "N/A",
                        shift.ShiftName ?? "N/A",
                        shift.StartTime.ToString("h:mm tt") ?? "N/A",
                        shift.EndTime.ToString("h:mm tt") ?? "N/A",
                        shift.Duration.ToString("H:mm") ?? "N/A",
                        ($"{shift.Worker.FirstName} {shift.Worker.LastName}") ?? "N/A",
                        shift.Worker.Title ?? "N/A");
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
