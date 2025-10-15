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

    [HttpGet("average-sales-by-genre")]
    public async Task<IActionResult> GetAverageSalesByGenre()
    {
        var result = await _analyticsService.GetSalesAverageByGenre();
        Console.WriteLine($"Nombre de genres trouvés : {result.Count()}");
        return Ok(result);
    }

    [HttpGet("max-sold-book-by-genre")]
    public async Task<IActionResult> GetMaxSoldBookByGenre()
    {
        var result = await _analyticsService.GetMaxSoldBookByGenre();
        return Ok(result);
    }
    [HttpGet("books-published-after-2010")]
    public async Task<IActionResult> GetBooksPublishedAfter2010()
    {
        var result = await _analyticsService.GetBooksPublishedAfter2010();
        return Ok(result);
    }
}
