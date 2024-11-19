using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public long Id { get; set; }
    [Column("user_id")]
    public long UserId { get; set; }
    [Column("total_price")]
    public double TotalPrice { get; set; }
    [Column("order_date")]
    public DateTime OrderDate { get; set; }
    [Column("cinema_id")]
    public long CinemaId { get; set; }
    [Column("promotion_id")]
    public long PromotionId { get; set; }

    public User User { get; set; }
    public Cinema Cinema { get; set; }
    public Promotion Promotion { get; set; }
    public ICollection<Refund> Refunds { get; set; }
}