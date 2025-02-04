﻿// MoviesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using API_Film.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
                .Include(m => m.Showtimes)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Genre = m.Genre,
                    Duration = m.Duration,
                    Description = m.Description ?? "", // Nếu Description null, gán chuỗi rỗng
                    Director = m.Director ?? "", // Nếu Director null, gán chuỗi rỗng
                    ImageUrl = m.ImageUrl ?? "", // Nếu ImageUrl null, gán chuỗi rỗng
                    Showtimes = m.Showtimes.Select(s => new ShowtimeDTO
                    {
                        Id = s.Id,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime
                    }).ToList() ?? new List<ShowtimeDTO>() // Ensure Showtimes is not null
                })
                .ToList();
            return Ok(movies);
        }


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
                StartTime = s.StartTime,
                EndTime = s.EndTime
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
        public IActionResult CreateMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi nếu dữ liệu không hợp lệ
            }

            // Nếu Showtimes hoặc MovieReviews không được gửi, đảm bảo không null
            movie.Showtimes ??= new List<Showtime>();
            movie.MovieReviews ??= new List<MovieReview>();

            // Thêm vào database
            _context.Movies.Add(movie);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(long id, [FromBody] JsonElement movieDtoJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm phim trong cơ sở dữ liệu
            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound("Không tìm thấy phim với ID được cung cấp.");
            }

            try
            {
                // Cập nhật các trường dữ liệu từ JSON
                using (var document = JsonDocument.Parse(movieDtoJson.GetRawText()))
                {
                    existingMovie.Name = document.RootElement.GetProperty("name").GetString();
                    existingMovie.Genre = document.RootElement.GetProperty("genre").GetString();
                    existingMovie.Duration = document.RootElement.GetProperty("duration").GetInt32();
                    existingMovie.Description = document.RootElement.GetProperty("description").GetString();
                    existingMovie.Director = document.RootElement.GetProperty("director").GetString();
                    existingMovie.ImageUrl = document.RootElement.GetProperty("imageUrl").GetString();
                }

                _context.Entry(existingMovie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
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