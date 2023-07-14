using AutoMapper;
using backend_API.Dto;
using backend_API.Models.Data;

namespace backend_API.Services
{
    //INTERFAZ PARA DESACOPLAR EL CONTROLADOR DEL ACCESO A DATOS
    public interface IProyectoRepository
    {
        public ProyectoDto GetLastProyecto();
        public void InsertProyecto(ProyectoDto proyectoDto);
    }

    //CLASE REPOSITORIO CRUD PARA LA ENTIDAD CLIENTE
    public class ProyectoRepository : IProyectoRepository
    {
        protected readonly DBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProyectoRepository> _logger;

        //CONSTRUCTOR CON ACCESO A LA BD
        public ProyectoRepository(DBContext dbContext, IMapper mapper, ILogger<ProyectoRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        //OBTENER EL ÚLTIMO PROYECTO REEGISTRADO
        public ProyectoDto GetLastProyecto()
        {
            _logger.LogInformation("Obteniendo el último proyecto");
            var ultimoProyecto = _dbContext.Proyectos.OrderByDescending(p => p.IdProyecto).FirstOrDefault();
            return _mapper.Map<ProyectoDto>(ultimoProyecto);
        }

        public void InsertProyecto(ProyectoDto proyectoDto)
        {
            throw new NotImplementedException();
        }

      

        //OBTENER LA LISTA DE PROYECTOS

    }

 
}
