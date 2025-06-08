namespace ShiftsLogger.ConsoleApp.Models;

public class ShiftDto
{
    public int ShiftId { get; set; }
    public string ShiftName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double Duration { get; set; }
    //public WorkerDto Worker { get; set; }
    public string WorkerName { get; set; }
}
