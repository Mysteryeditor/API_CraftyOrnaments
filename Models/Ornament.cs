using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class Ornament
{
    public short OrnamentId { get; set; }

    public string OrnamentName { get; set; } = null!;

    public byte[] OrnamentImage { get; set; } = null!;

    public string? OrnamentDescription { get; set; }

    public decimal? MakingCharge { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
