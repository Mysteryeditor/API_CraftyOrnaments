using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class RingSize
{
    public short SizeId { get; set; }

    public string? SizeName { get; set; }

    public decimal? SizeValue { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
