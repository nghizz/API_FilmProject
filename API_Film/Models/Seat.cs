﻿using API_Film.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Seat
{
    [Column("id")]
    public long Id { get; set; }

    [Column("row_number")]
    public int RowNumber { get; set; }

    [Column("seat_number")]
    public int SeatNumber { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; }

    [Column("seat_type_id")]
    public long SeatTypeId { get; set; } // Thêm thuộc tính cho SeatType

    // Navigation Properties
    public SeatType SeatType { get; set; } // Thêm Navigation Property
    public ICollection<SeatShowtime> SeatShowtimes { get; set; }
}
