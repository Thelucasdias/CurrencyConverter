public class NewsApiResponse
{
    public string Status { get; set; }
    public int TotalResults { get; set; }
    public List<NewsResult> Results { get; set; }
}