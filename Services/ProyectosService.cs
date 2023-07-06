using backend_API.Context;
using backend_API.Interfaces;


namespace backend_API.Services
{
    public class ProyectosService : IProyectosService
    {
        private readonly DBContext _dbContext;

        public ProyectosService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string CrearNuevaReferencia()
        {
            var ultimoProyecto = _dbContext.Proyectos.OrderByDescending(c => c.IdProyecto).FirstOrDefault();
            var ultimoNumReferencia = 4300;

            if (ultimoProyecto != null)
            {
                var ultimaReferencia = ultimoProyecto.Referencia;
                ultimoNumReferencia = int.Parse(ultimaReferencia[(ultimaReferencia.LastIndexOf('-') + 1)..]);

            }
            var nuevoNumReferencia = ultimoNumReferencia + 1;
            var nuevaReferencia = $"OFC-G{DateTime.Now.Year.ToString()[2..]}-{nuevoNumReferencia}";

            return nuevaReferencia;
        }
    }
}
