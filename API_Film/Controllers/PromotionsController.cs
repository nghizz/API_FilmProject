// PromotionsController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public PromotionsController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllPromotions()
        {
            return Ok(_context.Promotions.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetPromotionById(long id)
        {
            var promotion = _context.Promotions.Find(id);
            if (promotion == null) return NotFound();
            return Ok(promotion);
        }

        [HttpPost]
        public IActionResult CreatePromotion(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPromotionById), new { id = promotion.Id }, promotion);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePromotion(long id, Promotion promotion)
        {
            if (id != promotion.Id) return BadRequest();
            _context.Entry(promotion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePromotion(long id)
        {
            var promotion = _context.Promotions.Find(id);
            if (promotion == null) return NotFound();
            _context.Promotions.Remove(promotion);
            _context.SaveChanges();
            return NoContent();
        }
    }
}