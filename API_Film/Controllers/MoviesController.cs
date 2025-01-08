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
                .Include(m => m.Showtimes) // Include Showtimes
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Genre = m.Genre,
                    Duration = m.Duration,
                    Description = m.Description,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    Showtimes = m.Showtimes.Select(s => s.StartTime).ToList() // Add showtimes to DTO
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
        public IActionResult UpdateMovie(long id, [FromBody] MovieDto movieDto)
        {
            // Kiểm tra ID có khớp không
            if (id != movieDto.Id)
                return BadRequest("ID không khớp.");

            // Tìm phim trong cơ sở dữ liệu
            var existingMovie = _context.Movies
                                        .Include(m => m.Showtimes) // Bao gồm cả danh sách showtimes
                                        .FirstOrDefault(m => m.Id == id);
            if (existingMovie == null)
                return NotFound("Không tìm thấy phim với ID được cung cấp.");

            // Cập nhật các trường dữ liệu
            existingMovie.Name = movieDto.Name;
            existingMovie.Genre = movieDto.Genre;
            existingMovie.Duration = movieDto.Duration;
            existingMovie.Description = movieDto.Description;
            existingMovie.Director = movieDto.Director;
            existingMovie.ImageUrl = movieDto.ImageUrl;

            // Cập nhật Showtimes
            if (movieDto.Showtimes != null)
            {
                // Xóa các showtimes cũ
                existingMovie.Showtimes.Clear();

                // Thêm các showtimes mới từ DTO
                foreach (var showtime in movieDto.Showtimes)
                {
                    existingMovie.Showtimes.Add(new Showtime
                    {
                        StartTime = showtime,
                        MovieId = existingMovie.Id // Gán MovieId để đảm bảo liên kết đúng
                    });
                }
            }

            // Đánh dấu thực thể đã được sửa đổi
            _context.Entry(existingMovie).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            // Lưu thay đổi
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi khi lưu thay đổi: {ex.Message}");
            }

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