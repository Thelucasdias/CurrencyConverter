using CurrencyConverter.Services;
using CurrencyConverter.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<INewsService, NewsService>();
builder.Services.Configure<NewsApiSettings>(builder.Configuration.GetSection("NewsDataApi"));
builder.Services.AddScoped<INewsService, NewsService>();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddHttpClient<CurrencyService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAll");



app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

public class NewsApiSettings
{
}