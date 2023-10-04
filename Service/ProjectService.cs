using System.Globalization;
using System.Text;
using backend_API.Dto;
using Newtonsoft.Json.Linq;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace backend_API.Service
{
   // INTERFAZ QUE FUNCIONA COMO SERVICIO DEL PROYECTO PARA OTRAS CLASES, MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IProjectService
   {
      public InstalacionDto CalcularInstalacion(InstalacionDto Instalacion);
      public double[] GetUTM(InstalacionDto Instalacion);
      public string ClonarFiles(ProyectoDto Proyecto, string[] pathsOrigin, string folderEnd);
      public string GetCIF(string empresa);
      public ProyectoDto GetDatosLegalizaciones(ProyectoDto Proyecto);
      public bool CheckMunicipio(UbicacionDto Ubicacion);
      public StringBuilder WithoutTildes(string item);
   }

   /*
   * CLASE QUE SIRVE DE SERVICIO PARA REALIZAR CÁLCULOS SOBRE LA INSTALACION DE UN PROYECTO
   */
   public class ProjectService : IProjectService
   {
      // CONSTRUCTOR POR DEFECTO
      public ProjectService() { }

      // CALCULAR LOS PARÁMETROS TÉCNICOS DE UNA INSTALACIÓN A TRAVES DE LA COMBINACIÓN DE CADENAS
      public InstalacionDto CalcularInstalacion(InstalacionDto Instalacion)
      {
         var Cadenas = Instalacion.Cadenas;

         foreach (CadenaDto c in Cadenas)
         {
            c.MinModulos = (int)(Math.Ceiling(c.Inversor.Vmin / c.Modulo.Vca));
            c.MaxModulos = (int)(Math.Ceiling(c.Inversor.Vmax / c.Modulo.Vca));

            c.PotenciaString = Math.Round(c.NumModulos * c.Modulo.Potencia, 2);
            c.PotenciaPico = Math.Round(c.Modulo.Potencia * c.NumModulos, 2) / 1000;
            c.PotenciaNominal = Math.Round((double)(c.NumInversores * c.Inversor.PotenciaNominal), 2);
            c.TensionString = Math.Round((c.NumModulos * c.Modulo.Vca), 2);

            Instalacion.TotalModulos += c.NumModulos;
            Instalacion.TotalCadenas += c.NumCadenas;
            Instalacion.TotalInversores += c.NumInversores;
            Instalacion.TotalPico += Math.Round(c.PotenciaPico, 2);
            Instalacion.TotalNominal += Math.Round(c.PotenciaNominal, 2);

            c.IdInversor = c.Inversor.IdInversor;
            c.IdModulo = c.Modulo.IdModulo;
         }
         return Instalacion;
      }

      // CALCULAR EL LATLNG POR EL SISTEMA DE UTM-30 LAS COORDENADAS DE LA INSTALACIÓN
      public double[] GetUTM(InstalacionDto Instalacion)
      {
         CoordinateTransformationFactory factory = new();
         CoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
         ProjectedCoordinateSystem utm = ProjectedCoordinateSystem.WGS84_UTM(30, true);
         ICoordinateTransformation transformation = factory.CreateFromCoordinateSystems(wgs84, utm);
         double[] latLng = { Instalacion.CoordXConexion, Instalacion.CoordYConexion };
         double[] latLngUTM = transformation.MathTransform.Transform(latLng);
         return latLngUTM;
      }

      // REALIZA UNA COPIA DE ARCHIVOS DEL PROYECTO DE UNA CARPETA ORIGEN A OTRA DE DESTINO 
      public string ClonarFiles(ProyectoDto Proyecto, string[] pathsOrigin, string folderEnd)
      {
         try
         {
            foreach (string path in pathsOrigin)
            {
               var nameFileOrigin = Path.GetFileName(path).Replace("NAME", Proyecto.Cliente.Nombre); ;
               var pathFileEnd = Path.Combine(folderEnd, nameFileOrigin);
               File.Copy(path, pathFileEnd, true);
            }
            return folderEnd;
         }
         catch (Exception error) { return "No se han creado los archivos. ERROR: " + error.Message; }
      }

      // OBTENER EL CIF DE UNA EMPRESA DISTRIBUIDORA DESDE UN ARCHIVO JSON 
      public string GetCIF(string? Empresa)
      {
         try
         {
            string? filePath = "SeedData/CIFs.json";
            string? jsonContent = File.ReadAllText(filePath);
            JObject? jsonObj = JObject.Parse(jsonContent);
            JArray? body = jsonObj["body"] as JArray;

            foreach (string? row in body)
            {
               string? CIF = row[8].ToString().ToUpper();
               string? fEmpresaDB = row[1].ToString().Split(",")[0];
               string? empresaDB = WithoutTildes(fEmpresaDB).ToString();
               Empresa = WithoutTildes(Empresa).ToString().ToLower().Replace("_"," ");

               if (empresaDB.Contains(Empresa)) return CIF;
               else return "ERROR => No se encontró ninguna empresa con ese nombre";

            } return "ERROR al procesar el archivo JSON: ";

         }     catch (Exception ex) { return "ERROR: " + ex.Message; }
      }

      // OBTENER LOS DATOS PARA LEGALIZACIONES SOBRE EL DIRECTOR DE OBRA Y LA INSPECCIÓN SEGÚN LA POTENCIA NOMINAL DEL PROYECTO
      public ProyectoDto GetDatosLegalizaciones(ProyectoDto Proyecto)
      {
         var Instalacion = Proyecto.Instalacion;
         if (Instalacion.TotalNominal >= 10 && Instalacion.DirectorObra == "")
         {
            Instalacion.DirectorObra = "ALBERTO ARENAS ÁLVARO";
            Instalacion.Titulacion = "INGENIERO TÉCNICO INDUSTRIAL";
            Instalacion.ColeOficial = "COLEGIO OFICIAL DE GRADUADOS E INGENIEROS TÉCNICOS INDUSTRIALES DE SEVILLA(COGITISE)";
            Instalacion.NumColegiado = "11605";
         }

         if (Instalacion.TotalNominal >= 25 && Proyecto.OCA == "") Proyecto.OCA = "OCA GLOBAL";

         return Proyecto;
      }

      // OBTENER SI EL PROYECTO SE ENCUENTRA DENTRO DE LOS MUNICIPIOS PARA PLAN ECO 3 DESDE UN ARCHIVO JSON
      public bool CheckMunicipio(UbicacionDto Ubicacion)
      {
         try
         {
            string? filePath = "SeedData/ECO3.json";
            string? jsonContent = File.ReadAllText(filePath);
            JObject? provincias = JObject.Parse(jsonContent);

            foreach (var prov in provincias)
            {
               if (WithoutTildes(prov.Key).Equals(WithoutTildes(Ubicacion.Provincia)))
               {
                  JObject? municipios = (JObject?)prov.Value;
                  foreach (var muni in municipios)
                     if (WithoutTildes(muni.Key).Equals(WithoutTildes(Ubicacion.Municipio))) return true;
               }
            } return false;

         } catch (Exception ex) { throw new Exception("EXCEPTION: " + ex.Message); }
      }

      // OBTENER UNA CADENA DE TEXTO SIN TILDES
      public StringBuilder WithoutTildes(string item)
      {
         StringBuilder sb = new();
         var format = item.Normalize(NormalizationForm.FormD);
         foreach (char f in format)
            if (CharUnicodeInfo.GetUnicodeCategory(f) != UnicodeCategory.NonSpacingMark) sb.Append(f);

         return sb;
      }
   }
}
           
   

     
