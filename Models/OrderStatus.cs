using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class OrderStatus
{
    public byte StatusId { get; set; }

    public string? StatusName { get; set; }
}
