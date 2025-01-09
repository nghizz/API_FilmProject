using API_Film.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Film.Data
{
    public class FilmDbContext : DbContext
    {
        public DbSet<OrderSeat> OrderSeats { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<SeatShowtime> SeatShowtimes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MovieReview> MovieReviews { get; set; }
        public DbSet<Refund> Refunds { get; set; }

        public FilmDbContext(DbContextOptions<FilmDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình tên bảng (optional if your class names match table names)
            modelBuilder.Entity<OrderSeat>().ToTable("order_seat");
            modelBuilder.Entity<Movie>().ToTable("movies");
            modelBuilder.Entity<SeatType>().ToTable("seat_types");
            modelBuilder.Entity<Showtime>().ToTable("showtimes");
            modelBuilder.Entity<Seat>().ToTable("seats");
            modelBuilder.Entity<SeatShowtime>().ToTable("seat_showtime");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<MovieReview>().ToTable("movie_reviews");
            modelBuilder.Entity<Promotion>().ToTable("promotions");
            modelBuilder.Entity<Refund>().ToTable("refunds");

            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<OrderSeat>()
                .HasKey(os => new { os.OrderId, os.SeatId });

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.SeatType)
                .WithMany(st => st.Seats)
                .HasForeignKey(s => s.SeatTypeId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Showtimes)
                .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<SeatShowtime>()
                .HasKey(ss => new { ss.ShowtimeId, ss.SeatId, ss.MovieId });

            modelBuilder.Entity<SeatShowtime>()
                .HasOne(ss => ss.Showtime)
                .WithMany(s => s.SeatShowtimes)
                .HasForeignKey(ss => ss.ShowtimeId);

            modelBuilder.Entity<SeatShowtime>()
                .HasOne(ss => ss.Seat)
                .WithMany(s => s.SeatShowtimes)
                .HasForeignKey(ss => ss.SeatId);

        }
    }
}