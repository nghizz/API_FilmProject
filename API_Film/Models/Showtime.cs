using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Showtime
{
    public long Id { get; set; }
    [Column("movie_id")]
    public long MovieId { get; set; }
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Column("end_time")]
    public DateTime EndTime { get; set; }

    public Movie Movie { get; set; }
    public ICollection<SeatShowtime> SeatShowtimes { get; set; }
}