using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;

namespace backend_API.Service
{
    /*
    * INTERFAZ QUE FUNCIONA COMO SERCVICIO ENINYECCIÓN DE
    * DEPENDENCIAS PARA OTRAS CLASES
    */
    public interface IInstalacionService
    {
        public InstalacionDto CalcularInstalacion (InstalacionDto instalación);
    }

    public class InstalacionService : IInstalacionService
    {
        private readonly IBaseRepository<Instalacion, InstalacionDto> _instalacionRepository;
        private readonly IBaseRepository<Inversor, InversorDto> _inversorRepository;
        private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;

        public InstalacionService(IBaseRepository<Instalacion, InstalacionDto> instalacionRepository, 
            IBaseRepository<Inversor, InversorDto> inversorRepository,
            IBaseRepository<Modulo, ModuloDto> moduloRepository) {

            _instalacionRepository = instalacionRepository;
            _inversorRepository = inversorRepository;
            _moduloRepository = moduloRepository;
        }

        public InstalacionDto CalcularInstalacion(InstalacionDto Instalacion)
        {
            var Cadenas = Instalacion.Cadenas;

            foreach (CadenaDto c in Cadenas)
            {
                c.MinModulos = (int)(Math.Ceiling(c.Inversor.Vmin / c.Modulo.Vmp));
                c.MaxModulos = (int)(Math.Ceiling(c.Inversor.Vmax / c.Modulo.Vca));
                if (c.NumModulos == 0)
                    c.NumModulos = c.MinModulos;

                c.PotenciaString = c.NumModulos * c.Modulo.Potencia;
                c.PotenciaPico = Math.Round((c.PotenciaString / 1000), 2);
                c.TensionString = Math.Round((c.NumModulos * c.Modulo.Vca), 2);

                // Valores reiniciados en cada iteración
                Instalacion.TotalModulos += c.NumModulos;
                Instalacion.TotalCadenas += c.NumCadenas;
                Instalacion.TotalPico += Math.Round(c.PotenciaPico, 2);
                Instalacion.TotalNominal += c.Inversor.PotenciaNominal;
            }
            return Instalacion;
        }

    }
}
