using System;
using System.Collections.Generic;
using BurgerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BurgerApi.Data;

public partial class BurgerContext : DbContext
{
    public BurgerContext() { }

    public BurgerContext(DbContextOptions<BurgerContext> options)
        : base(options) { }

    public virtual DbSet<Establishment> Establishments { get; set; }

    public virtual DbSet<EstablishmentProduct> EstablishmentProducts { get; set; }

    public virtual DbSet<OpeningTime> OpeningTimes { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<HistoryEmailSend> HistoryEmailSends { get; set; }
    public virtual DbSet<HistoryTokens> HistoryTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Host=localhost;Database=burger;Username=postgres;Password=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.HasKey(e => e.EstablishmentId).HasName("establishments_pkey");

            entity.ToTable("establishments");

            entity
                .Property(e => e.EstablishmentId)
                .ValueGeneratedNever()
                .HasColumnName("establishment_id");
            entity.Property(e => e.Address).HasMaxLength(100).HasColumnName("address");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
            entity.Property(e => e.Ownerid).HasColumnName("ownerid");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15).HasColumnName("phone_number");

            entity
                .HasOne(d => d.Owner)
                .WithMany(p => p.Establishments)
                .HasForeignKey(d => d.Ownerid)
                .HasConstraintName("establishments_ownerid_fkey");
        });

        modelBuilder.Entity<EstablishmentProduct>(entity =>
        {
            entity.HasKey(e => e.EstablishmentProductId).HasName("establishment_products_pkey");

            entity.ToTable("establishment_products");

            entity
                .Property(e => e.EstablishmentProductId)
                .ValueGeneratedNever()
                .HasColumnName("establishment_product_id");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.Category).HasMaxLength(50).HasColumnName("category");
            entity.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
            entity.Property(e => e.Price).HasPrecision(10, 2).HasColumnName("price");

            entity
                .HasOne(d => d.Establishment)
                .WithMany(p => p.EstablishmentProducts)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("establishment_products_establishment_id_fkey");
        });

        modelBuilder.Entity<OpeningTime>(entity =>
        {
            entity.HasKey(e => e.OpeningTimeId).HasName("opening_times_pkey");

            entity.ToTable("opening_times");

            entity
                .Property(e => e.OpeningTimeId)
                .ValueGeneratedNever()
                .HasColumnName("opening_time_id");
            entity.Property(e => e.CloseTime).HasColumnName("close_time");
            entity.Property(e => e.DayOfWeek).HasMaxLength(10).HasColumnName("day_of_week");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.OpenTime).HasColumnName("open_time");

            entity
                .HasOne(d => d.Establishment)
                .WithMany(p => p.OpeningTimes)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("opening_times_establishment_id_fkey");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("order_details_pkey");

            entity.ToTable("order_details");

            entity.Property(e => e.OrderId).ValueGeneratedNever().HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Subtotal).HasPrecision(10, 2).HasColumnName("subtotal");

            entity
                .HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("order_details_product_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.PaymentId).ValueGeneratedNever().HasColumnName("payment_id");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod).HasMaxLength(20).HasColumnName("payment_method");
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2).HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("payments_user_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).ValueGeneratedNever().HasColumnName("product_id");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.Category).HasMaxLength(50).HasColumnName("category");
            entity.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
            entity.Property(e => e.Price).HasPrecision(10, 2).HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever().HasColumnName("user_id");
            entity.Property(e => e.Address).HasMaxLength(100).HasColumnName("address");
            entity.Property(e => e.Email).HasMaxLength(50).HasColumnName("email");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasMaxLength(100).HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15).HasColumnName("phone_number");
        });

        modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.Entity<User>().Property(b => b.UserId).HasIdentityOptions(startValue: 1);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
