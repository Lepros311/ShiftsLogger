using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Models;
using ShiftsLogger.API.Data;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Services;

namespace ShiftsLogger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftsDbContext _context;

        public ShiftsController(ShiftsDbContext context)
        {
            _context = context;
        }

        // GET: api/Shifts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShifts()
        {
            //return await _context.Shifts.Include(s => s.Worker).ToListAsync();
            try
            {
                var shifts = await _context.Shifts
                    .Include(s => s.Worker)
                    .Select(s => new ShiftDto
                    {
                        ShiftId = s.ShiftId,
                        ShiftName = s.ShiftName,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Duration = s.Duration,
                        WorkerName = s.Worker.FirstName + " " + s.Worker.LastName
                    })
                    .ToListAsync();

                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Shifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(int id)
        {
            var shift = await _context.Shifts
                .Include(s => s.Worker)
                .Where(s => s.ShiftId == id)
                .Select(s => new ShiftDto
                {
                    ShiftId = s.ShiftId,
                    ShiftName = s.ShiftName,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Duration = s.Duration,
                    WorkerName = s.Worker.FirstName + " " + s.Worker.LastName
                })
                .FirstOrDefaultAsync();

            if (shift == null)
            {
                return NotFound();
            }

            return Ok(shift);
        }

        // PUT: api/Shifts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShift(int id, Shift shift)
        {
            if (id != shift.ShiftId)
            {
                return BadRequest();
            }

            _context.Entry(shift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShiftExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shifts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Shift>> PostShift(ShiftDto shiftDto)
        {
            var worker = await _context.Workers
                .Where(w => (w.FirstName + " " + w.LastName) == shiftDto.WorkerName)
                .Select(w => new WorkerDto
                {
                    WorkerId = w.WorkerId,
                    FirstName = w.FirstName,
                    LastName = w.LastName
                })
                .FirstOrDefaultAsync();

            Shift shift = new Shift
            {
                ShiftName = shiftDto.ShiftName,
                StartTime = shiftDto.StartTime,
                EndTime = shiftDto.EndTime,
                Duration = shiftDto.Duration,
                WorkerId = worker.WorkerId,
                //Worker = worker
            };

            ShiftService shiftService = new ShiftService(_context);
            shiftService.CreateShift(shift);

            return CreatedAtAction("GetShift", new { id = shift.ShiftId }, shift);
        }

        // DELETE: api/Shifts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.ShiftId == id);
        }
    }
}
