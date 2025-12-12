using CrawlerService.Models;

namespace CrawlerService.Interfaces;

public interface IScraper
{
    Task<IEnumerable<News>> ScrapeNewsAsync(DateTime dateTime);
}
