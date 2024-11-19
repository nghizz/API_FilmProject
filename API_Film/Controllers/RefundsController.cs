// RefundsController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundsController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public RefundsController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllRefunds()
        {
            return Ok(_context.Refunds.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetRefundById(long id)
        {
            var refund = _context.Refunds.Find(id);
            if (refund == null) return NotFound();
            return Ok(refund);
        }

        [HttpPost]
        public IActionResult CreateRefund(Refund refund)
        {
            _context.Refunds.Add(refund);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetRefundById), new { id = refund.Id }, refund);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRefund(long id)
        {
            var refund = _context.Refunds.Find(id);
            if (refund == null) return NotFound();
            _context.Refunds.Remove(refund);
            _context.SaveChanges();
            return NoContent();
        }
    }
}