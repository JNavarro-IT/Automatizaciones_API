using backend_API.Dto;
using backend_API.Models;
using backend_API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //
    [ApiController]
    [Route("generator")]
    public class ControllerGenerator : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public ControllerGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        private IBaseController<T, TDto>? GetController<T, TDto>()
        where T : ModelBase
        where TDto : DtoBase
            => _serviceProvider.GetService<IBaseController<T, TDto>>();


        [HttpGet("referencia")]
        public ActionResult<string> CrearReferencia()
        {
            if (GetController<ModelBase, DtoBase>() is not IBaseController<Proyecto, ProyectoDto> proyectoController)
                return NotFound("El controlador de proyecto no se encontró");

            var proyectosList = proyectoController.GetListAsync().Result.Value;

            if (proyectosList == null)
                return NotFound("Lista de proyectos no encontrada");

            var ultimoProyecto = proyectosList.OrderByDescending(p => p.IdProyecto).FirstOrDefault();
            var defaultNumReferencia = 5000;

            if (ultimoProyecto != null)
            {
                var ultimaReferencia = ultimoProyecto.Referencia;
                defaultNumReferencia = int.Parse(ultimaReferencia[(ultimaReferencia.LastIndexOf('-') + 1)..]);
                var nuevoNumReferencia = defaultNumReferencia + 1;
                var nuevaReferencia = $"OFC-G{DateTime.Now.Year.ToString()[2..]}-{nuevoNumReferencia}";
                return Ok(nuevaReferencia);
            }
            else
            {
                return NoContent();
            }
        }
    }
}