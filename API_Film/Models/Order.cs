using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Film.Models
{
    public class Order
    {
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("total_price")]
        public double TotalPrice { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        [Column("promotion_id")]
        public long PromotionId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }

        [Column("id_movie")]
        public long? MovieId { get; set; }  // Correct column mapping

        public User User { get; set; }
        public Promotion Promotion { get; set; }
        public Movie Movie { get; set; }
        public ICollection<Refund> Refunds { get; set; }
    }
}
