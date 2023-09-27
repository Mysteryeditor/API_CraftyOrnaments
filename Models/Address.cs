using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? Line3 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? Pincode { get; set; }

    public string? Country { get; set; }

    public int? UserId { get; set; }

    public virtual AccountProfile? User { get; set; }
}
