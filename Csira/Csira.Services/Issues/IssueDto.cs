using Csira.Services.Dtos;

namespace Csira.Services.Issues;

public class IssueDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public IssuePriorityDto Priority { get; init; }
}