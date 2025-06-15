using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API.Data;

public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Shift> Shifts { get; set; }

    public DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=shiftsDb;Trusted_Connection=True;Initial Catalog=shiftsDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>().HasOne(s => s.Worker).WithMany().HasForeignKey(s => s.WorkerId);

        modelBuilder.Entity<Shift>()
            .Property(s => s.StartTime)
            .HasColumnType("TIME(0)"); 

        modelBuilder.Entity<Shift>()
            .Property(s => s.EndTime)
            .HasColumnType("TIME(0)"); 

        modelBuilder.Entity<Shift>()
            .Property(s => s.Duration)
            .HasComputedColumnSql(@"
            CONVERT(TIME(0), 
                CASE 
                    WHEN EndTime < StartTime 
                        THEN DATEADD(MINUTE, DATEDIFF(MINUTE, 
                            CAST(StartTime AS DATETIME), 
                            DATEADD(DAY, 1, CAST(EndTime AS DATETIME))
                        ), '00:00')
                    ELSE DATEADD(MINUTE, DATEDIFF(MINUTE, 
                            CAST(StartTime AS DATETIME), 
                            CAST(EndTime AS DATETIME)
                        ), '00:00')
                END)");

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
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 15)),
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("16:00")
                },
                new Shift
                {
                    ShiftId = 2,
                    WorkerId = 1,
                    ShiftName = "2nd",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 15)),
                    StartTime = TimeOnly.Parse("16:00"),
                    EndTime = TimeOnly.Parse("00:00")
                },
                new Shift
                {
                    ShiftId = 3,
                    WorkerId = 2,
                    ShiftName = "2nd",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 14)),
                    StartTime = TimeOnly.Parse("16:00"),
                    EndTime = TimeOnly.Parse("00:00")
                },
                new Shift
                {
                    ShiftId = 4,
                    WorkerId = 2,
                    ShiftName = "3rd",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 14)),
                    StartTime = TimeOnly.Parse("00:00"),
                    EndTime = TimeOnly.Parse("08:00")
                },
                new Shift
                {
                    ShiftId = 5,
                    WorkerId = 3,
                    ShiftName = "3rd",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 13)),
                    StartTime = TimeOnly.Parse("16:00"),
                    EndTime = TimeOnly.Parse("00:00")
                },
                new Shift
                {
                    ShiftId = 6,
                    WorkerId = 3,
                    ShiftName = "2nd",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 13)),
                    StartTime = TimeOnly.Parse("16:00"),
                    EndTime = TimeOnly.Parse("00:00")
                },
                new Shift
                {
                    ShiftId = 7,
                    WorkerId = 4,
                    ShiftName = "1st",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 14)),
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("16:00")
                },
                new Shift
                {
                    ShiftId = 8,
                    WorkerId = 4,
                    ShiftName = "1st",
                    Date = DateOnly.FromDateTime(new DateTime(2025, 06, 13)),
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("16:00")
                }
           });
    }
}
