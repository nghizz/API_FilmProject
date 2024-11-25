using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public SeatsController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: api/Seats/available
        [HttpGet("available")]  // Thêm route cho ghế có sẵn
        public async Task<ActionResult<IEnumerable<Seat>>> GetAvailableSeats()
        {
            var availableSeats = await _context.Seats.Where(s => s.IsAvailable).ToListAsync();
            return Ok(availableSeats);
        }

        // GET: api/Seats/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Seat>> GetSeatById(long id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                return NotFound();
            return Ok(seat);
        }

        // POST: api/Seats
        [HttpPost]
        public async Task<ActionResult<Seat>> CreateSeat(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSeatById), new { id = seat.Id }, seat);
        }

        // PUT: api/Seats/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(long id, Seat seat)
        {
            if (id != seat.Id)
                return BadRequest();

            _context.Entry(seat).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Seats.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/Seats/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(long id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                return NotFound();

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
