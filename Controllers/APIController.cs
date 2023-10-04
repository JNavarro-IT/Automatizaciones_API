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
      private readonly IBaseRepository<Error, ErrorDto> _errorRepository;
      private readonly IProjectService _projectService;

      public ApiController(IHttpClientFactory httpClientFactory, IBaseRepository<Proyecto, ProyectoDto> proyectoRepository, IBaseRepository<Inversor, InversorDto> inversorRepository, IBaseRepository<Modulo, ModuloDto> moduloRepository, IProjectService projectService, IBaseRepository<Error, ErrorDto> errorRepository)
      {
         _httpClientFactory = httpClientFactory;
         _proyectoRepository = proyectoRepository;
         _inversorRepository = inversorRepository;
         _moduloRepository = moduloRepository;
         _projectService = projectService;
         _errorRepository = errorRepository;
      }

      //CONSULTAR APIS PUBLICAS MEDIANTE UN AUTOPROXY PARA SUBSANAR POLÍTICAS DE CORS
      [HttpGet("proxy/{*url}")] 
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<IActionResult> UseProxy(string url)
      {
         if (url == null) return BadRequest("Error en la url enviada");
         string? provincia = Request.Query["Provincia"];
         string? coordX = Request.Query["CoorX"];
         string? coordY = Request.Query["CoorY"];
         string? SRS = Request.Query["SRS"];
         string? refCatastral = Request.Query["RefCat"];


         if (provincia != null) url += "?Provincia=" + provincia;
         if (coordX != null && coordX != null && SRS != null) url += "?CoorX=" + coordX + "&CoorY=" + coordY + "&SRS=" + SRS;
         if (refCatastral != null) url += "?RefCat=" + refCatastral;
         if (url.Contains("PVGIS")) url = (url.Split("=>")[1]).Replace("*", "?");

         try
         {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
               var content = await response.Content.ReadAsStringAsync();
               return Content(content);

            }
            return StatusCode((int)response.StatusCode);
         }
         catch (Exception e) { return BadRequest($"Respuesta no encontrada: {e.StackTrace}"); }
      }

      [HttpGet("referencia")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<string>> CrearReferencia()
      {
         var proyectosList = await _proyectoRepository.GetEntitiesList();
         if (proyectosList == null) return NotFound("Lista de proyectos no encontrada");

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
         if (referencia == null) return BadRequest("La referencia no es válida no existe");
         var Proyecto = _proyectoRepository.GetEntitiesInclude(
               filter: p => p.Referencia == referencia,
               includes: query => query
               .Include(p => p.Cliente)
               .Include(p => p.Instalacion)
               .Include(p => p.LugaresPRL))
               .FirstOrDefault();

         if (Proyecto == null) return NotFound("No existe el proyecto para esa referencia");

         var Cadenas = Proyecto.Instalacion.Cadenas;
         foreach (CadenaDto? c in Cadenas)
         {
            c.Inversor = await _inversorRepository.GetEntityDto(c.IdInversor);
            c.Modulo = await _moduloRepository.GetEntityDto(c.IdModulo);
         }
         Proyecto.Fecha = Proyecto.Fecha.Date;
         Proyecto.PlazoEjecucion = Proyecto.PlazoEjecucion.Date;

         return Ok(Proyecto);
      }

      [HttpGet("getCIF/{empresa}")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> ObtenerCIF(string empresa)
      {
         if (empresa == null || empresa == "") return BadRequest("Se requiere el nombre de la empresa para obtener el CIF");

         string CIF = _projectService.GetCIF(empresa);
         if (CIF.Contains("ERROR")) return NotFound(CIF);

         return Ok(CIF);
      }

      [HttpGet("inversores")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<InversorDto>> GetInversoresList()
      {
         var inversoresList = _inversorRepository.GetEntitiesList().Result.ToList();
         if (inversoresList == null) return NotFound("No se ha encontrado ninguna lista de inversores");
         return Ok(inversoresList);
      }

      [HttpPost("inversor/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<InversorDto>> InsertInversor(InversorDto Inversor)
      {
         if (Inversor == null) return BadRequest("El inversor enviado no es válido");

         InversorDto newInversor = await _inversorRepository.CreateEntity(Inversor);
         if (newInversor == null) return NotFound("Error al crear el inversor");

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
         if (Modulo == null) return BadRequest("El módulo enviado no es válido");

         ModuloDto newModulo = await _moduloRepository.CreateEntity(Modulo);
         if (newModulo == null) return NotFound("Error al crear el módulo");

         return Ok("Módulo creado. Id: " + newModulo.IdModulo + ",  Modelo: " + newModulo.Modelo);
      }

      [HttpPost("instalacion/calcular")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<InstalacionDto?> GetInstalacionCalculated(InstalacionDto? Instalacion)
      {
         if (Instalacion == null) return BadRequest("La instalación enviada no es válida");

         int? size = Instalacion.Cadenas.Count();
         Instalacion = _projectService.CalcularInstalacion(Instalacion);
         if (Instalacion == null || Instalacion.Cadenas.Count < size) return NoContent();

         return Ok(Instalacion);
      }

      [HttpPost("proyecto/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> InsertProyecto(ProyectoDto Proyecto)
      {
         try
         {
            if (Proyecto == null) return BadRequest("El proyecto enviado no es válido");
            var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
            var Instalacion = Proyecto.Instalacion;
            Proyecto = _projectService.GetDatosLegalizaciones(Proyecto);
            var latLngUTM = _projectService.GetUTM(Instalacion);
            Ubicacion.CoordXUTM = latLngUTM[0];
            Ubicacion.CoordYUTM = latLngUTM[1];
            var Cadenas = Instalacion.Cadenas;
            foreach (CadenaDto c in Cadenas)
            {
               c.IdInversor = c.Inversor.IdInversor;
               c.IdModulo = c.Modulo.IdModulo;
            }
            ProyectoDto newProyecto = await _proyectoRepository.CreateEntity(Proyecto);

            if (newProyecto == null) return NotFound("Error al crear el proyecto");
            return Ok(newProyecto);

         }
         catch (Exception ex)
         {
            Console.Error.WriteLine(ex.ToString());
            return Conflict();
         }
      }

      [HttpPost("proyecto/update")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> UpdateProyecto(ProyectoDto Proyecto)
      {
         if (Proyecto == null) return BadRequest("El proyecto enviado no es válido");

         var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         var Instalacion = Proyecto.Instalacion;
         var latLngUTM = _projectService.GetUTM(Instalacion);
         Ubicacion.CoordXUTM = latLngUTM[0];
         Ubicacion.CoordYUTM = latLngUTM[1];

         var files = await _proyectoRepository.UpdateEntity(Proyecto);
         if (files == -1) return BadRequest("Error al crear el proyecto");
         else if (files == 0) return NotFound("No se ha encontrado el proyecto a actualizar");

         return Ok("Proyecto actualizado con éxito");
      }

      [HttpPost("insertError")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<IActionResult> InsertError(ErrorDto errorFront)
      {
         if (errorFront == null) return BadRequest("Datos de error no válidos.");

         var newError = await _errorRepository.CreateEntity(errorFront);
         if (newError == null) return BadRequest("No se ha pododoguardar el Error");

         return Ok("Error registrado exitosamente.");
      }
   }
}