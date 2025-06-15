using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.API.Models;

public class Worker
{
    public int WorkerId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Title { get; set; }

}
