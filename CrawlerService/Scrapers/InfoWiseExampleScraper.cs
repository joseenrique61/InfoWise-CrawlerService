using System.Globalization;
using CrawlerService.Interfaces;
using CrawlerService.Models;
using HtmlAgilityPack;

namespace CrawlerService.Scrapers;

public class InfoWiseExampleScraper(HttpClient httpClient) : IScraper
{
    private const string BaseUrl = "http://localhost:5023";

    public async Task<IEnumerable<News>> ScrapeNewsAsync(DateTime dateTime)
    {
        return await ScrapeNewsByDateAsync(dateTime);
    }

    private async Task<List<News>> ScrapeNewsByDateAsync(DateTime date)
    {
        var newsList = new List<News>();
        var doc = new HtmlDocument();

        string htmlContent;
        try
        {
            htmlContent = await httpClient.GetStringAsync(BaseUrl);
        }
        catch (HttpRequestException)
        {
            return newsList;
        }
        doc.LoadHtml(htmlContent);

        var rowNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'row')]/div");

        var cardNodes = rowNode.SelectNodes("./div[contains(@class, 'card')]");

        if (cardNodes == null) return newsList;
        
        foreach (var cardNode in cardNodes)
        {
            var dateNode = cardNode.SelectSingleNode(".//h6[contains(@class, 'card-subtitle')]");
            if (dateNode == null) continue;

            if (!DateTime.TryParseExact(dateNode.InnerText.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var articleDate)) continue;
            if (articleDate.Date != date.Date) continue;
            var linkNode = cardNode.SelectSingleNode(".//a[contains(@class, 'card-link')]");
            if (linkNode == null) continue;
            var relativeArticleUrl = linkNode.GetAttributeValue("href", string.Empty);
            var absoluteArticleUrl = BaseUrl + relativeArticleUrl;
            var news = await ScrapeArticleContentAsync(absoluteArticleUrl);
            newsList.Add(news);
        }

        return newsList;
    }

    private async Task<News> ScrapeArticleContentAsync(string articleUrl)
    {
        var doc = new HtmlDocument();
        var htmlContent = await httpClient.GetStringAsync(articleUrl);
        doc.LoadHtml(htmlContent);

        var titleNode = doc.DocumentNode.SelectSingleNode("//h1[contains(@class, 'title')]");
        var dateNode = doc.DocumentNode.SelectSingleNode("//p[contains(@class, 'date')]");
        var contentNode = doc.DocumentNode.SelectSingleNode("//p[contains(@class, 'content')]");
        
        var dateText = dateNode?.InnerText.Replace("Publicado el:", "").Replace("&#x202F;", " ").Replace("&#xA0;", " ").Trim();
        DateTime.TryParse(dateText, CultureInfo.GetCultureInfo("es-ES"), DateTimeStyles.None, out var parsedDate);

        return new News
        {
            Title = titleNode?.InnerText.Trim() ?? string.Empty,
            Date = parsedDate,
            Content = contentNode?.InnerText.Trim() ?? string.Empty,
            Source = "InfoWise"
        };
    }
}
