using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Controllers;

public class WorkersController
{
    private readonly ShiftsDbContext _context;

    public WorkersController(DbContextOptions<ShiftsDbContext> options) 
    { 
        _context = new ShiftsDbContext(options); 
    }

    internal List<Worker> GetWorkers()
    {
        List<Worker> workers = _context.Workers.Include(x => x.Shifts).ToList();
        return workers;
    }

    internal void AddWorker(Worker worker)
    {
        _context.Add(worker);
        _context.SaveChanges();
    }

    internal void DeleteWorker(Worker worker)
    {
        _context.Remove(worker);
        _context.SaveChanges();
    }

    internal void UpdateWorker(Worker worker)
    {
        _context.Update(worker);
        _context.SaveChanges();
    }
}
