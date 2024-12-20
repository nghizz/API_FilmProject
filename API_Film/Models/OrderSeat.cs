using System.ComponentModel.DataAnnotations.Schema;

namespace API_Film.Models
{
    public class OrderSeat
    {
        [Column("order_id")]
        public long OrderId { get; set; }

        [Column("seat_id")]
        public long SeatId { get; set; }

        public Order Order { get; set; }
        public SeatShowtime SeatShowtime { get; set; }
    }
}
