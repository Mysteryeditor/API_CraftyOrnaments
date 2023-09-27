using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class MetalChoice
{
    public short MetalId { get; set; }

    public string? MetalName { get; set; }

    public decimal? MarketPrice { get; set; }

    public string? PurityGrade { get; set; }

    public byte[]? MetalImage { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
