using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class SeatShowtime
{
    [Column("showtime_id")]
    public long ShowtimeId { get; set; }


    [Column("seat_id")]
    public long SeatId { get; set; }


    [Column("movie_id")]
    public long MovieId { get; set; }

    public Showtime Showtime { get; set; }
    public Seat Seat { get; set; }
    public Movie Movie { get; set; }
}