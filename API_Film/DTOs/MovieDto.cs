namespace API_Film.DTOs
{
    public class MovieDto
    {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Genre { get; set; }
            public int Duration { get; set; }
            public string Description { get; set; }
            public string Director { get; set; }
            public string ImageUrl { get; set; }
            public List<DateTime> Showtimes { get; set; }
    }
}

