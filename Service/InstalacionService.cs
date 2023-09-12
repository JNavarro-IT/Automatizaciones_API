using backend_API.Dto;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace backend_API.Service
{
   /*
   * INTERFAZ QUE FUNCIONA COMO SERCVICIO ENINYECCIÓN DE
   * DEPENDENCIAS PARA OTRAS CLASES
   */
   public interface IInstalacionService
   {
      public InstalacionDto CalcularInstalacion(InstalacionDto Instalacion);
      public double[] GetUTM(InstalacionDto Instalacion);
   }

   public class InstalacionService : IInstalacionService
   {
      public InstalacionService() { }
      public InstalacionDto CalcularInstalacion(InstalacionDto Instalacion)
      {
         var Cadenas = Instalacion.Cadenas;

         foreach (CadenaDto c in Cadenas)
         {
            c.MinModulos = (int)(Math.Ceiling(c.Inversor.Vmin / c.Modulo.Vca));
            c.MaxModulos = (int)(Math.Ceiling(c.Inversor.Vmax / c.Modulo.Vca));

            c.PotenciaString = c.NumModulos * c.Modulo.Potencia;
            c.PotenciaPico = Math.Round(c.Modulo.Potencia * c.NumModulos, 2) / 1000;
            c.PotenciaNominal = (double)(c.NumInversores * c.Inversor.PotenciaNominal);
            c.TensionString = Math.Round((c.NumModulos * c.Modulo.Vca), 2);

            Instalacion.TotalModulos += c.NumModulos;
            Instalacion.TotalCadenas += c.NumCadenas;
            Instalacion.TotalInversores += c.NumInversores;
            Instalacion.TotalPico += Math.Round(c.PotenciaPico, 2);
            Instalacion.TotalNominal += c.PotenciaNominal;
         }
         return Instalacion;
      }

      public double[] GetUTM(InstalacionDto Instalacion)
      {
         CoordinateTransformationFactory factory = new();
         CoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
         ProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(30, true);
         ICoordinateTransformation transformation = factory.CreateFromCoordinateSystems(wgs84, utm);
         double[] latLng = { Instalacion.CoordXConexion, Instalacion.CoordYConexion };
         double[] latLngUTM = transformation.MathTransform.Transform(latLng);
         Console.Error.WriteLine(latLngUTM);
         return latLngUTM;
      }
   }
}
