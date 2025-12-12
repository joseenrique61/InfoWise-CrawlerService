using CrawlerService.Interfaces;
using CrawlerService.Scrapers;

namespace CrawlerService.Infrastructure;

public class ScraperConfigurator
{
    public static void ConfigureScrapers(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<InfoWiseExampleScraper>();
        builder.Services.AddScoped<IScraper, InfoWiseExampleScraper>();
    }
}