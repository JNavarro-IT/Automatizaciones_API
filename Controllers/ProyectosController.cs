using backend_API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProyectosController: ControllerBase
    {
        private readonly ILogger<ModulosController> _logger;
        private readonly IProyectosService _proyectosService;

        public ProyectosController(IProyectosService proyectosService, ILogger<ModulosController> logger)
        {
            _logger = logger;
            _proyectosService = proyectosService;
        }

        [HttpGet("nuevo-proyecto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<string> GetNuevaReferencia()
        {
            _logger.LogInformation("Obteniendo nueva referencia...");
            string nuevaReferencia = _proyectosService.CrearNuevaReferencia();

            if(nuevaReferencia.Length == 0)
            {
                return NoContent();
            }

            return Ok(nuevaReferencia);
        }
    }
}
