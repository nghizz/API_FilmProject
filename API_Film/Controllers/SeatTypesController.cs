using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatTypesController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public SeatTypesController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: api/SeatTypes/allSeatTypes
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<SeatType>>> GetAllSeatTypes()
        {
            var seatTypes = await _context.SeatTypes.ToListAsync();
            return Ok(seatTypes);
        }

        // GET: api/SeatTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SeatType>> GetSeatTypeById(long id)
        {
            var seatType = await _context.SeatTypes.FindAsync(id);
            if (seatType == null)
                return NotFound();
            return Ok(seatType);
        }

        // POST: api/SeatTypes
        [HttpPost]
        public async Task<ActionResult<SeatType>> CreateSeatType(SeatType seatType)
        {
            _context.SeatTypes.Add(seatType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSeatTypeById), new { id = seatType.Id }, seatType);
        }

        // PUT: api/SeatTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeatType(long id, SeatType seatType)
        {
            if (id != seatType.Id)
                return BadRequest();

            _context.Entry(seatType).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SeatTypes.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/SeatTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatType(long id)
        {
            var seatType = await _context.SeatTypes.FindAsync(id);
            if (seatType == null)
                return NotFound();

            _context.SeatTypes.Remove(seatType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
