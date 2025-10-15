using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TempQcPo> TempQcPos => Set<TempQcPo>();
    public DbSet<TempQcProduct> TempQcProducts => Set<TempQcProduct>();
    public DbSet<VendorThreshold> VendorThresholds => Set<VendorThreshold>();
    public DbSet<OverrideApproval> OverrideApprovals => Set<OverrideApproval>();
    public DbSet<TempQcUnloadedBy> TempQcUnloadedBys => Set<TempQcUnloadedBy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TempQcPo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PoNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Vendor).HasMaxLength(100).IsRequired();

            entity.HasMany(e => e.Products)
                  .WithOne(p => p.TempQcPo)
                  .HasForeignKey(p => p.TempQcPoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TempQcProduct>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<VendorThreshold>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Vendor).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Sku).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(20).IsRequired();

            entity.HasIndex(e => new { e.Vendor, e.Sku, e.Position }).IsUnique();
        });

        modelBuilder.Entity<OverrideApproval>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PinLast4).HasMaxLength(10).IsRequired();

            entity.HasOne(e => e.TempQcProduct)
                  .WithMany()
                  .HasForeignKey(e => e.TempQcProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TempQcUnloadedBy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnloadedBy).HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.TempQcPo)
                  .WithMany()
                  .HasForeignKey(e => e.TempQcPoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}