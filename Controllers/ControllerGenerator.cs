using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
    //
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerGenerator<T, TDto> : BaseController<T, TDto>
      where T : class
      where TDto : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseRepository<T, TDto> _repository;
        public ControllerGenerator(IServiceProvider serviceProvider, IBaseRepository<T, TDto> repository)  
            : base(repository)
        {
            _serviceProvider = serviceProvider;
            _repository = repository;   
        }

        private IBaseController<T, TDto>? GetController()
            => _serviceProvider.GetService<IBaseController<T, TDto>>();         

        [HttpGet("referencia")]
        public ActionResult<string> CrearReferencia()
        {
            IBaseController<Proyecto, ProyectoDto>? proyectoController = GetController() as IBaseController<Proyecto, ProyectoDto>;

            if (proyectoController == null)
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