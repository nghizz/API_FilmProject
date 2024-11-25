using System.ComponentModel.DataAnnotations.Schema;

namespace API_Film.Models
{
    public class Cinema
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        [Column("image_url")] // Thêm attribute này
        public string ImageUrl { get; set; }

        [Column("support_info")] // Thêm attribute này
        public string SupportInfo { get; set; }

        [Column("user_rating")] // Thêm attribute này
        public double UserRating { get; set; }

        // Navigation Properties
        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
