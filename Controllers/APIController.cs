using Automatizaciones_API.Models;
using Automatizaciones_API.Models.Dto;
using Automatizaciones_API.Repository;
using Automatizaciones_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Automatizaciones_API.Controllers
{
   // CONTROLADOR DE LAS PETICIONES HTTP ACERCA DEL PROYECTO, TAMBIEN LLAMA A APIS EXTERNAS COMO PROXY
   [ApiController]
   [Route("[controller]")]
   public class ApiController : ControllerBase
   {
      private readonly IHttpClientFactory _httpClientFactory;
      private readonly IBaseRepository<Proyecto, ProyectoDto> _proyectoRepository;
      private readonly IBaseRepository<Inversor, InversorDto> _inversorRepository;
      private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;
      private readonly IProjectService _projectService;

      // CONSTRUCTOR POR PARÁMETROS PARA INYECTAR LAS DEPENDENCIAS
      public ApiController(IHttpClientFactory httpClientFactory, IBaseRepository<Proyecto, ProyectoDto> proyectoRepository, IBaseRepository<Inversor, InversorDto> inversorRepository, IBaseRepository<Modulo, ModuloDto> moduloRepository, IProjectService projectService)
      {
         _httpClientFactory = httpClientFactory;
         _proyectoRepository = proyectoRepository;
         _inversorRepository = inversorRepository;
         _moduloRepository = moduloRepository;
         _projectService = projectService;
      }

      // CONSULTAR APIS PUBLICAS MEDIANTE UN AUTOPROXY PARA SUBSANAR POLÍTICAS DE CORS
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

         if (coordX != null && coordY != null && SRS != null)
            url += "?CoorX=" + coordX + "&CoorY=" + coordY + "&SRS=" + SRS;

         if (refCatastral != null) url += "?RefCat=" + refCatastral;

         if (url.Contains("PVGIS")) url = url.Split("=>")[1].Replace("*", "?");

         try
         {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
               string content = await response.Content.ReadAsStringAsync();
               return Content(content);

            }
            return NotFound("ERROR => Referencia no encontrada " + response.ReasonPhrase);

         }
         catch (Exception ex) { return Problem("EXCEPTION => La referencia ha fallado: " + ex.StackTrace); }
      }

      [HttpGet("referencia")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<string>> CrearReferencia()
      {
         IEnumerable<ProyectoDto> proyectosList = await _proyectoRepository.GetEntitiesList();

         if (proyectosList == null) return NotFound("Lista de proyectos no encontrada");

         ProyectoDto? ultimoProyecto = proyectosList.OrderByDescending(p => p.IdProyecto).FirstOrDefault();
         int defaultNumReferencia = 5000;

         if (ultimoProyecto != null)
         {
            string ultimaReferencia = ultimoProyecto.Referencia;
            defaultNumReferencia = int.Parse(ultimaReferencia[(ultimaReferencia.LastIndexOf('-') + 1)..]);
         }
         int nuevoNumReferencia = defaultNumReferencia + 1;
         string nuevaReferencia = $"OFC-G{DateTime.Now.Year.ToString()[2..]}-{nuevoNumReferencia}";
         return Ok(nuevaReferencia);
      }

      [HttpGet("proyecto/{referencia}")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> GetProyecto(string referencia)
      {
         if (referencia == null) return BadRequest("La referencia no es válida no existe");

         ProyectoDto? Proyecto = _proyectoRepository.GetEntitiesInclude(
               filter: p => p.Referencia == referencia,
               includes: query => query
               .Include(p => p.Cliente)
               .Include(p => p.Instalacion)
               .Include(p => p.LugaresPRL))
               .FirstOrDefault();

         if (Proyecto == null)
            return NotFound("No existe el proyecto para esa referencia");

         IList<CadenaDto> Cadenas = Proyecto.Instalacion.Cadenas;
         foreach (CadenaDto c in Cadenas)
         {
            if (c is null) continue;

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
         if (empresa is null or "")
            return BadRequest("Se requiere el nombre de la empresa para obtener el CIF");

         string CIF = _projectService.GetCIF(empresa);
         return CIF.Contains("ERROR") ? NotFound(CIF) : Ok(CIF);
      }

      [HttpGet("inversores")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<InversorDto>> GetInversoresList()
      {
         List<InversorDto>? inversoresList = _inversorRepository.GetEntitiesList().Result.ToList();

         if (inversoresList == null) return NotFound("ERROR => No se ha encontrado ninguna lista de inversores");

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
         return newInversor == null
            ? NotFound("Error al crear el inversor")
            : Ok("Inversor creado. Id: " + newInversor.IdInversor + ", Modelo: " + newInversor.Modelo);
      }

      [HttpGet("modulos")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<IEnumerable<ModuloDto>> GetModulosList()
      {
         IEnumerable<ModuloDto> modulosList = _moduloRepository.GetEntitiesList().Result;
         return modulosList == null ? NotFound("No se ha encontrado ninguna lista de módulos") : (ActionResult<IEnumerable<ModuloDto>>)Ok(modulosList);
      }

      [HttpPost("modulo/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ModuloDto>> InsertModulo(ModuloDto Modulo)
      {
         if (Modulo == null) return BadRequest("El módulo enviado no es válido");

         ModuloDto newModulo = await _moduloRepository.CreateEntity(Modulo);
         return newModulo == null
            ? NotFound("Error al crear el módulo")
            : Ok("Módulo creado. Id: " + newModulo.IdModulo + ",  Modelo: " + newModulo.Modelo);
      }

      [HttpPost("instalacion/calcular")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status409Conflict)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]

      public ActionResult<InstalacionDto?> GetInstalacionCalculated(InstalacionDto? Instalacion)
      {
         if (Instalacion == null) return BadRequest("La instalación enviada no es válida");

         try
         {
            var newInstalacion = _projectService.CalcularInstalacion(Instalacion);
            return Ok(newInstalacion);
         }
         catch (InvalidOperationException ex) { return Conflict(ex.Message); }
         catch (Exception e)
         {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + e.Message);
         }
      }

      [HttpPost("proyecto/insert")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> InsertProyecto(ProyectoDto Proyecto)
      {
         try
         {
            if (Proyecto == null) return BadRequest("ERROR => El proyecto enviado no es válido");

            UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
            InstalacionDto Instalacion = Proyecto.Instalacion;
            Proyecto = _projectService.GetDatosLegalizaciones(Proyecto);
            double[] latLngUTM = _projectService.GetUTM(Instalacion);
            Ubicacion.CoordXUTM = latLngUTM[0];
            Ubicacion.CoordYUTM = latLngUTM[1];
            IList<CadenaDto> Cadenas = Instalacion.Cadenas;
            foreach (CadenaDto c in Cadenas)
            {
               c.IdInversor = c.Inversor.IdInversor;
               c.IdModulo = c.Modulo.IdModulo;
            }
            ProyectoDto newProyecto = await _proyectoRepository.CreateEntity(Proyecto);

            return newProyecto == null ? NotFound("ERROR => Durante la creación del proyecto") : Ok(newProyecto);
         }
         catch (Exception ex) { return Conflict("EXCEPTION INSERT-PROYECT=> No se ha podido insertar el proyecto: " + ex.GetBaseException() + "AYUDA: " + ex.HelpLink + " PILA: " + ex.StackTrace); }
      }

      [HttpPost("proyecto/update")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<ProyectoDto>> UpdateProyecto(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return BadRequest("ERROR => El proyecto enviado no es válido");

         UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         InstalacionDto Instalacion = Proyecto.Instalacion;
         double[] latLngUTM = _projectService.GetUTM(Instalacion);
         Ubicacion.CoordXUTM = latLngUTM[0];
         Ubicacion.CoordYUTM = latLngUTM[1];

         int files = await _proyectoRepository.UpdateEntity(Proyecto);
         if (files == -1)
            return BadRequest("ERROR => Error al crear el proyecto");

         else if (files == 0)
            return NotFound("ERROR => No se ha encontrado el proyecto a actualizar");

         return Ok(Proyecto);
      }
   }
}