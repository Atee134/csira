namespace Csira.Services.Issues;

public interface IIssueService
{
    Task<IssueListResult> GetIssuesAsync(IssueListQuery query, CancellationToken cancellationToken = default);

    Task<IssueDto?> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> DeleteIssueAsync(Guid id, CancellationToken cancellationToken = default);
}
