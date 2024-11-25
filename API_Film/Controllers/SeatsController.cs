// SeatsController.cs
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

        // GET: api/Seats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> GetSeats()
        {
            var seats = await _context.Seats.ToListAsync();
            return Ok(seats);
        }

        [HttpGet]
        public IActionResult GetAllSeats()
        {
            return Ok(_context.Seats.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetSeatById(long id)
        {
            var seat = _context.Seats.Find(id);
            if (seat == null) return NotFound();
            return Ok(seat);
        }

        [HttpPost]
        public IActionResult CreateSeat(Seat seat)
        {
            _context.Seats.Add(seat);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSeatById), new { id = seat.Id }, seat);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSeat(long id, Seat seat)
        {
            if (id != seat.Id) return BadRequest();
            _context.Entry(seat).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeat(long id)
        {
            var seat = _context.Seats.Find(id);
            if (seat == null) return NotFound();
            _context.Seats.Remove(seat);
            _context.SaveChanges();
            return NoContent();
        }
    }
}