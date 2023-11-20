using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class Otp
{
    public int OtpId { get; set; }

    public string OtpValue { get; set; } = null!;

    public int? UserId { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public virtual AccountProfile? User { get; set; }
}
