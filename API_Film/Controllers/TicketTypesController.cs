// TicketTypesController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public TicketTypesController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllTicketTypes()
        {
            return Ok(_context.TicketTypes.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetTicketTypeById(long id)
        {
            var ticketType = _context.TicketTypes.Find(id);
            if (ticketType == null) return NotFound();
            return Ok(ticketType);
        }

        [HttpPost]
        public IActionResult CreateTicketType(TicketType ticketType)
        {
            _context.TicketTypes.Add(ticketType);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTicketTypeById), new { id = ticketType.Id }, ticketType);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTicketType(long id, TicketType ticketType)
        {
            if (id != ticketType.Id) return BadRequest();
            _context.Entry(ticketType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicketType(long id)
        {
            var ticketType = _context.TicketTypes.Find(id);
            if (ticketType == null) return NotFound();
            _context.TicketTypes.Remove(ticketType);
            _context.SaveChanges();
            return NoContent();
        }
    }
}