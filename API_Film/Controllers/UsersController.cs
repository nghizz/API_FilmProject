using API_Film.Models;
using API_Film.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequestIdentity = Microsoft.AspNetCore.Identity.Data.LoginRequest;
using LoginRequestAPI = API_Film.Models.LoginRequest;

namespace API_Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public UsersController(FilmDbContext context)
        {
            _context = context;
        }

        // API lấy tất cả người dùng
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_context.Users.ToList());
        }

        // API lấy người dùng theo ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(long id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // API tạo mới người dùng
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // API cập nhật người dùng
        [HttpPut("{id}")]
        public IActionResult UpdateUser(long id, User user)
        {
            if (id != user.Id) return BadRequest();
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        // API xóa người dùng
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        // API đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestAPI loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ username và password." });
            }

            // Tìm tài khoản theo username
            var user = _context.Users.FirstOrDefault(u => u.Username == loginRequest.Username);

            if (user == null)
            {
                // Không tìm thấy tài khoản
                return Unauthorized(new { message = "Tài khoản không tồn tại." });
            }

            // Kiểm tra mật khẩu
            if (user.Password != loginRequest.Password)
            {
                // Sai mật khẩu
                return Unauthorized(new { message = "Mật khẩu không chính xác." });
            }

            // Đăng nhập thành công
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            return Ok(new
            {
                message = "Đăng nhập thành công",
                user = new { user.Id, user.Username, user.Role }
            });
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetPromotionHistory(long userId)
        {
            var history = await _context.Orders
                .Where(o => o.UserId == userId && o.PromotionId != null)
                .Join(
                    _context.Promotions,
                    order => order.PromotionId,
                    promotion => promotion.Id,
                    (order, promotion) => new
                    {
                        OrderId = order.Id,
                        PromotionName = promotion.Name,
                        PromotionDescription = promotion.Description,
                        DiscountPercentage = promotion.Discount,
                        UsedDate = order.OrderDate,
                        StartDate = promotion.StartDate,
                        EndDate = promotion.EndDate
                    }
                )
                .ToListAsync();

            if (history.Count == 0)
            {
                return NotFound(new { Message = "Người dùng chưa sử dụng khuyến mãi nào." });
            }

            return Ok(history);
        }
    }
}

