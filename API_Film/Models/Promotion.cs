using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Promotion
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [Column("image_url")]
    public string ImageUrl { get; set; }
    public double Discount { get; set; }
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    [Column("minimum_amount")]
    public double MinimumAmount { get; set; }
    [Column("cinema_id")]
    public long CinemaId { get; set; }

    public Cinema Cinema { get; set; }
    public ICollection<Order> Orders { get; set; }
}