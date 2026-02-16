namespace Csira.Services.Issues;

public class IssueListResult
{
    public IReadOnlyList<IssueDto> Items { get; init; } = Array.Empty<IssueDto>();

    public int TotalCount { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
