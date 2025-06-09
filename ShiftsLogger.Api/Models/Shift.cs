using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftsLogger.API.Models;

public class Shift
{
    public int ShiftId { get; set; }

    public string ShiftName { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public TimeOnly Duration { get; set; }

    public int WorkerId { get; set; }

    public Worker Worker { get; set; }
}
