using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Models;

namespace WorkOrderApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Title).IsRequired().HasMaxLength(200);
            entity.Property(w => w.Description).HasMaxLength(2000);
            entity.Property(w => w.AssignedTo).IsRequired().HasMaxLength(100);
            entity.Property(w => w.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasIndex(w => w.Status);
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Action).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Details).HasMaxLength(2000);
            entity.Property(a => a.PerformedBy).IsRequired().HasMaxLength(100);
            entity.HasOne(a => a.WorkOrder)
                  .WithMany(w => w.Activities)
                  .HasForeignKey(a => a.WorkOrderId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(a => new { a.WorkOrderId, a.Timestamp });
        });
    }
}
