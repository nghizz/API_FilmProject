using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class MovieReview
{
    public long Id { get; set; }
    [Column("user_id")]
    public long UserId { get; set; }
    [Column("movie_id")]
    public long MovieId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }

    public User User { get; set; }
    public Movie Movie { get; set; }
}