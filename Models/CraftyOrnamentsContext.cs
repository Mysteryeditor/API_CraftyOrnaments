using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_CraftyOrnaments.Models;

public partial class CraftyOrnamentsContext : DbContext
{
    public CraftyOrnamentsContext()
    {
    }

    public CraftyOrnamentsContext(DbContextOptions<CraftyOrnamentsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountProfile> AccountProfiles { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<FinalPayment> FinalPayments { get; set; }

    public virtual DbSet<MetalChoice> MetalChoices { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderInformation> OrderInformations { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Ornament> Ornaments { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<RingSize> RingSizes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<PasswordByte> PasswordBytes { get; set; }

    public virtual DbSet<RoleDeciderResult> AuthenticationDetails { get; set; }
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=SRIK;Database=CraftyOrnaments;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AccountP__1788CC4CAAF5A33E");

            entity.ToTable("AccountProfile");

            entity.HasIndex(e => e.Email, "UQ__AccountP__AB6E61646CB4BB26").IsUnique();

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastLoggedIn).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderCount).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Role).WithMany(p => p.AccountProfiles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__AccountPr__RoleI__286302EC");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFBF5EE82DF");

            entity.ToTable("Address");

            entity.Property(e => e.City)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Line1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Line2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Line3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Address__UserId__3C69FB99");
        });

        modelBuilder.Entity<FinalPayment>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__FinalPay__55433A6B6B55EA9F");

            entity.ToTable("FinalPayment", tb => tb.HasTrigger("completePayment"));

            entity.Property(e => e.AmountPaid).HasColumnType("money");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.IsSuccess)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isSuccess");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modifiedDate");
            entity.Property(e => e.RazorpayOrderId).IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.FinalPayments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__FinalPaym__Order__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.FinalPayments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FinalPaym__UserI__74AE54BC");
        });

        modelBuilder.Entity<MetalChoice>(entity =>
        {
            entity.HasKey(e => e.MetalId).HasName("PK__MetalCho__ACC86EBB4EDB4B40");

            entity.ToTable("MetalChoice");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MarketPrice).HasColumnType("smallmoney");
            entity.Property(e => e.MetalImage).IsUnicode(false);
            entity.Property(e => e.MetalName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PurityGrade)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderDet__C3905BCF0202BC47");

            entity.ToTable("OrderDetail", tb =>
                {
                    tb.HasTrigger("DecrementOrderCount");
                    tb.HasTrigger("IncrementOrderCount");
                });

            entity.Property(e => e.AdvanceAmount).HasColumnType("smallmoney");
            entity.Property(e => e.AdvancePaid).HasDefaultValueSql("((0))");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.FinalWeight).HasColumnName("finalWeight");
            entity.Property(e => e.Finalamount)
                .HasColumnType("money")
                .HasColumnName("finalamount");
            entity.Property(e => e.FullAmountPaid).HasDefaultValueSql("((0))");
            entity.Property(e => e.GeneratedId)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.IsCustomized).HasDefaultValueSql("((0))");
            entity.Property(e => e.Length).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.RemainingAmount).HasColumnType("smallmoney");
            entity.Property(e => e.TotalAmount).HasColumnType("money");
            entity.Property(e => e.Width).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Metal).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MetalId)
                .HasConstraintName("FK__OrderDeta__Metal__60A75C0F");

            entity.HasOne(d => d.Ornament).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrnamentId)
                .HasConstraintName("FK__OrderDeta__Ornam__619B8048");

            entity.HasOne(d => d.SizeNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Size)
                .HasConstraintName("FK__OrderDetai__Size__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OrderDeta__UserI__5FB337D6");
        });

        modelBuilder.Entity<OrderInformation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OrderInformation");

            entity.Property(e => e.Advanceamount)
                .HasColumnType("smallmoney")
                .HasColumnName("advanceamount");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("dueDate");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FinalWeight).HasColumnName("finalWeight");
            entity.Property(e => e.Finalamount)
                .HasColumnType("money")
                .HasColumnName("finalamount");
            entity.Property(e => e.FullamountPaid).HasColumnName("fullamountPaid");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Iscustomized).HasColumnName("iscustomized");
            entity.Property(e => e.Length)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("length");
            entity.Property(e => e.MetalName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("metalName");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.OrnamentName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ornamentName");
            entity.Property(e => e.Remainingamount)
                .HasColumnType("smallmoney")
                .HasColumnName("remainingamount");
            entity.Property(e => e.Size).HasColumnName("size");
            entity.Property(e => e.SizeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("sizeName");
            entity.Property(e => e.SizeValue).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Totalamount)
                .HasColumnType("money")
                .HasColumnName("totalamount");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserName)
                .HasMaxLength(61)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.Width)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("width");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__C8EE2063DA6DD948");

            entity.ToTable("OrderStatus");

            entity.HasIndex(e => e.StatusName, "UQ__OrderSta__05E7698ACDC3047C").IsUnique();

            entity.Property(e => e.StatusId).ValueGeneratedOnAdd();
            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ornament>(entity =>
        {
            entity.HasKey(e => e.OrnamentId).HasName("PK__Ornament__7F5317C114AF674F");

            entity.ToTable("Ornament");

            entity.HasIndex(e => e.OrnamentName, "UQ__Ornament__903D90DE1178D556").IsUnique();

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MakingCharge).HasColumnType("smallmoney");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrnamentDescription).IsUnicode(false);
            entity.Property(e => e.OrnamentImage)
                .IsUnicode(false)
                .HasColumnName("ornamentImage");
            entity.Property(e => e.OrnamentName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.OtpId).HasName("PK__OTP__122D946A8200164B");

            entity.ToTable("OTP");

            entity.Property(e => e.OtpId).HasColumnName("otpId");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiryDate");
            entity.Property(e => e.OtpValue)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("otpValue");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Otps)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OTP__userId__7F2BE32F");
        });

        modelBuilder.Entity<RingSize>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__RingSize__83BD097A6BFBBF15");

            entity.ToTable("RingSize");

            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modifiedDate");
            entity.Property(e => e.SizeName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SizeValue).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A7C6861A7");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
