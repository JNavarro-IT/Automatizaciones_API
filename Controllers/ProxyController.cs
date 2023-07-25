using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //CONTROLADOR DE LAS PETICIONES HTTP, TAMBIEN A APIS EXTERNAS COMO PROXY
    [ApiController]
    [Route("[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBaseRepository<Proyecto, ProyectoDto> _projectController;
        private readonly IBaseRepository<Inversor, InversorDto> _inversorRepository;
        private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;

        //CONSTRUCTOR PROXY CON UN CLIENTE HTTP PARA LAS PETICIONES EXTERNAS
        public ProxyController(IHttpClientFactory httpClientFactory, IBaseRepository<Proyecto, ProyectoDto> projectController, IBaseRepository<Inversor, InversorDto> inversorRepository, IBaseRepository<Modulo, ModuloDto> moduloRepository)
        {
            _httpClientFactory = httpClientFactory;
            _projectController = projectController;
            _inversorRepository = inversorRepository;
            _moduloRepository = moduloRepository;
        }

        //CONSULTAR API PUBLICA DEL CATASTRO SEGÚN EL PARAMETRO QUE SE OBTENGA
        [HttpGet("{*url}")] // proxy/URLPublica
        public async Task<IActionResult> Get(string url)
        {
            string? provincia = Request.Query["Provincia"];
            string? coordX = Request.Query["CoorX"];
            string? coordY = Request.Query["CoorY"];
            string? SRS = Request.Query["SRS"];
            string? refCatastral = Request.Query["RefCat"];

            if (provincia != null)
                url += "?Provincia=" + provincia;

            if (coordX != null && coordX != null && SRS != null)
                url += "?CoorX=" + coordX + "&CoorY=" + coordY + "&SRS=" + SRS;

            if (refCatastral != null)
                url += "?RefCat=" + refCatastral;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }
                return StatusCode((int)response.StatusCode);
            }
            catch (Exception e)
            {
                return BadRequest($"Respuesta no encontrada: {e.Message}");
            }
        }

        [HttpGet("referencia")]
        public async Task<ActionResult<string>> CrearReferenciaAsync()
        {

            var proyectosList = await _projectController.GetEntitiesListAsync();
        
            if (proyectosList == null)
                return NotFound("Lista de proyectos no encontrada");

            var ultimoProyecto = proyectosList.OrderByDescending(p => p.IdProyecto).FirstOrDefault();
            var defaultNumReferencia = 5000;

            if (ultimoProyecto != null)
            {
                var ultimaReferencia = ultimoProyecto.Referencia;
                defaultNumReferencia = int.Parse(ultimaReferencia[(ultimaReferencia.LastIndexOf('-') + 1)..]);
            }
            var nuevoNumReferencia = defaultNumReferencia + 1;
            var nuevaReferencia = $"OFC-G{DateTime.Now.Year.ToString()[2..]}-{nuevoNumReferencia}";
            return Ok(nuevaReferencia);
        }

        [HttpGet("inversores")]
        public ActionResult<IEnumerable<Inversor>> GetInversoresList()
        {
            var inversoresList = _inversorRepository.GetEntitiesListAsync().Result.ToList();
            if (inversoresList == null)
                return NotFound("No se ha encontrado ninguna lista de inversores");

            return Ok(inversoresList);
        }

        [HttpGet("modulos")]
        public ActionResult<IEnumerable<Modulo>> GetModulosList()
        {
            var modulosList = _moduloRepository.GetEntitiesListAsync().Result;
            if (modulosList == null)
                return NotFound("No se ha encontrado ninguna lista de módulos");

            return Ok(modulosList);
        }
    }
}