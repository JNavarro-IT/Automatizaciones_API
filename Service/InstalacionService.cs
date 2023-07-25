using backend_API.Dto;

namespace backend_API.Service
{
    /*
    * INTERFAZ QUE FUNCIONA COMO SERCVICIO ENINYECCIÓN DE
    * DEPENDENCIAS PARA OTRAS CLASES
    */
    public interface IInstalacionService
    {
        public InstalacionDto InstalacionCalculated(InstalacionDto instalacion);
    }

    public class InstalacionService : IInstalacionService
    {
        public InstalacionDto InstalacionCalculated(InstalacionDto instalacion)
        {
            var cadenas = instalacion.Cadenas;
            List<InversorDto> inversoresTipo = new();

            foreach (CadenaDto cadenaDto in cadenas)
            {
                cadenaDto.MinModulos = (int)(Math.Ceiling(cadenaDto.Inversor.Vmin / cadenaDto.Modulo.Vmp));

                cadenaDto.MaxModulos = (int)(Math.Ceiling(cadenaDto.Inversor.Vmax / cadenaDto.Modulo.Vca));

                cadenaDto.PotenciaPico = cadenaDto.NumModulos * cadenaDto.Modulo.Potencia / 1000;

                instalacion.TotalPico += cadenaDto.PotenciaPico;

                if (!inversoresTipo.Contains(cadenaDto.Inversor))
                {
                    inversoresTipo.Add(cadenaDto.Inversor);
                }
            }

            foreach (InversorDto inversor in inversoresTipo)
            {
                instalacion.TotalNominal += inversor.PotenciaNominal;
            }
            return instalacion;
        }
    }
}
