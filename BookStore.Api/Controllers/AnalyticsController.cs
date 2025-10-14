using Microsoft.AspNetCore.Mvc;
using BookStore.Core.Services;

namespace BookStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly BookAnalyticsService _analyticsService;

    public AnalyticsController(BookAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("top-authors")]
    public async Task<IActionResult> GetTopAuthors()
    {
        var result = await _analyticsService.GetTopAuthorsBySalesAsync(3);
        return Ok(result);
    }
}
