using backend_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //CONTROLADOR DE LAS PETICIONES HTTP DEL CLIENTE
    [ApiController]
    [Route("api/proyectos")]
    public class ProyectoController: ControllerBase
    {
        private readonly ILogger<ProyectoController> _logger;
        private readonly IProyectoRepository _proyectoRepository;

        public ProyectoController(IProyectoRepository proyectoRepository, ILogger<ProyectoController> logger)
        {
            _logger = logger;
            _proyectoRepository = proyectoRepository;
        }

        //Proyecto/nuevo-proyecto
        [HttpGet("nuevo-proyecto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<string> CrearReferencia()
        {
            _logger.LogInformation("Obteniendo nueva referencia...");
            string nuevaReferencia = _proyectosService.CrearReferencia();

            if(nuevaReferencia.Length == 0)
            {
                return NoContent();
            }

            return Ok(nuevaReferencia);
        }
    }
}
