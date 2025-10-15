using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TempQcPo> TempQcPos => Set<TempQcPo>();
    public DbSet<TempQcProduct> TempQcProducts => Set<TempQcProduct>();

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
    }
}