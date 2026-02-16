using Csira.DataAccess;
using Csira.DataAccess.Entities;
using Csira.Services.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Csira.Services.Issues;

public class IssueService(AppDbContext dbContext) : IIssueService
{
    public async Task<IReadOnlyList<IssueDto>> GetIssuesAsync(CancellationToken cancellationToken = default)
    {
        var issues = await dbContext.Issues
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return issues.Select(MapIssue).ToList();
    }

    public async Task<IssueDto?> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var issue = await dbContext.Issues
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (issue is null)
        {
            return null;
        }

        return MapIssue(issue);
    }

    private static IssueDto MapIssue(IssueEntity issue)
    {
        return new IssueDto
        {
            Id = issue.Id,
            Name = issue.Name,
            Description = issue.Description,
            Priority = MapPriority(issue.Priority)
        };
    }

    private static IssuePriorityDto MapPriority(IssuePriority priority)
    {
        return priority switch
        {
            IssuePriority.Low => IssuePriorityDto.Low,
            IssuePriority.Medium => IssuePriorityDto.Medium,
            IssuePriority.High => IssuePriorityDto.High,
            _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unsupported issue priority value.")
        };
    }
}
