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
    }
    public DbSet<StoreFinancial> StoreFinancials { get; set; }
}