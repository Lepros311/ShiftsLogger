using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.API.Data;

namespace ShiftsLogger.Api.Controllers;

[Route("api/Database")]
[ApiController]

public class DatabaseController : ControllerBase
{
    private readonly ShiftsDbContext _context;

    public DatabaseController(ShiftsDbContext context)
    {
        _context = context;
    }

    [HttpPost("Initializer")]
    public IActionResult InitializeDatabase()
    {
        _context.Database.EnsureCreated();
        return Ok("Database Initialized.");
    }
}
