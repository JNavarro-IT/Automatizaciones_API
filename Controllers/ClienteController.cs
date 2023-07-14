using AutoMapper;
using backend_API.Dto;
using backend_API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //CONTROLADOR DE LAS PETICIONES HTTP PARA LA ENTIDAD CLIENTE
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController: ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteController> _logger;

        //CONSTRUCTOR QUE ACCEDE AL REPOSITORIO CRUD
        public ClienteController(IClienteRepository clienteRepository, ILogger<ClienteController> logger)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
        }

        // GET: OBTERNER LISTA DE CLIENTES
        [HttpGet("clientesList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientesListAsync()
        {
            _logger.LogInformation("Obteniendo lista de clientes");
            var clientesList = await _clienteRepository.GetClientesListAsync();
            if (clientesList == null)
            {
                return NoContent();
            }
            return Ok(clientesList);
        }

        // GET: OBTENER UN CLIENTE POR SU ID
        [HttpGet("byId/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ClienteDto> GetCliente(int id)
        {
            _logger.LogInformation("Obteniendo cliente por su ID...");
            if (id == 0)
                return BadRequest();

            var cliente = _clienteRepository.GetCliente(id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        // POST: CREAR UN NUEVO CLIENTE
        [HttpPost("createCliente/{cliente:Cliente}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] ClienteDto clienteDto)
        {
            if (clienteDto == null)
                return BadRequest(clienteDto);

            if (clienteDto.IdCliente > 0)
                return StatusCode(StatusCodes.Status406NotAcceptable);

            try
            {
                await _clienteRepository.CreateClienteAsync(clienteDto);
                return CreatedAtRoute("GetCliente", new { id = clienteDto.IdCliente }, clienteDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // PUT: ACTUALIZAR UN CLIENTE
        [HttpPut("updateliente/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> UpdateCliente(int id, [FromBody] ClienteDto clienteDto)
        {

            if (clienteDto == null || id != clienteDto.IdCliente)
            {
                return BadRequest();
            }

            try
            {
                await _clienteRepository.UpdateClienteAsync(clienteDto);
                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // DELETE: BORRAR UN MODELO
        [HttpDelete("deleteModuo/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> DeleteCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var isDelete = await _clienteRepository.DeleteClienteAsync(clienteDto);

            if (!isDelete)
            {
                return NotFound();
            }
            return Ok();
        }

        // PATCH: ACTUALIZAR PARCIALMENTE
        [HttpPatch("patchCliente/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialCliente(int id, JsonPatchDocument<ClienteDto> patchDto)
        {

            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var clienteDto = _clienteRepository.GetCliente(id);

            if (clienteDto == null)
                return NotFound();

            patchDto.ApplyTo(clienteDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _clienteRepository.UpdateClienteAsync(clienteDto);
                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
