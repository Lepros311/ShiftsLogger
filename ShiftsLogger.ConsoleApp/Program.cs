using ShiftsLogger.App.Views;

class Program
{
    static async Task Main()
    {

        Console.Title = "Shifts Logger";

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7150")
        };

        static async Task EnsureDatabaseInitialized(HttpClient httpClient)
        {
            int retries = 5;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    var response = await httpClient.PostAsync("/api/Database/Initialize", null);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Database is ready.");
                        return;
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("API not available yet, retrying...");
                    await Task.Delay(2000); // Wait 2 seconds before retrying
                }
            }

            Console.WriteLine("Error initializing database after multiple attempts.");
        }


        await EnsureDatabaseInitialized(httpClient);



        await UserInterface.PrintSelectionMainMenu();
    }
}