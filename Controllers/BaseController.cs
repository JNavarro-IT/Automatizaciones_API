using Automatizaciones_API.Repository;
using Automatizaciones_API.Utilities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;

namespace Automatizaciones_API.Controllers
{
   //INTERFAZ PRINCIPAL QUE IMPLENTA UN CRUD BASICO DE LOS CONTROLADORES
   public interface IBaseController<T, TDto>
        where T : ModelBase
        where TDto : DtoBase
   {
      public Task<ActionResult<IEnumerable<TDto>>> GetListAsync();
      public ActionResult<TDto> GetByIdentity(object identity);
      public Task<ActionResult<TDto>> CreateEntity(object identity, [FromBody] TDto dto);
      public Task<ActionResult<TDto>> UpdateEntity(object identity, [FromBody] TDto dto);
      public Task<ActionResult<TDto>> DeleteEntity(object identity, [FromBody] TDto dto);
      public Task<ActionResult> UpdatePartialEntity(object identity, JsonPatchDocument<TDto> patchDto);
   }

   /* 
   * CLASE ABSTRACTA PARA REPRESENTAR UN CONTROLADOR CRUD ECAPSULADO PARA LA BASE DE DATOS
   * LOS MODELOS QUEDAN AISLADOS DEL FRONTENT Y LA INFORMACIÓN SE TRANSPORTA EN CLASES DTO  
   */
   [ApiController]
   [Route("api")]
   public class BaseController<T, TDto> : ControllerBase
       where T : ModelBase
       where TDto : DtoBase
   {
      private readonly IBaseRepository<T, TDto> _baseRepository;

      //CONSTRUCTOR CON INYECCION DE DEPENDENCIA

      public BaseController(IBaseRepository<T, TDto> repository)
      {
         _baseRepository = repository;
      }

      //GET: OBTENER LA LISTA DE UNA ENTIDAD
      [HttpGet("list")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<IEnumerable<TDto>>> GetListAsync()
      {
         IEnumerable<TDto> entitiesList = await _baseRepository.GetEntitiesList();
         return entitiesList == null ? NoContent() : Ok(entitiesList);
      }

      //GET: OBTENER UNA ENTIDAD MEDIANTE EL ID
      [HttpGet("byIdentity/{identity:int}")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<TDto>> GetByIdentity(object identity)
      {
         if (identity == null) return BadRequest("El identificador enviado es nulo");

         TDto? entity = await _baseRepository.GetEntityDto(identity);
         return entity == null ? NotFound("Objeto no encontrado con ese identificador") : Ok(entity);
      }

      //POST: CREAR UNA ENTIDAD
      [HttpPost("create")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      public async Task<ActionResult<TDto>> CreateEntity(object identity, [FromBody] TDto dto)
      {
         if (dto == null || identity == null) return BadRequest("No se ha enviado ningún dato");

         try
         {
            TDto newDto = await _baseRepository.CreateEntity(dto);
            return newDto != null ? Ok(newDto) : NoContent();

         }
         catch (Exception ex) { return BadRequest(ex.ToString()); }
      }

      //PUT: ACTUALIZAR UNA ENTIDAD POR UN IDENTIFICADOR
      [HttpPut("update")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<ActionResult<TDto>> UpdateEntity(object identity, [FromBody] TDto dto)
      {
         if (dto == null || identity == null) return BadRequest("No se ha enviado ningún dato");

         Task<TDto?> dtoDB = _baseRepository.GetEntityDto(identity);
         if (dtoDB == null) return NotFound("Objeto no encontrado con el identificador enviado");

         try
         {
            int updated = await _baseRepository.UpdateEntity(dto);
            return updated > 0 ? Ok(dto) : NotFound("No se ha actualizado ningún objeto");

         }
         catch (Exception ex) { return BadRequest(ex.ToString()); }
      }

      //BORRAR UNA ENTIDAD POR EL ID
      [HttpDelete("delete")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<ActionResult<TDto>> DeleteEntity(object identity, [FromBody] TDto dto)
      {
         if (identity == null) return BadRequest("No se ha pasado ningún identificador");

         int deleted = await _baseRepository.DeleteEntity(dto);
         return deleted <= 0 ? NotFound("No se ha eliminado ningún objeto") : Ok(dto);
      }

      //PATCH: ACTUALIZAR UNA ENTIDAD DE FORMA PARCIAL
      [HttpPatch("patch")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<ActionResult> UpdatePartialEntity(object identity, JsonPatchDocument<TDto> patchDto)
      {
         if (patchDto == null || identity == null) return BadRequest("No se han detectado parámetros para actualizar");

         TDto? entityDto = await _baseRepository.GetEntityDto(identity);
         if (entityDto == null) return NotFound("No se ha encontrado ningún objeto con ese identificador");

         patchDto.ApplyTo(entityDto, ModelState as IObjectAdapter);

         if (!ModelState.IsValid) return BadRequest("El modelo no es válido" + ModelState);

         try
         {
            int updated = await _baseRepository.UpdateEntity(entityDto);
            return updated <= 0 ? BadRequest("No se ha actualizado ningún objeto") : Ok(entityDto);

         }
         catch (Exception ex) { return BadRequest(ex.ToString()); }
      }
   }
}