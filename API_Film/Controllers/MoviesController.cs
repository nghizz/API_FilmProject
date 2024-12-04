// MoviesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using API_Film.DTOs;

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
            var movies = _context.Movies
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Genre = m.Genre,
                    Duration = m.Duration,
                    Description = m.Description,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                })
                .ToList();

            return Ok(movies);
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

        [HttpGet("search")]
        public IActionResult SearchMovies([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword không được để trống.");
            }

            var movies = _context.Movies
                .Where(m => m.Name.Contains(keyword) || m.Description.Contains(keyword))
                .ToList();

            return Ok(movies);
        }
    }
}