using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class OrderInformation
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public int OrderId { get; set; }

    public string? MetalName { get; set; }

    public double? Weight { get; set; }

    public string OrnamentName { get; set; } = null!;

    public byte[]? Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public short? Size { get; set; }

    public string? SizeName { get; set; }

    public decimal? SizeValue { get; set; }

    public double? FinalWeight { get; set; }

    public decimal? Advanceamount { get; set; }

    public decimal? Remainingamount { get; set; }

    public decimal? Totalamount { get; set; }

    public decimal? Finalamount { get; set; }

    public bool? Iscustomized { get; set; }

    public bool? FullamountPaid { get; set; }

    public byte StatusId { get; set; }

    public string? StatusName { get; set; }
}
