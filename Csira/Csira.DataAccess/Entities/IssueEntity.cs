using System.ComponentModel.DataAnnotations;

namespace Csira.DataAccess.Entities;

public class IssueEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    public IssuePriority Priority { get; set; }
}
