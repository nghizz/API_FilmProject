using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

public class SeatType
{
    public long Id { get; set; }

    [Column("name")]
    public string TypeName { get; set; } // Ví dụ: VIP, Standard, Economy

    [Column("description")]
    public string Description { get; set; }

    [Column("price")]
    public double Price { get; set; }

    // Navigation Properties
    public ICollection<Seat> Seats { get; set; }
}
