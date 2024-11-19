// MoviesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public MoviesController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            return Ok(_context.Movies.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieById(long id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpPost]
        public IActionResult CreateMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(long id, Movie movie)
        {
            if (id != movie.Id) return BadRequest();
            _context.Entry(movie).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(long id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return NoContent();
        }
    }
}