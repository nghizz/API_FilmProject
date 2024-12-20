﻿using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Movie
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }
    public string Description { get; set; }
    [Column("trailer_url")]
    public string TrailerUrl { get; set; }
    public string Director { get; set; }
    [Column("image_url")]
    public string ImageUrl { get; set; }

    public ICollection<Showtime> Showtimes { get; set; }
    public ICollection<MovieReview> MovieReviews { get; set; }
    public List<SeatShowtime> SeatShowtimes { get; set; }
}