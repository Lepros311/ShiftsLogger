namespace ShiftsLogger.API.Models;

public class Worker
{
    public int WorkerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Title { get; set; }

    public List<Shift> Shifts { get; set; }
}
