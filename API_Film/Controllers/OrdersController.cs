// OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public OrdersController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(_context.Orders.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(long id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(long id, Order order)
        {
            if (id != order.Id) return BadRequest();
            _context.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(long id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }
    }
}