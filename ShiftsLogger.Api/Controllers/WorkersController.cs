using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly ShiftsDbContext _context;

    public WorkersController(DbContextOptions<ShiftsDbContext> options)
    {
        _context = new ShiftsDbContext(options);
    }

    //[HttpGet]
    //public List<Worker> GetWorkersWithShifts()
    //{
    //    //List<Worker> workers = _context.Workers.Include(x => x.Shifts).ToList();
    //    List<Worker> workers = _context.Workers.ToList();
    //    return workers;
    //}

    [HttpGet("workers")]
    public List<Worker> GetWorkers()
    {
        List<Worker> workers = _context.Workers.ToList();
        return workers;
    }

    [HttpGet("{id}")]
    public ActionResult<Worker> GetWorkerById(int id)
    {
        var result = _context.Workers.Find(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);

        //Worker worker = _context.Workers.Find(id);
        //return worker;
    }

    [HttpPost]
    public IActionResult AddWorker([FromBody] Worker worker)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _context.Add(worker);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorker(int id, [FromBody] Worker worker)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var workerById = await _context.Workers.FindAsync(id);
        if (workerById == null)
        {
            return NotFound();
        }

        workerById.FirstName = worker.FirstName;
        workerById.LastName = worker.LastName;
        workerById.Title = worker.Title;

        _context.Update(workerById);
        _context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorker(int id)
    {
        var workerById = await _context.Workers.FindAsync(id);
        if (workerById == null)
        {
            return NotFound();
        }

        _context.Remove(workerById);
        _context.SaveChanges();

        return NoContent();
    }
}
