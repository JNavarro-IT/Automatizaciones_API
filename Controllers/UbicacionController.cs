using backend_API.Dto;
using backend_API.Models;
using backend_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UbicacionController: ControllerBase
    {
        private readonly ILogger<UbicacionController> _logger;
        private readonly IUbicacionService _ubicacionService;

        public UbicacionController(IUbicacionService ubicacionService, ILogger<UbicacionController> logger)
        {
            _logger = logger;
            _ubicacionService = ubicacionService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetUbicacionById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ModuloDto> GetUbicacionById(int idUbicacion)
        {
            var ubicacion = _ubicacionService.GetUbicacionById(idUbicacion);      

            if (idUbicacion == 0)
            {
                _logger.LogError("Error al recuperar el Modulo con Id: ", idUbicacion);
                return BadRequest();
            }

           if (ubicacion == null)
            {
                return NotFound();
            }
            return Ok(ubicacion);
        }
     
        [HttpPost]
        [ActionName(nameof(InsertUbicacionAsync))]
        public async Task<ActionResult<UbicacionDto>> InsertUbicacionAsync(Ubicacion ubicacion)
        {
            await _ubicacionService.InsertUbicacionAsync(ubicacion);
            return CreatedAtAction(nameof(GetUbicacionById), new { id = ubicacion.IdUbicacion }, ubicacion);
        }


    }
}
