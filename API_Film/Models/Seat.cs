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

    [Column("seat_type_id")]
    public long SeatTypeId { get; set; } // Thêm thuộc tính cho SeatType

    // Navigation Properties
    public Cinema Cinema { get; set; }
    public SeatType SeatType { get; set; } // Thêm Navigation Property
    public ICollection<SeatShowtime> SeatShowtimes { get; set; }
}
