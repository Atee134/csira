namespace Csira.Services.Issues;

public class IssueListQuery
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;

    public IssueSortOption Sort { get; init; } = IssueSortOption.CreatedAtDesc;
}
