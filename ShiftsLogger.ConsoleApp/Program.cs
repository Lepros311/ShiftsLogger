using ShiftsLogger.ConsoleApp.Views;

class Program
{
    static async Task Main()
    {
        Console.Title = "Shifts Logger";

        await Menus.PrintSelectionMainMenu();
    }
}