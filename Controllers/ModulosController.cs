using AutoMapper;
using backend_API.Dto;
using backend_API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //CONTROLADOR PARA LAS PETICIONES RELACIONADAS CON LA ENTIDAD MÓDULO
    [ApiController]
    [Route("api/modulos")]
    public class ModulosController : ControllerBase
    {
       private readonly IModuloRepository _moduloRepository;
       private readonly ILogger<ModulosController> _logger;

        //CONSTRUCTOR QUE ACCEDE AL REPOSITORIO CRUD
        public ModulosController(IModuloRepository moduloRepository, ILogger<ModulosController> logger)
        {
            _moduloRepository = moduloRepository;
            _logger = logger;
        }

        // GET: OBTERNER LISTA DE MODULOS
        [HttpGet("modulosList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ModuloDto>>> GetModulosListAsync()
        {
            _logger.LogInformation("Obteniendo lista de módulos");
            var modulosList = await _moduloRepository.GetModulosListAsync();
            if(modulosList == null)
            {
                return NoContent();
            }
            return Ok(modulosList);
        }

        // GET: OBTENER UN MODULO POR SU ID
        [HttpGet("byId/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ModuloDto> GetModulo(int id)
        {
            _logger.LogInformation("Obteniendo modulo por su ID...");
            if(id == 0) 
                return BadRequest();
            
            var modulo = _moduloRepository.GetModulo(id);
            if(modulo == null)
                return NotFound();

            return Ok(modulo);
        }
        
        // POST: CREAR UN NUEVO MODULO
        [HttpPost("createModulo/{modulo:Modulo}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<ModuloDto>> CreateModulo([FromBody] ModuloDto moduloDto)
        {
            if(moduloDto == null)
                return BadRequest(moduloDto);
                  
            if(moduloDto.Id>0) 
                return StatusCode(StatusCodes.Status406NotAcceptable);

            try
            {
                await _moduloRepository.CreateModuloAsync(moduloDto);
                return CreatedAtRoute("GetModulo", new { id = moduloDto.Id }, moduloDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // PUT: ACTUALIZAR UN MODELO
        [HttpPut("updateModulo/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ModuloDto>> UpdateModulo(int id, [FromBody] ModuloDto moduloDto)
        {
            
            if(moduloDto==null || id != moduloDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _moduloRepository.UpdateModuloAsync(moduloDto);
                return Ok(moduloDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }          
        }

        // DELETE: BORRAR UN MODELO
        [HttpDelete("deleteModuo/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ModuloDto>> DeleteModulo(int id, [FromBody] ModuloDto moduloDto)
        {
            if(id==0)
            {
                return BadRequest();
            }

            var isDelete = await _moduloRepository.DeleteModuloAsync(moduloDto);

            if (!isDelete)
            {
                return NotFound();
            }
            return Ok();
        }

        // PATCH: Modulos/patchModulo/{id}
        [HttpPatch("patchModulo/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialModulo(int id, JsonPatchDocument<ModuloDto> patchDto)
        {

            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var moduloDto = _moduloRepository.GetModulo(id);

            if (moduloDto == null)
                return NotFound();

            patchDto.ApplyTo(moduloDto, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _moduloRepository.UpdateModuloAsync(moduloDto);
                return Ok(moduloDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}