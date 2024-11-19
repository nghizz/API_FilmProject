using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Refund
{
    public long Id { get; set; }
    [Column("order_id")]
    public long OrderId { get; set; }
    [Column("ticket_type_id")]
    public long TicketTypeId { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; }
    [Column("refund_date")]
    public DateTime RefundDate { get; set; }
    public string Status { get; set; }

    public Order Order { get; set; }
    public TicketType TicketType { get; set; }
}