using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public interface INewsService
{
    Task<NewsApiResponse> GetFinancialNews(string country = "br", string language = "pt");
}

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NewsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["NewsDataApi:Key"];
    }

    public async Task<NewsApiResponse> GetFinancialNews(string country = "br", string language = "pt")
    {
        var url = $"https://newsdata.io/api/1/news?apikey={_apiKey}&q=financas&country={country}&language={language}&category=business,politics,world";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<NewsApiResponse>(content);
    }
}