using Csira.DataAccess;
using Csira.DataAccess.Entities;
using Csira.Services.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Csira.Services.Issues;

public class IssueService(AppDbContext dbContext) : IIssueService
{
    public async Task<IssueListResult> GetIssuesAsync(IssueListQuery query, CancellationToken cancellationToken = default)
    {
        var pageSize = query.PageSize is 10 or 20 or 50 ? query.PageSize : 10;
        var totalCount = await dbContext.Issues.CountAsync(cancellationToken);

        var totalPages = totalCount == 0
            ? 1
            : (int)Math.Ceiling(totalCount / (double)pageSize);
        var pageNumber = Math.Clamp(query.PageNumber, 1, totalPages);
        var skip = (pageNumber - 1) * pageSize;

        var sortedQuery = ApplySort(
            dbContext.Issues.AsNoTracking(),
            query.Sort);

        var issues = await sortedQuery
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new IssueListResult
        {
            Items = issues.Select(MapIssue).ToList(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
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

    public async Task<bool> DeleteIssueAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deletedCount = await dbContext.Issues
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return deletedCount > 0;
    }

    private static IssueDto MapIssue(IssueEntity issue)
    {
        return new IssueDto
        {
            Id = issue.Id,
            CreatedAtUtc = issue.CreatedAtUtc,
            Name = issue.Name,
            Description = issue.Description,
            Priority = MapPriority(issue.Priority)
        };
    }

    private static IQueryable<IssueEntity> ApplySort(IQueryable<IssueEntity> query, IssueSortOption sort)
    {
        return sort switch
        {
            IssueSortOption.CreatedAtAsc => query
                .OrderBy(x => x.CreatedAtUtc)
                .ThenBy(x => x.Id),
            IssueSortOption.CreatedAtDesc => query
                .OrderByDescending(x => x.CreatedAtUtc)
                .ThenBy(x => x.Id),
            IssueSortOption.NameAsc => query
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.CreatedAtUtc),
            IssueSortOption.NameDesc => query
                .OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.CreatedAtUtc),
            IssueSortOption.PriorityAsc => query
                .OrderBy(x => x.Priority == IssuePriority.Low ? 0 : x.Priority == IssuePriority.Medium ? 1 : 2)
                .ThenByDescending(x => x.CreatedAtUtc),
            IssueSortOption.PriorityDesc => query
                .OrderByDescending(x => x.Priority == IssuePriority.Low ? 0 : x.Priority == IssuePriority.Medium ? 1 : 2)
                .ThenByDescending(x => x.CreatedAtUtc),
            _ => query
                .OrderByDescending(x => x.CreatedAtUtc)
                .ThenBy(x => x.Id)
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
