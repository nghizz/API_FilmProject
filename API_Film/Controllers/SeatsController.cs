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

        // GET: api/Seats/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAvailableSeats()
        {
            var availableSeats = await _context.Seats
                .Include(s => s.SeatType)
                .Select(s => new
                {
                    s.Id,
                    s.RowNumber,
                    s.SeatNumber,
                    s.IsAvailable,
                    SeatType = s.SeatType.TypeName,
                    Price = s.SeatType.Price
                })
                .Where(s => s.IsAvailable)
                .ToListAsync();

            return Ok(availableSeats); // Trả về mảng trực tiếp, không bọc trong $values
        }

        // GET: api/Seats/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<SeatType>>> GetSeatTypes()
        {
            var seatTypes = await _context.SeatTypes.ToListAsync();
            return Ok(seatTypes);
        }

        // POST: api/Seats/reserve
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeats([FromBody] List<long> seatIds)
        {
            var seats = await _context.Seats.Where(s => seatIds.Contains(s.Id)).ToListAsync();
            if (!seats.Any()) return NotFound("Không tìm thấy ghế.");

            foreach (var seat in seats)
            {
                seat.IsAvailable = false; // Set seat as reserved
            }

            await _context.SaveChangesAsync();
            return Ok("Ghế đã được cập nhật trạng thái.");
        }
    }
}
