using Csira.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Csira.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public static Guid SeedIssueId => Guid.Parse("e89e4a8f-7bea-4db4-b8f5-7a13df7c7d01");

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

            entity.HasData(new IssueEntity
            {
                Id = SeedIssueId,
                Name = "Set up initial Jira-like issue tracker scaffold",
                Description = "Create a 3-layer architecture with Razor Pages web layer, service layer DTO mapping, and EF Core data access layer.",
                Priority = IssuePriority.Medium
            });
        });
    }
}