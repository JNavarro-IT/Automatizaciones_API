using backend_API.Dto;


namespace backend_API.Service
{
    public interface IInstalacionService
    {
        public InstalacionDto InstalacionCalculated(InstalacionDto instalacion);
    }

    public class InstalacionService : IInstalacionService
    {
        public InstalacionService()
        {
        }

        public InstalacionDto InstalacionCalculated(InstalacionDto instalacion)
        {
            var cadenas = instalacion.Cadenas;
            var inversores = instalacion.Inversores;

            foreach (CadenaDto cadenaDto in cadenas)
            {
                cadenaDto.MinModulos = (int)(Math.Ceiling(cadenaDto.Inversor.Vmin / cadenaDto.Modulo.Vmp));
                cadenaDto.MaxModulos = (int)(Math.Ceiling(cadenaDto.Inversor.Vmax / cadenaDto.Modulo.Vca));
                cadenaDto.PotenciaPico = cadenaDto.NumModulos * cadenaDto.Modulo.Potencia / 1000;

                instalacion.TotalPico += cadenaDto.PotenciaPico;
            }
            
            foreach (InversorDto inversor in inversores)
            {
                instalacion.TotalNominal += inversor.PotenciaNominal;
            }
            return instalacion;
        }
    }
}
