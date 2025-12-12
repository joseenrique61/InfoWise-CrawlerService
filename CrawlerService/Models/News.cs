namespace CrawlerService.Models;

public class News
{
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
