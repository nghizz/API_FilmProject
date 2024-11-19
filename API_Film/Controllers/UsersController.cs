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

            var user = _context.Users
                               .FirstOrDefault(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng." });
            }

            return Ok(new { message = "Đăng nhập thành công", user = new { user.Id, user.Username, user.Role } });
        }


    }
}
