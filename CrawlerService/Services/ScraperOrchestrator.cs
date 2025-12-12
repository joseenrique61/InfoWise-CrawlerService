using CrawlerService.Interfaces;
using CrawlerService.Models;

namespace CrawlerService.Services;

public class ScraperOrchestrator(IEnumerable<IScraper> scrapers)
{
    public async Task<IEnumerable<News>> ScrapeAllAsync(DateTime dateTime)
    {
        var allNews = new List<News>();
        foreach (var scraper in scrapers)
        {
            var news = await scraper.ScrapeNewsAsync(dateTime);
            allNews.AddRange(news);
        }
        return allNews;
    }
}
