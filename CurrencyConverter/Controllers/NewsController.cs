using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string country = "br",
        [FromQuery] string language = "pt")
    {
        try
        {
            var news = await _newsService.GetFinancialNews(country, language);
            return Ok(news);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}