namespace Csira.Services.Issues;

public interface IIssueService
{
    Task<IReadOnlyList<IssueDto>> GetIssuesAsync(CancellationToken cancellationToken = default);

    Task<IssueDto?> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
