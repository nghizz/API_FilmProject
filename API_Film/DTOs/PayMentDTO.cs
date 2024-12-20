using System.ComponentModel.DataAnnotations.Schema;

namespace API_Film.DTOs
{
    public class PayMentDTO
    {
        public long ShowtimeId { get; set; }
        public long CustomerId { get; set; }
        public long MovieId { get; set; }
        public DateTime DateOrder { get; set; }
        public string Showtime { get; set; }
        public string SeatNumbers { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public long PromotionId { get; set; }
    }
}
