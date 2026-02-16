using Csira.Services.Issues;

namespace Csira.Web.Models.Issues;

public class IssuesIndexViewModel
{
    public IReadOnlyList<IssueDto> Issues { get; init; } = Array.Empty<IssueDto>();

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public IssueSortOption Sort { get; init; }

    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
}
