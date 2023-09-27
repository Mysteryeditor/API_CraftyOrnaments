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

    public virtual DbSet<MetalChoice> MetalChoices { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Ornament> Ornaments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<AuthenticationDetails> AuthenticationDetails { get; set; }
    public virtual DbSet<PasswordByte> PasswordBytes { get; set; }


    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=SRIK;Database=CraftyOrnaments;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AccountP__1788CC4CAAF5A33E");

            entity.ToTable("AccountProfile");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
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

        modelBuilder.Entity<MetalChoice>(entity =>
        {
            entity.HasKey(e => e.MetalId).HasName("PK__MetalCho__ACC86EBB4EDB4B40");

            entity.ToTable("MetalChoice");

            entity.Property(e => e.MarketPrice).HasColumnType("smallmoney");
            entity.Property(e => e.MetalName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PurityGrade)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderDet__C3905BCF09DF04B2");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.AdvanceAmount).HasColumnType("smallmoney");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IsCustomized).HasDefaultValueSql("((0))");
            entity.Property(e => e.RemainingAmount).HasColumnType("smallmoney");
            entity.Property(e => e.TotalAmount).HasColumnType("money");

            entity.HasOne(d => d.Metal).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MetalId)
                .HasConstraintName("FK__OrderDeta__Metal__36B12243");

            entity.HasOne(d => d.Ornament).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrnamentId)
                .HasConstraintName("FK__OrderDeta__Ornam__37A5467C");

            entity.HasOne(d => d.User).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OrderDeta__UserI__35BCFE0A");
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
            entity.Property(e => e.OrnamentName)
                .HasMaxLength(20)
                .IsUnicode(false);
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
