using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class AccountProfile
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public long? PhoneNumber { get; set; }

    public byte[]? Password { get; set; }

    public byte? RoleId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? LastLoggedIn { get; set; }

    public bool? IsDeleted { get; set; }

    public short? OrderCount { get; set; }

    public byte[]? ProfilePic { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<FinalPayment> FinalPayments { get; set; } = new List<FinalPayment>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Otp> Otps { get; set; } = new List<Otp>();

    public virtual Role? Role { get; set; }
}
