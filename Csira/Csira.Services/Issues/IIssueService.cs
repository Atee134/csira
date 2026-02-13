namespace Csira.Services.Issues;

public interface IIssueService
{
    Task<IssueDto?> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken = default);
}