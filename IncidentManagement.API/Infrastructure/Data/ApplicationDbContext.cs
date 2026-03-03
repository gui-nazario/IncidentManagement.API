using Microsoft.EntityFrameworkCore;
using IncidentManagement.API.Domain.Entities;

namespace IncidentManagement.API.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Store> Stores => Set<Store>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Store>(entity =>

        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(s => s.Brand)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.City)
                  .HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(u => u.Username)
                  .IsUnique();

            entity.Property(u => u.PasswordHash)
                  .IsRequired();

            entity.Property(u => u.Role)
                  .IsRequired()
                  .HasMaxLength(50);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Token)
                  .IsRequired();

            entity.HasOne(r => r.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<StoreFinancial>()
    .HasOne(sf => sf.Store)
    .WithMany(s => s.Financials)
    .HasForeignKey(sf => sf.StoreId);

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Action)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(a => a.PerformedBy)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(a => a.Timestamp)
                  .IsRequired();
        });

        modelBuilder.Entity<AuditLog>()
    .Property(a => a.Source)
    .HasConversion<int>();

        modelBuilder.Entity<PaymentMethod>().HasData(
    new PaymentMethod { Id = 1, Name = "Pix" },
    new PaymentMethod { Id = 2, Name = "Credit Card" },
    new PaymentMethod { Id = 3, Name = "Debit Card" },
    new PaymentMethod { Id = 4, Name = "Boleto" }
);

        modelBuilder.Entity<PurchaseChannel>().HasData(
            new PurchaseChannel { Id = 1, Name = "App" },
            new PurchaseChannel { Id = 2, Name = "Website" },
            new PurchaseChannel { Id = 3, Name = "Physical Store" }
        );

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Pending" },
            new OrderStatus { Id = 2, Name = "Paid" },
            new OrderStatus { Id = 3, Name = "Cancelled" },
            new OrderStatus { Id = 4, Name = "Refunded" }
        );

    }


    public DbSet<StoreFinancial> StoreFinancials { get; set; }
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<PurchaseChannel> PurchaseChannels { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }



}