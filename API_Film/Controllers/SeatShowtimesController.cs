// SeatShowtimesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatShowtimesController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public SeatShowtimesController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllSeatShowtimes()
        {
            return Ok(_context.SeatShowtimes.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetSeatShowtimeById(long id)
        {
            var seatShowtime = _context.SeatShowtimes.Find(id);
            if (seatShowtime == null) return NotFound();
            return Ok(seatShowtime);
        }

        [HttpPost]
        public IActionResult CreateSeatShowtime(SeatShowtime seatShowtime)
        {
            _context.SeatShowtimes.Add(seatShowtime);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSeatShowtimeById), new { id = seatShowtime.ShowtimeId }, seatShowtime);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeatShowtime(long id)
        {
            var seatShowtime = _context.SeatShowtimes.Find(id);
            if (seatShowtime == null) return NotFound();
            _context.SeatShowtimes.Remove(seatShowtime);
            _context.SaveChanges();
            return NoContent();
        }
    }
}