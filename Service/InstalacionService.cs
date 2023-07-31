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
        public Task<InstalacionDto> InstalacionCalculated(InstalacionDto instalacion);
    }

    public class InstalacionService : IInstalacionService
    {
        private readonly IBaseRepository<Instalacion, InstalacionDto> _instalacionRepository;
        private readonly IBaseRepository<Inversor, InversorDto> _inversorRepository;
        private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;
        public InstalacionService(IBaseRepository<Instalacion, InstalacionDto> instalacionRepository, IBaseRepository<Inversor, InversorDto> inversorRepository,
            IBaseRepository<Modulo, ModuloDto> moduloRepository) {

            _instalacionRepository = instalacionRepository;
            _inversorRepository = inversorRepository;
            _moduloRepository = moduloRepository;
        }
      
        public async Task<InstalacionDto> InstalacionCalculated(InstalacionDto instalacion)
        {
            var cadenas = instalacion.Cadenas;

            foreach (CadenaDto cadenaDto in cadenas)
            {
                var Inversor = await _inversorRepository.GetEntity(cadenaDto.IdInversor);
                var Modulo = await _moduloRepository.GetEntity(cadenaDto.IdModulo);

                cadenaDto.MinModulos = (int)(Math.Ceiling(Inversor.Vmin / Modulo.Vmp));
                cadenaDto.MaxModulos = (int)(Math.Ceiling(Inversor.Vmax / Modulo.Vca));
                cadenaDto.PotenciaPico = cadenaDto.NumModulos * Modulo.Potencia / 1000;

                instalacion.TotalPico += cadenaDto.PotenciaPico;
                instalacion.TotalNominal += Inversor.PotenciaNominal;            
            }
            return instalacion;
        }
    }
}
