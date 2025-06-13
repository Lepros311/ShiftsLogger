namespace ShiftsLogger.ConsoleApp.Models;

public class WorkerDto
{
    public int WorkerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}, {Title}";
    }
}
