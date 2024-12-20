// OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using API_Film.Models;
using API_Film.Data;
using API_Film.DTOs;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.User)
                                        .Include(o => o.Promotion)
                                        .Include(o => o.Movie)  // Nếu bạn muốn include Movie
                                        .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<Order> GetOrderByIdAsync(long id)
        {
            return await _context.Orders.Include(o => o.User)
                                        .Include(o => o.Promotion)
                                        .Include(o => o.Movie)  // Nếu bạn muốn include Movie
                                        .FirstOrDefaultAsync(o => o.Id == id);
        }

        private List<long> GetSeatIdFromSeatNumber(string seatNumbers)
        {
            var seatIds = new List<long>();
            foreach (var seatNumber in seatNumbers.Split(','))
            {
                // Tách row và seatNumber theo dấu "-"
                var parts = seatNumber.Trim().Split('-');
                if (parts.Length != 2)
                {
                    throw new Exception($"Dữ liệu ghế không hợp lệ: {seatNumber}");
                }

                // Lấy phần row và seatNumber từ chuỗi đã tách
                int row = int.Parse(parts[0]);     // row là số (ví dụ 1, 2)
                int number = int.Parse(parts[1]); // seatNumber là số ghế (ví dụ 2, 1)

                // Tìm seat_id trong database dựa trên row (int) và number (int)
                var seat = _context.Seats.FirstOrDefault(s => s.RowNumber == row && s.SeatNumber == number);
                if (seat != null)
                {
                    seatIds.Add(seat.Id);
                }
                else
                {
                    throw new Exception($"Không tìm thấy ghế có số {seatNumber}");
                }
            }
            return seatIds;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] PayMentDTO paymentDto)
        {
            // Kiểm tra mã khuyến mãi
            /*var promotion = _context.Promotions.Find(paymentDto.PromotionId);
            if (promotion == null)
            {
                return BadRequest(new { Message = "Mã khuyến mãi không tồn tại!" });
            }*/

            // Tạo đơn hàng
            var order = new Order
            {
                UserId = paymentDto.CustomerId,
                TotalPrice = paymentDto.TotalAmount,
                OrderDate = paymentDto.DateOrder,
                PaymentStatus = paymentDto.PaymentStatus,
                PaymentMethod = paymentDto.PaymentMethod,
                TransactionId = Guid.NewGuid().ToString(),
                PromotionId = paymentDto.PromotionId,
                MovieId = paymentDto.MovieId
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            var seatIds = GetSeatIdFromSeatNumber(paymentDto.SeatNumbers);

            foreach (var seatId in seatIds)
            {
                var seatShowtime = new SeatShowtime
                {
                    ShowtimeId = paymentDto.ShowtimeId,
                    SeatId = seatId,
                    MovieId = paymentDto.MovieId
                };
                _context.SeatShowtimes.Add(seatShowtime);

                var orderSeat = new OrderSeat
                {
                    OrderId = order.Id,
                    SeatId = seatId
                };
                _context.OrderSeats.Add(orderSeat);
            }
            _context.SaveChanges();

            return Ok(new { Message = "Đơn hàng đã được tạo thành công!" });
        }


        [HttpPut("{id}")]
        public async Task<Order> UpdateOrderAsync(long id, Order order)
        {
            if (id != order.Id)
                return null;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrderAsync(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}