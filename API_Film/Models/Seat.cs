using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Seat
{
    public long Id { get; set; }
    [Column("cinema_id")]
    public long CinemaId { get; set; }
    [Column("row_number")]
    public int RowNumber { get; set; }
    [Column("seat_number")]
    public int SeatNumber { get; set; }
    [Column("is_available")]
    public bool IsAvailable { get; set; }

    public Cinema Cinema { get; set; }
    public ICollection<SeatShowtime> SeatShowtimes { get; set; }
}