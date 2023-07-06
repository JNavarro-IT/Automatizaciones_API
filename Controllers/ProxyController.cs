using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    // proxy/URLPublica
    [HttpGet("{*url}")]
    public async Task<IActionResult> Get(string url)
    {
        string provincia = Request.Query["Provincia"];

        if (provincia != null || provincia == "")
        {
            url += "?Provincia=" + provincia;
        }

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
        return StatusCode((int)response.StatusCode);
    }
}
