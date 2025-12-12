using CrawlerService.Infrastructure;
using CrawlerService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ScraperConfigurator.ConfigureScrapers(builder);
builder.Services.AddScoped<ScraperOrchestrator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
