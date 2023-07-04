
using backend_API.Dto;
using backend_API.Stores;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace backend_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModulosController : ControllerBase
    {
        private readonly ILogger<ModulosController> _logger;
        public ModulosController(ILogger<ModulosController> logger)
        {
            _logger = logger;
        }

        // GET: Modulos/modulosList
        [HttpGet("modulosList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ModuloDto>> GetModulosList()
        {
            //var usersList = await _context.Users.ToListAsync();

            _logger.LogInformation("Obtener los Modulos");
            return Ok(ModuloStore.modulosList);
        }

        // GET: Modulos/id
        [HttpGet("{id:int}", Name ="GetModulo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ModuloDto> GetModulo(int id)
        {
            //var userDto = await _context.FindAsync(id);
            if(id==0)
            {
                _logger.LogError("Error al recuperar el Modulo con Id: " + id);
                return BadRequest();
            }

            var modulo = ModuloStore.modulosList.FirstOrDefault(m => m.Id == id);

            if(modulo==null)
            {
                return NotFound();
            }
            return Ok(modulo);
        }

        // POST: Modulos/createModulo
        [HttpPost("createModulo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ModuloDto> CreateModulo([FromBody] ModuloDto moduloDto)
        {
            if(ModuloStore.modulosList.FirstOrDefault(m => m.Nombre.ToUpper() == moduloDto.Nombre.ToUpper()) != null)
            {
                ModelState.AddModelError("NombreExiste", "El modelo con ese nombre ya existe");
            }
            if(moduloDto == null)
            {
                return BadRequest(moduloDto);
            }
            if(moduloDto.Id>0) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            moduloDto.Id = ModuloStore.modulosList.OrderByDescending(m => m.Id).FirstOrDefault().Id+1;
            ModuloStore.modulosList.Add(moduloDto);

            return CreatedAtRoute("GetModulo", new {id = moduloDto.Id}, moduloDto);

        }

        // PUT: Modulos/updateModulo/{id}
        [HttpPut("updateModulo/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateModulo(int id, [FromBody] ModuloDto moduloDto)
        {

            if(moduloDto==null || id != moduloDto.Id)
            {
                return BadRequest();
            }

            var modulo = ModuloStore.modulosList.FirstOrDefault(m => m.Id == id);

            modulo.Nombre = moduloDto.Nombre;
            modulo.Potencia = moduloDto.Potencia;
            modulo.Icc = moduloDto.Icc;

            return NoContent();
        }

        // DELETE: ModulosI/deleteModulo/{id}
        [HttpDelete("deleteModuo/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteModulo(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }

            var modelo = ModuloStore.modulosList.FirstOrDefault(m => m.Id == id);

            if (modelo == null)
            {
                return NotFound();
            }

            ModuloStore.modulosList.Remove(modelo);

            return NoContent();
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

            var modulo = ModuloStore.modulosList.FirstOrDefault(m => m.Id == id);

            patchDto.ApplyTo(modulo, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }

}