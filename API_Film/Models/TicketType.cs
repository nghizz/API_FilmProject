using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class TicketType
{
    public long Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    [Column("cinema_id")]
    public long CinemaId { get; set; }

    public Cinema Cinema { get; set; }
}