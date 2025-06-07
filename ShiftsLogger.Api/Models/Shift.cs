namespace ShiftsLogger.API.Models;

public class Shift
{
    public int ShiftId { get; set; }

    public string ShiftName { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public double Duration { get; set; }

    public int WorkerId { get; set; }

    public Worker Worker { get; set; }
}
