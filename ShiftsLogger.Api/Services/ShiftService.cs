using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Services;

public interface IShiftService
{
    public List<Shift> GetShifts();
    public Shift? GetShiftById(int id);
    public Shift CreateShift(Shift shift);
    public Shift UpdateShift(Shift updatedShift);
    public string? DeleteShift(int id);
}

public class ShiftService : IShiftService
{
    private readonly ShiftsDbContext Context;

    public ShiftService(ShiftsDbContext context)
    {
        Context = context;
    }

    public Shift CreateShift(Shift shift)
    {
        var savedShift = Context.Shifts.Add(shift);
        Context.SaveChanges();
        return savedShift.Entity;
    }

    public string? DeleteShift(int id)
    {
        Shift savedShift = Context.Shifts.Find(id);

        if (savedShift == null)
        {
            return null;
        }

        Context.Shifts.Remove(savedShift);
        Context.SaveChanges();

        return $"Successfully deleted shift with id: {id}";
    }

    public List<Shift> GetShifts()
    {
        return Context.Shifts.ToList();
    }

    public Shift? GetShiftById(int id)
    {
        Shift savedShift = Context.Shifts.Find(id);
        return savedShift == null ? null : savedShift;
    }

    public Shift UpdateShift(Shift shift)
    {
        Shift savedShift = Context.Shifts.Find(shift.ShiftId);

        if (savedShift == null)
        {
            return null;
        }

        Context.Entry(savedShift).CurrentValues.SetValues(shift);
        Context.SaveChanges();

        return savedShift;
    }
}
