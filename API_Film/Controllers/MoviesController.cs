// MoviesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using API_Film.DTOs;
using Microsoft.EntityFrameworkCore;

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


        // Trong MoviesController.cs
        [HttpGet("{id}")]
        public IActionResult GetMovieById(long id)
        {
            var movie = _context.Movies.Include(m => m.Showtimes)
                                  .FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound();

            // Chuyển đổi danh sách Showtimes sang danh sách ShowtimeDTO
            var showtimeDTOs = movie.Showtimes.Select(s => new ShowtimeDTO
            {
                Id = s.Id,
                StartTime = s.StartTime
            }).ToList();

            // Tạo một object mới để trả về, chỉ bao gồm thông tin cần thiết
            var result = new
            {
                movie.Id,
                movie.Name,
                movie.Genre,
                movie.Duration,
                movie.Description,
                movie.TrailerUrl,
                movie.Director,
                movie.ImageUrl,
                showtimes = showtimeDTOs // Sử dụng danh sách ShowtimeDTO
            };

            return Ok(result);
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