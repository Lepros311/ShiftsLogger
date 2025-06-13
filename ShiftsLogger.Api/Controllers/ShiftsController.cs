using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Models;
using ShiftsLogger.API.Data;
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
                        Date = s.Date,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Duration = s.Duration,
                        Worker = s.Worker
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
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Duration = s.Duration,
                    Worker = s.Worker
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
        public async Task<IActionResult> PutShift(int id, [FromBody] ShiftDto shiftDto)
        {
            if (shiftDto == null || id != shiftDto.ShiftId)
            {
                return BadRequest("Invalid shift data.");
            }

            // Retrieve the existing shift
            var existingShift = await _context.Shifts.FindAsync(id);
            if (existingShift == null)
            {
                return NotFound("Shift not found.");
            }

            // Retrieve the worker (same as in POST)
            var worker = await _context.Workers.FindAsync(shiftDto.WorkerId);
            if (worker == null)
            {
                return NotFound("The specified worker does not exist.");
            }

            // Update the existing shift's properties
            existingShift.ShiftName = shiftDto.ShiftName;
            existingShift.Date = shiftDto.Date;
            existingShift.StartTime = shiftDto.StartTime;
            existingShift.EndTime = shiftDto.EndTime;
            existingShift.Duration = shiftDto.Duration;
            existingShift.WorkerId = worker.WorkerId;
            existingShift.Worker = worker;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
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
        }


        // POST: api/Shifts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostShift([FromBody] ShiftDto shiftDto)
        {
            if (shiftDto == null)
            {
                return BadRequest("Shift data is required.");
            }
            // Check if the worker exists and retrieve the Worker object
            var worker = await _context.Workers.FindAsync(shiftDto.WorkerId);
            if (worker == null)
            {
                return NotFound("The specified worker does not exist.");
            }

            Shift shift = new Shift
            {
                ShiftName = shiftDto.ShiftName,
                Date = shiftDto.Date,
                StartTime = shiftDto.StartTime,
                EndTime = shiftDto.EndTime,
                Duration = shiftDto.Duration,
                WorkerId = worker.WorkerId,
                Worker = worker
            };

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

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
