using Microsoft.EntityFrameworkCore;
using API_Film.Models;

namespace API_Film.Data
{
    public class FilmDbContext : DbContext
    {
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
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
            // Cấu hình tên bảng
            modelBuilder.Entity<Cinema>().ToTable("cinemas");
            modelBuilder.Entity<Movie>().ToTable("movies");
            modelBuilder.Entity<TicketType>().ToTable("ticket_types");
            modelBuilder.Entity<Showtime>().ToTable("showtimes");
            modelBuilder.Entity<Seat>().ToTable("seats");
            modelBuilder.Entity<SeatShowtime>().ToTable("seat_showtime");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<MovieReview>().ToTable("movie_reviews");
            modelBuilder.Entity<Promotion>().ToTable("promotions");
            modelBuilder.Entity<Refund>().ToTable("refunds");


            base.OnModelCreating(modelBuilder);

            // Cấu hình các quan hệ giữa các bảng
            modelBuilder.Entity<TicketType>()
                .HasOne(t => t.Cinema)
                .WithMany(c => c.TicketTypes)
                .HasForeignKey(t => t.CinemaId);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Cinema)
                .WithMany(c => c.Promotions)
                .HasForeignKey(p => p.CinemaId);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Cinema)
                .WithMany(c => c.Seats)
                .HasForeignKey(s => s.CinemaId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Showtimes)
                .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Cinema)
                .WithMany(c => c.Showtimes)
                .HasForeignKey(s => s.CinemaId);

            modelBuilder.Entity<SeatShowtime>()
                .HasKey(ss => new { ss.ShowtimeId, ss.SeatId });
            modelBuilder.Entity<SeatShowtime>()
                .HasOne(ss => ss.Showtime)
                .WithMany(s => s.SeatShowtimes)
                .HasForeignKey(ss => ss.ShowtimeId);
            modelBuilder.Entity<SeatShowtime>()
                .HasOne(ss => ss.Seat)
                .WithMany(s => s.SeatShowtimes)
                .HasForeignKey(ss => ss.SeatId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cinema)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CinemaId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Promotion)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PromotionId);

            modelBuilder.Entity<MovieReview>()
                .HasOne(mr => mr.User)
                .WithMany(u => u.MovieReviews)
                .HasForeignKey(mr => mr.UserId);

            modelBuilder.Entity<MovieReview>()
                .HasOne(mr => mr.Movie)
                .WithMany(m => m.MovieReviews)
                .HasForeignKey(mr => mr.MovieId);

            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Order)
                .WithMany(o => o.Refunds)
                .HasForeignKey(r => r.OrderId);
        }
    }
}