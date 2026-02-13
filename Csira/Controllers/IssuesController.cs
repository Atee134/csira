using Csira.Services.Issues;
using Microsoft.AspNetCore.Mvc;

namespace Csira.Web.Controllers;

[Route("issues")]
public class IssuesController(IIssueService issueService) : Controller
{
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