using Csira.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Csira.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<IssueEntity> Issues => Set<IssueEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IssueEntity>(entity =>
        {
            entity.ToTable("Issues");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired();

            entity.Property(x => x.Priority)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });
    }
}
