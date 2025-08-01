using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext : DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Customer>(cb => {
            cb.ToTable("Customer");
            cb.HasKey(c => c.Id);
            cb.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
            cb.Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired();
            cb.Property(c => c.PhoneNumber)
                .HasMaxLength(15);
        });

        modelBuilder.Entity<Product>(eb =>
        {
            eb.ToTable("Product");
            eb.HasKey(p => p.Id);
            eb.Property(p => p.Sku)
                .HasMaxLength(20)
                .IsRequired();
            eb.Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();
            eb.Property(p => p.CurrentUnitPrice)
                .HasPrecision(15, 2);
            eb.Property(p => p.InternalCode)
                .HasMaxLength(10);
            eb.Property(p => p.Description)
                .HasMaxLength(200);
            eb.Property(p => p.StockQuantity)
                .HasDefaultValue(0);
            eb.HasCheckConstraint("CK_Product_CurrentUnitPrice_Positive", "[CurrentUnitPrice] > 0");
            eb.HasCheckConstraint("CK_Product_Stock_NonNegative", "[StockQuantity] >= 0");

            eb.HasIndex(p => p.Sku).IsUnique();
        });

        modelBuilder.Entity<Order>(od => {
            od.ToTable("Order");
            od.HasKey(o => o.Id);
            od.Property(o => o.ShippingAddress)
                .HasMaxLength(200);
            od.Property(o => o.BillingAddress)
                .HasMaxLength(200);
            od.Property(o => o.Notes)
                .HasMaxLength(500);
            od.Property(o => o.TotalAmount)
                .HasPrecision(15, 2);
            od.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Pending);

            od.HasOne(o => o.Customer)
              .WithMany(c => c.Orders)
              .HasForeignKey(o => o.CustomerId)
              .OnDelete(DeleteBehavior.Restrict); // No eliminar cliente si tiene órdenes
        });

        modelBuilder.Entity<OrderItem>(eb =>
        {
            eb.ToTable("OrderItem");
            eb.HasKey(oi => oi.Id);
            eb.Property(oi => oi.Quantity)
                .HasDefaultValue(0);
            eb.Property(oi => oi.UnitPrice)
                .HasPrecision(15, 2);
            eb.Property(oi => oi.Subtotal)
                .HasPrecision(15, 2);

            eb.HasOne(oi => oi.Order)
              .WithMany(o => o.Items)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade); // Si se elimina orden, eliminar items

            eb.HasOne(oi => oi.Product)
              .WithMany(p => p.Items)
              .HasForeignKey(oi => oi.ProductId)
              .OnDelete(DeleteBehavior.Restrict); // No eliminar producto si está en órdenes
        });
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
}