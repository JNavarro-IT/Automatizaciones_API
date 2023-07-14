using backend_API.Services;

namespace backend_API.Service
{
    //INTERFAZ PARA DESACOPLAR LA LÓGICA DE NEGOCIO (SIN EL CRUD)
    public interface IProyectoService
    {
        public string CrearReferencia();
    }

    //CLASE SERVICIO PARA LA LÓGICA DE NEGOCIO
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _proyectoRepository;
        private readonly ILogger<ProyectoService> _logger;

        //CONSTRUCTOR QUE ACCEDE AL REPOSITORIO DE LA BASE DE DATOS
        public ProyectoService(IProyectoRepository proyectoRepository, ILogger<ProyectoService> logger)
        {
            _proyectoRepository = proyectoRepository;
            _logger = logger;
        }

        //GENERAR UNA NUEVA REFERENCIA LEYENDO LA ÚLTIMA GUARDADA
        public string CrearReferencia()
        {
            _logger.LogInformation("Creando nueva referencia");
            var ultimoProyecto = _proyectoRepository.GetLastProyecto();
            var defaultNumReferencia = 5000;

            if (ultimoProyecto != null)
            {
                var ultimaReferencia = ultimoProyecto.Referencia;
                defaultNumReferencia = int.Parse(ultimaReferencia[(ultimaReferencia.LastIndexOf('-') + 1)..]);

            }
            var nuevoNumReferencia = defaultNumReferencia + 1;
            var nuevaReferencia = $"OFC-G{DateTime.Now.Year.ToString()[2..]}-{nuevoNumReferencia}";

            return nuevaReferencia;
        }
    }
}
