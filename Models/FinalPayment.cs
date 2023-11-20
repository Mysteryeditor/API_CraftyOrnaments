using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class FinalPayment
{
    public int TransactionId { get; set; }

    public int? OrderId { get; set; }

    public string? RazorpayOrderId { get; set; }

    public int? UserId { get; set; }

    public decimal? AmountPaid { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsSuccess { get; set; }

    public virtual OrderDetail? Order { get; set; }

    public virtual AccountProfile? User { get; set; }
}
