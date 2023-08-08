using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;
using backend_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //CONTROLADOR DE LAS PETICIONES HTTP, TAMBIEN A APIS EXTERNAS COMO PROXY
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBaseRepository<Proyecto, ProyectoDto> _proyectoRepository;
        private readonly IBaseRepository<Instalacion, InstalacionDto> _instalacionRepository;
        private readonly IBaseRepository<Inversor, InversorDto> _inversorRepository;
        private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;
        private readonly IInstalacionService _instalacionService;

        //CONSTRUCTOR PROXY CON UN CLIENTE HTTP PARA LAS PETICIONES EXTERNAS
        public ApiController(IHttpClientFactory httpClientFactory, IBaseRepository<Proyecto, ProyectoDto> projectController, IBaseRepository<Inversor, InversorDto> inversorRepository, IBaseRepository<Modulo, ModuloDto> moduloRepository, IInstalacionService instalacionService, IBaseRepository<Instalacion, InstalacionDto> instalacionRepository)
        {
            _httpClientFactory = httpClientFactory;
            _proyectoRepository = projectController;
            _inversorRepository = inversorRepository;
            _moduloRepository = moduloRepository;
            _instalacionService = instalacionService;
            _instalacionRepository = instalacionRepository;
        }

        //CONSULTAR API PUBLICA DEL CATASTRO SEGÚN EL PARAMETRO QUE SE OBTENGA
        [HttpGet("proxy/{*url}")] // proxy/URLPublica
        public async Task<IActionResult> Get(string url)
        {
            if (url == null)
                return BadRequest("Error en la url enviada");

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

            var proyectosList = await _proyectoRepository.GetEntitiesList();
        
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
        public ActionResult<IEnumerable<InversorDto>> GetInversoresList()
        {
            var inversoresList = _inversorRepository.GetEntitiesList().Result.ToList();
            if (inversoresList == null)
                return NotFound("No se ha encontrado ninguna lista de inversores");

            return Ok(inversoresList);
        }


        [HttpPost("inversor/insert")]
        public async Task<ActionResult<InversorDto>> InsertInversor(InversorDto Inversor)
        {
            if (Inversor == null)
                return BadRequest("El inversor enviado no es válido");


            bool created = await _inversorRepository.CreateEntity(Inversor);

            if (!created)
                return NotFound("Error al crear el inversor");

            return Ok("Inversor creado: " + Inversor.IdInversor + ": " + Inversor.Modelo);
        }

        [HttpGet("modulos")]
        public ActionResult<IEnumerable<ModuloDto>> GetModulosList()
        {
            var modulosList = _moduloRepository.GetEntitiesList().Result;
            if (modulosList == null)
                return NotFound("No se ha encontrado ninguna lista de módulos");

            return Ok(modulosList);
        }

        [HttpPost("modulo/insert")]
        public async Task<ActionResult<ModuloDto>> InsertModulo(ModuloDto Modulo)
        {
            if (Modulo == null)
                return BadRequest("El módulo enviado no es válido");


            bool created = await _moduloRepository.CreateEntity(Modulo);

            if (!created)
                return NotFound("Error al crear el módulo");

            return Ok("Modulo creado: " + Modulo.IdModulo + ": " + Modulo.Modelo);
        }

        [HttpPost("instalacion/calcular")]
        public ActionResult<InstalacionDto> GetInstalacionCalculated(InstalacionDto Instalacion)
        {
            if (Instalacion == null)
                return BadRequest("La instalacion enviada no es válida");
            
            
            Instalacion = _instalacionService.CalcularInstalacion(Instalacion);

            if(Instalacion == null)
                return NoContent();
            
            return Ok(Instalacion);
        }

    }
}