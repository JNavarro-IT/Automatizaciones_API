using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //CONTROLADOR DE LAS PETICIONES HTTP A APIS EXTERNAS COMO PROXY
    [ApiController]
    [Route("[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProxyController> _logger;

        //CONSTRUCTOR PROXY CON UN CLIENTE HTTP PARA LAS PETICIONES EXTERNAS
        public ProxyController(IHttpClientFactory httpClientFactory, ILogger<ProxyController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
       
        //CONSULTAR API PUBLICA DEL CATASTRO SEGÚN EL PARAMETRO QUE SE OBTENGA
        [HttpGet("{*url}")] // proxy/URLPublica
        public async Task<IActionResult> Get(string url)
        {
            _logger.LogInformation("Accediendo a API externa del Catastro...");

            string provincia = Request.Query["Provincia"];
            string coordX = Request.Query["CoorX"];
            string coordY = Request.Query["CoorY"];
            string SRS = Request.Query["SRS"];
            string refCatastral = Request.Query["RefCat"];

            if (provincia != null)
            {
                url += "?Provincia=" + provincia;
            }

            if (coordX != null && coordX != null && SRS != null)
            {
                url += "?CoorX=" + coordX + "&CoorY=" + coordY + "&SRS=" + SRS;
            }

            if (refCatastral != null)
            {
                url += "?RefCat=" + refCatastral;
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
}