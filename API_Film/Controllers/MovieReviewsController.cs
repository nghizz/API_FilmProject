using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieReviewsController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public MovieReviewsController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieReviews()
        {
            var reviews = await _context.MovieReviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .ToListAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieReviewById(long id)
        {
            var review = await _context.MovieReviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieReview(MovieReview review)
        {
            _context.MovieReviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovieReviewById), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieReview(long id, MovieReview review)
        {
            if (id != review.Id)
                return BadRequest();

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieReviewExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieReview(long id)
        {
            var review = await _context.MovieReviews.FindAsync(id);
            if (review == null)
                return NotFound();

            _context.MovieReviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieReviewExists(long id)
        {
            return _context.MovieReviews.Any(e => e.Id == id);
        }
    }
}