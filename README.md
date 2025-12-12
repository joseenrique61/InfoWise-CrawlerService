# InfoWise CrawlerService

A C# .NET Core web API for orchestrating various web scrapers. Each scraper implements a common interface and returns news articles with titles, dates, content, and source information. This service does not use a database and returns scrapped data on demand.

This project is part of InfoWise.

## Project Structure

-   `CrawlerService.sln`: Solution file.
-   `CrawlerService/`: Main project directory.
    -   `Models/News.cs`: Defines the data model for a news article.
    -   `Interfaces/IScraper.cs`: Defines the contract for all web scrapers.
    -   `Scrapers/SampleNewsScraper.cs`: A sample scraper that returns hardcoded news data.
    -   `Scrapers/InfoWiseScraper.cs`: A scraper that fetches content from `http://test.example.com` and parses news articles.
    -   `Services/ScraperOrchestrator.cs`: Orchestrates multiple `IScraper` implementations to gather news.
    -   `Controllers/ScraperController.cs`: API endpoint to trigger the scraping process.
    -   `Infrastructure/ScraperConfigurator.cs`: Configures services for the scrapers.
    -   `Program.cs`: Configures the application, registers services (including the orchestrator), and defines the HTTP request pipeline.

## Getting Started

### Prerequisites

-   .NET SDK (compatible with the project's target framework, e.g., .NET 8.0)

### Setup

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd CrawlerService
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

### Running the Application

1.  **Build the project:**
    ```bash
    dotnet build
    ```

2.  **Run the application:**
    ```bash
    dotnet run --project CrawlerService
    ```
    The application will start, typically listening on `http://localhost:5178` (or other ports configured in `launchSettings.json`).

### API Endpoint

Once the application is running, you can access the scraping functionality via the following endpoint:

-   **GET /api/scraper/scrape**
    This endpoint will trigger all registered scrapers and return a consolidated list of news articles.

    Example using `curl`:
    ```bash
    curl -k http://localhost:5178/api/scraper/scrape
    ```

### Adding New Scrapers

To add a new web scraper:

1.  Create a new class in the `Scrapers/` directory (e.g., `MyNewScraper.cs`).
2.  Implement the `IScraper` interface in your new class, providing the logic to scrape news from your target website.
3.  Register your new scraper in `CrawlerService/Infrastructure/ScraperConfigurator.cs` by adding a line like this:
    ```csharp
    builder.Services.AddScoped<IScraper, MyNewScraper>();
    ```
    If your scraper requires an `HttpClient` (as `InfoWiseScraper` does), you can register it similarly:
    ```csharp
    builder.Services.AddHttpClient<MyNewScraper>();
    builder.Services.AddScoped<IScraper, MyNewScraper>();
    ```
    The `ScraperOrchestrator` will automatically discover and use your new scraper.

## License
This project is distributed under the MIT License, as specified in the [LICENSE](LICENSE) file.