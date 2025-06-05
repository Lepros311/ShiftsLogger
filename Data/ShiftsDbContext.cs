using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Models;

namespace ShiftsLogger.Data;

public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Shift> Shifts { get; set; }

    public DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=shiftsDb;Trusted_Connection=True;Initial Catalog=shiftsDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>().HasOne(s => s.Worker).WithMany(w => w.Shifts).HasForeignKey(s => s.WorkerId);

        modelBuilder.Entity<Worker>()
            .HasData(new List<Worker>
            {
                new Worker
                {
                    WorkerId = 1,
                    FirstName = "Tom",
                    LastName = "Foolery",
                    Title = "Laborer"
                },
                new Worker
                {
                    WorkerId = 2,
                    FirstName = "Sue",
                    LastName = "Smith",
                    Title = "Laborer"
                },
                new Worker
                {
                    WorkerId = 3,
                    FirstName = "Doug",
                    LastName = "Mitchell",
                    Title = "Foreman"
                },
                new Worker
                {
                    WorkerId = 4,
                    FirstName = "Mike",
                    LastName = "Wilson",
                    Title = "Apprentice"
                }
            });

        modelBuilder.Entity<Shift>()
           .HasData(new List<Shift>
           {
                new Shift
                {
                    ShiftId = 1,
                    WorkerId = 1,
                    ShiftName = "1st",
                    StartTime = new DateTime(2023, 10, 15, 08, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 16, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 2,
                    WorkerId = 1,
                    ShiftName = "2nd",
                    StartTime = new DateTime(2023, 10, 15, 16, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 00, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 3,
                    WorkerId = 2,
                    ShiftName = "2nd",
                    StartTime = new DateTime(2023, 10, 15, 16, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 00, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 4,
                    WorkerId = 2,
                    ShiftName = "3rd",
                    StartTime = new DateTime(2023, 10, 15, 00, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 08, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 5,
                    WorkerId = 3,
                    ShiftName = "3rd",
                    StartTime = new DateTime(2023, 10, 15, 00, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 08, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 6,
                    WorkerId = 3,
                    ShiftName = "2nd",
                    StartTime = new DateTime(2023, 10, 15, 16, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 00, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 7,
                    WorkerId = 4,
                    ShiftName = "1st",
                    StartTime = new DateTime(2023, 10, 15, 08, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 16, 00, 0),
                    Duration = 8
                },
                new Shift
                {
                    ShiftId = 8,
                    WorkerId = 4,
                    ShiftName = "1st",
                    StartTime = new DateTime(2023, 10, 15, 08, 00, 0),
                    EndTime = new DateTime(2023, 10, 15, 16, 00, 0),
                    Duration = 8
                }
           });
    }
}
