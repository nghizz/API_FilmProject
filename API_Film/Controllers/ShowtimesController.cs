// ShowtimesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public ShowtimesController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllShowtimes()
        {
            return Ok(_context.Showtimes.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetShowtimeById(long id)
        {
            var showtime = _context.Showtimes.Find(id);
            if (showtime == null) return NotFound();
            return Ok(showtime);
        }

        [HttpPost]
        public IActionResult CreateShowtime(Showtime showtime)
        {
            _context.Showtimes.Add(showtime);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetShowtimeById), new { id = showtime.Id }, showtime);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShowtime(long id, Showtime showtime)
        {
            if (id != showtime.Id) return BadRequest();
            _context.Entry(showtime).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShowtime(long id)
        {
            var showtime = _context.Showtimes.Find(id);
            if (showtime == null) return NotFound();
            _context.Showtimes.Remove(showtime);
            _context.SaveChanges();
            return NoContent();
        }
    }
}