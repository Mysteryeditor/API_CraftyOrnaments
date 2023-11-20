using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public string? GeneratedId { get; set; }

    public int? UserId { get; set; }

    public short? MetalId { get; set; }

    public short? OrnamentId { get; set; }

    public double? Weight { get; set; }

    public byte? Quantity { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public byte? StatusId { get; set; }

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public short? Size { get; set; }

    public decimal? AdvanceAmount { get; set; }

    public decimal? RemainingAmount { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool? IsCustomized { get; set; }

    public bool? AdvancePaid { get; set; }

    public bool? FullAmountPaid { get; set; }

    public byte[]? Image { get; set; }

    public decimal? Finalamount { get; set; }

    public double? FinalWeight { get; set; }

    public virtual ICollection<FinalPayment> FinalPayments { get; set; } = new List<FinalPayment>();

    public virtual MetalChoice? Metal { get; set; }

    public virtual Ornament? Ornament { get; set; }

    public virtual RingSize? SizeNavigation { get; set; }

    public virtual AccountProfile? User { get; set; }
}
