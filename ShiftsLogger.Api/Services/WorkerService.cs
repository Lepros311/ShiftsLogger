using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Services;

public interface IWorkerService
{
    public List<Worker> GetWorkers();
    public Worker? GetWorkerById(int id);
    public Worker CreateWorker(Worker worker);
    public Worker UpdateWorker(Worker updatedWorker);
    public string? DeleteWorker(int id);
}

public class WorkerService : IWorkerService
{
    private readonly ShiftsDbContext Context;

    public WorkerService(ShiftsDbContext context)
    {
        Context = context;
    }

    public Worker CreateWorker(Worker worker)
    {
        var savedWorker = Context.Workers.Add(worker);
        Context.SaveChanges();
        return savedWorker.Entity;
    }

    public string? DeleteWorker(int id)
    {
        Worker savedWorker = Context.Workers.Find(id);

        if (savedWorker == null)
        {
            return null;
        }

        Context.Workers.Remove(savedWorker);
        Context.SaveChanges();

        return $"Successfully deleted worker with id: {id}";
    }

    public List<Worker> GetWorkers()
    {
        return Context.Workers.ToList();
    }

    public Worker? GetWorkerById(int id)
    {
        Worker savedWorker = Context.Workers.Find(id);
        return savedWorker == null ? null : savedWorker;
    }

    public Worker UpdateWorker(Worker worker)
    {
        Worker savedWorker = Context.Workers.Find(worker.WorkerId);

        if (savedWorker == null)
        {
            return null;
        }

        Context.Entry(savedWorker).CurrentValues.SetValues(worker);
        Context.SaveChanges();

        return savedWorker;
    }
}

