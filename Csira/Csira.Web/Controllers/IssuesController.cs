using Csira.Services.Issues;
using Csira.Web.Models.Issues;
using Microsoft.AspNetCore.Mvc;

namespace Csira.Web.Controllers;

[Route("issues")]
public class IssuesController(IIssueService issueService) : Controller
{
    private static readonly int[] AllowedPageSizes = [10, 20, 50];

    [HttpGet("")]
    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 10,
        IssueSortOption sort = IssueSortOption.CreatedAtDesc,
        CancellationToken cancellationToken = default)
    {
        var selectedSort = Enum.IsDefined(sort) ? sort : IssueSortOption.CreatedAtDesc;
        var selectedPageSize = AllowedPageSizes.Contains(pageSize) ? pageSize : 10;

        var query = new IssueListQuery
        {
            PageNumber = page,
            PageSize = selectedPageSize,
            Sort = selectedSort
        };

        var result = await issueService.GetIssuesAsync(query, cancellationToken);

        var viewModel = new IssuesIndexViewModel
        {
            Issues = result.Items,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Sort = selectedSort
        };

        return View(viewModel);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var issue = await issueService.GetIssueByIdAsync(id, cancellationToken);

        if (issue is null)
        {
            return NotFound();
        }

        return View(issue);
    }
}
