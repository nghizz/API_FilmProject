using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Refund
{
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    public int Quantity { get; set; }
    public string Reason { get; set; }

    [Column("refund_date")]
    public DateTime RefundDate { get; set; }

    public string Status { get; set; }

    // Navigation Properties
    public Order Order { get; set; }
}
