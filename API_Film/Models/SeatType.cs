using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

public class SeatType
{
    public long Id { get; set; }

    [Column("type_name")]
    public string TypeName { get; set; } // Ví dụ: VIP, Standard, Economy

    [Column("description")]
    public string Description { get; set; }

    [Column("price_multiplier")]
    public double PriceMultiplier { get; set; } // Hệ số giá (VD: 1.5x cho VIP)

    // Navigation Properties
    public ICollection<Seat> Seats { get; set; }
}
