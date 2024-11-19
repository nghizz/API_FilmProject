// CinemasController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public CinemasController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllCinemas()
        {
            return Ok(_context.Cinemas.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetCinemaById(long id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null) return NotFound();
            return Ok(cinema);
        }

        [HttpPost]
        public IActionResult CreateCinema(Cinema cinema)
        {
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCinemaById), new { id = cinema.Id }, cinema);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCinema(long id, Cinema cinema)
        {
            if (id != cinema.Id) return BadRequest();
            _context.Entry(cinema).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(long id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null) return NotFound();
            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }
    }
}