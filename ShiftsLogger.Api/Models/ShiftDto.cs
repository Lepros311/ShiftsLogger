namespace ShiftsLogger.API.Models;

public class ShiftDto
{
    public int ShiftId { get; set; }
    public string ShiftName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeOnly Duration { get; set; }

    public int WorkerId { get; set; }

    public Worker Worker { get; set; }
}
