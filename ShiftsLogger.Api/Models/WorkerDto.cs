using ShiftsLogger.API.Models;

namespace ShiftsLogger.Api.Models;

public class WorkerDto
{
    public int WorkerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Title { get; set; }

    public List<Shift> Shifts { get; set; }
}
