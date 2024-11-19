namespace API_Film.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // Navigation Properties không cần thiết khi đăng nhập
        public ICollection<Order> Orders { get; set; }
        public ICollection<MovieReview> MovieReviews { get; set; }
    }
}
