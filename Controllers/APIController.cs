using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;
using backend_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_API.Controllers
{
   //CONTROLADOR DE LAS PETICIONES HTTP, TAMBIEN A APIS EXTERNAS COMO PROXY
   [ApiController]
   [Route("[controller]")]
   public class ApiController : ControllerBase
   {
      private readonly IHttpClientFactory _httpClientFactory;
      private readonly IBaseRepository<Proyecto, ProyectoDto> _proyectoRepository;
      private readonly IBaseRepository<Inversor, InversorDto> _inversorRepository;
      private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;
      private readonly IInstalacionService _instalacionService;

      //CONSTRUCTOR PROXY CON UN CLIENTE HTTP PARA LAS PETICIONES EXTERNAS
      public ApiController(IHttpClientFactory httpClientFactory, IBaseRepository<Proyecto, ProyectoDto> projectController, IBaseRepository<Inversor, InversorDto> inversorRepository, IBaseRepository<Modulo, ModuloDto> moduloRepository, IInstalacionService instalacionService)
      {
         _httpClientFactory = httpClientFactory;
         _proyectoRepository = projectController;
         _inversorRepository = inversorRepository;
         _moduloRepository = moduloRepository;
         _instalacionService = instalacionService;
      }

      //CONSULTAR APIS PUBLICAS MEDIANTE UN AUTOPROXY PARA SUBSANAR POLÍTICAS DE CORS
      [HttpGet("proxy/{*url}")] // proxy/URLPublica
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<IActionResult> UseProxy(string url)
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
            return BadRequest($"Respuesta no encontrada: {e.StackTrace}");
         }
      }

      [HttpGet("referencia")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<string>> CrearReferencia()
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

      [HttpGet("proyecto/{referencia}")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> GetProyecto(string referencia)
      {
         if (referencia == null)
            return BadRequest("La referencia no es válida no existe");
         var Proyecto = _proyectoRepository.GetEntitiesInclude(
               filter: p => p.Referencia == referencia,
               includes: query => query.Include(p => p.Cliente).Include(p => p.Instalacion).Include(p => p.LugaresPRL))
            .FirstOrDefault();
         
         if (Proyecto == null)
            return NotFound("No existe el proyecto para esa referencia");

         var Cadenas = Proyecto.Instalacion.Cadenas;
         foreach (CadenaDto c in Cadenas)
         {
            c.Inversor = await _inversorRepository.GetEntityDto(c.IdInversor);
            c.Modulo = await _moduloRepository.GetEntityDto(c.IdModulo);
         }
         Proyecto.Fecha = Proyecto.Fecha.Date;
         Proyecto.PlazoEjecucion = Proyecto.PlazoEjecucion.Date;

         return Ok(Proyecto);
      }

    [HttpGet("inversores")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<InversorDto>> GetInversoresList()
      {
         var inversoresList = _inversorRepository.GetEntitiesList().Result.ToList();
         if (inversoresList == null)
            return NotFound("No se ha encontrado ninguna lista de inversores");

         return Ok(inversoresList);
      }
    
      [HttpPost("inversor/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<InversorDto>> InsertInversor(InversorDto Inversor)
      {
         if (Inversor == null)
            return BadRequest("El inversor enviado no es válido");

         InversorDto newInversor = await _inversorRepository.CreateEntity(Inversor);

         if (newInversor == null)
            return NotFound("Error al crear el inversor");

         return Ok("Inversor creado. Id: " + newInversor.IdInversor + ", Modelo: " + newInversor.Modelo);
      }

      [HttpGet("modulos")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<ModuloDto>> GetModulosList()
      {
         var modulosList = _moduloRepository.GetEntitiesList().Result;
         if (modulosList == null)
            return NotFound("No se ha encontrado ninguna lista de módulos");

         return Ok(modulosList);
      }

      [HttpPost("modulo/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ModuloDto>> InsertModulo(ModuloDto Modulo)
      {
         if (Modulo == null)
            return BadRequest("El módulo enviado no es válido");

         ModuloDto newModulo = await _moduloRepository.CreateEntity(Modulo);

         if (newModulo == null)
            return NotFound("Error al crear el módulo");

         return Ok("Modulo creado. Id: " + newModulo.IdModulo + ",  Modelo: " + newModulo.Modelo);
      }

      [HttpPost("instalacion/calcular")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<InstalacionDto> GetInstalacionCalculated(InstalacionDto Instalacion)
      {
         if (Instalacion == null)
            return BadRequest("La instalacion enviada no es válida");

         Instalacion = _instalacionService.CalcularInstalacion(Instalacion);

         if (Instalacion == null)
            return NoContent();

         return Ok(Instalacion);
      }

      [HttpPost("proyecto/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> InsertProyecto(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return BadRequest("El proyecto enviado no es válido");

         var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         var Instalacion = Proyecto.Instalacion;
         var latLngUTM = _instalacionService.GetUTM(Instalacion);
         Ubicacion.CoordXUTM = latLngUTM[0];
         Ubicacion.CoordYUTM = latLngUTM[1];
         var Cadenas = Instalacion.Cadenas;
         foreach (CadenaDto c in Cadenas)
         {
            c.IdInversor = c.Inversor.IdInversor;
            c.IdModulo = c.Modulo.IdModulo;
         }
         ProyectoDto newProyecto = await _proyectoRepository.CreateEntity(Proyecto);

         if (newProyecto == null)
            return NotFound("Error al crear el proyecto");

         return Ok(newProyecto);
      }

      [HttpPost("proyecto/update")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> UpdateProyecto(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return BadRequest("El proyecto enviado no es válido");

         var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         var Instalacion = Proyecto.Instalacion;
         var latLngUTM = _instalacionService.GetUTM(Instalacion);
         Ubicacion.CoordXUTM = latLngUTM[0];
         Ubicacion.CoordYUTM = latLngUTM[1];
         ProyectoDto newProyecto = await _proyectoRepository.CreateEntity(Proyecto);

         if (newProyecto == null)
            return NotFound("Error al crear el proyecto");

         return Ok(newProyecto);
      }
   }
}