using CrawlerService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrawlerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScraperController(ScraperOrchestrator scraperOrchestrator) : ControllerBase
{
    [HttpGet("scrape")]
    public async Task<IActionResult> Scrape(DateTime dateTime)
    {
        var news = await scraperOrchestrator.ScrapeAllAsync(dateTime);
        return Ok(news);
    }
}
