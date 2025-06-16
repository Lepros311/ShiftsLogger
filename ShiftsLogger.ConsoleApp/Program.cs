using ShiftsLogger.ConsoleApp.Views;

class ConsoleAppProgram
{
    static async Task Main()
    {
        Console.Title = "Shifts Logger";

        await Menus.PrintSelectionMainMenu();
    }
}