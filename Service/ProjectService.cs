using System.Globalization;
using System.Text;
using Automatizaciones_API.Models.Dto;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace Automatizaciones_API.Service
{
   // INTERFAZ QUE FUNCIONA COMO SERVICIO DEL PROYECTO PARA OTRAS CLASES, MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IProjectService
   {
      public InstalacionDto? CalcularInstalacion(InstalacionDto Instalacion);
      public double[] GetUTM(InstalacionDto Instalacion);
      public string GetCIF(string Empresa);
      public ProyectoDto GetDatosLegalizaciones(ProyectoDto Proyecto);
      public bool CheckMunicipio(UbicacionDto Ubicacion);
      public string ClonarFiles(ProyectoDto Proyecto, string[] pathsOrigin, string tempPath);
      public StringBuilder WithoutTildes(string item);
      public bool DeleteTemp(); 
   }

   //CLASE QUE SIRVE DE SERVICIO PARA REALIZAR CÁLCULOS SOBRE LA INSTALACION DE UN PROYECTO

   public class ProjectService : IProjectService
   {
      // CONSTRUCTOR POR DEFECTO
      public ProjectService() { }

      // CALCULAR LOS PARÁMETROS TÉCNICOS DE UNA INSTALACIÓN A TRAVES DE LA COMBINACIÓN DE CADENAS
      public InstalacionDto? CalcularInstalacion(InstalacionDto Instalacion)
      {
         IList<CadenaDto> Cadenas = Instalacion.Cadenas;
         if (Cadenas.Count == 0) return null;
         var hasError = false;

         try
         {
            foreach (CadenaDto c in Cadenas)
            {
               if (c.Modulo.Vca <= 1)
               {
                  c.Modulo.Vca = 50;
                  hasError = true;
               }

               if (c.Inversor.Vmin <= 1)
               {
                  c.Inversor.Vmin = 100;
                  hasError = true;
               }

               if (c.Inversor.Vmax == 0)
               {
                  c.Inversor.Vmax = 500;
                  hasError = true;
               }

               c.MinModulos = (int)Math.Ceiling(c.Inversor.Vmin / c.Modulo.Vca);
               c.MaxModulos = (int)Math.Ceiling(c.Inversor.Vmax / c.Modulo.Vca);

               c.PotenciaString = Math.Round(c.NumModulos * c.Modulo.Potencia, 2);
               c.PotenciaPico = Math.Round(c.Modulo.Potencia * c.NumModulos, 2) / 1000;
               c.PotenciaNominal = Math.Round((double)(c.NumInversores * c.Inversor.PotenciaNominal), 2);
               c.TensionString = Math.Round(c.NumModulos * c.Modulo.Vca, 2);

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
         catch (Exception ex)
         {
            if (hasError)
               throw new InvalidOperationException("Uso de datos simulados.\n Datos insuficientes para calcular la instalación.\n Actualice módulos e inversores => " + JsonConvert.SerializeObject(Instalacion));

            else throw new("EXCEPTION: " + ex.Message + " HELP => " + ex.HelpLink);
         }
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

      // OBTENER EL CIF DE UNA EMPRESA DISTRIBUIDORA DESDE UN ARCHIVO JSON 
      public string GetCIF(string Empresa)
      {
         try
         {
            string? filePath = "Utilities/Resources/CIFs.json";
            string? jsonContent = File.ReadAllText(filePath);
            JObject? jsonObj = JObject.Parse(jsonContent);
            JArray? body = (JArray?)jsonObj["body"];
            if (body != null)
            {
               foreach (JToken? row in body)
               {
                  string CIF = row[8].ToString().ToUpper();
                  string fEmpresaDB = row[1].ToString().Split(",")[0];
                  string? empresaDB = WithoutTildes(fEmpresaDB).ToString();
                  Empresa = WithoutTildes(Empresa).ToString().Replace("_", " ");

                  if (Empresa.Contains(empresaDB)) return CIF;
               }
            }
            return "ERROR => No se encontró ninguna empresa con ese nombre";

         }
         catch (Exception ex) { return "ERROR: " + ex.Message; }
      }

      // OBTENER LOS DATOS PARA LEGALIZACIONES SOBRE EL DIRECTOR DE OBRA Y LA INSPECCIÓN SEGÚN LA POTENCIA NOMINAL DEL PROYECTO
      public ProyectoDto GetDatosLegalizaciones(ProyectoDto Proyecto)
      {
         InstalacionDto Instalacion = Proyecto.Instalacion;
         if (Instalacion.TotalNominal >= 10 && Instalacion.DirectorObra == "")
         {
            Instalacion.DirectorObra = "ALBERTO ARENAS ÁLVARO";
            Instalacion.Titulacion = "INGENIERO TÉCNICO INDUSTRIAL";
            Instalacion.ColeOficial = "COLEGIO OFICIAL DE GRADUADOS E INGENIEROS TÉCNICOS INDUSTRIALES DE SEVILLA (COGITISE)";
            Instalacion.NumColegiado = "11605";
         }

         if (Instalacion.TotalNominal >= 25 && Proyecto.OCA == "")
            Proyecto.OCA = "OCA GLOBAL";

         return Proyecto;
      }

      // OBTENER SI EL PROYECTO SE ENCUENTRA DENTRO DE LOS MUNICIPIOS PARA PLAN ECO 3 DESDE UN ARCHIVO JSON
      public bool CheckMunicipio(UbicacionDto Ubicacion)
      {
         try
         {
            string? filePath = "\"Utilities/Resources/ECO3.json";
            string? jsonContent = File.ReadAllText(filePath);
            JObject? provincias = JObject.Parse(jsonContent);

            foreach (KeyValuePair<string, JToken?> prov in provincias)
            {
               if (WithoutTildes(prov.Key).Equals(WithoutTildes(Ubicacion.Provincia)))
               {
                  JObject? municipios = (JObject?)prov.Value;
                  if (municipios != null)
                  {
                     foreach (KeyValuePair<string, JToken?> muni in municipios)
                     {
                        if (WithoutTildes(muni.Key).Equals(WithoutTildes(Ubicacion.Municipio)))
                           return true;
                     }
                  }
               }
            }
            return false;

         }
         catch (Exception ex) { throw new Exception("EXCEPTION: " + ex.Message); }
      }

      //_________________________________________________________________________________\\
      //--------------------------------- COMMONS METHODs ---------------------------------\\

      // REALIZA UNA COPIA DE ARCHIVOS DEL PROYECTO DE UNA CARPETA ORIGEN A OTRA DE DESTINO 
      public string ClonarFiles(ProyectoDto Proyecto, string[] pathsOrigin, string tempPath)
      {
         try
         {
            foreach (string path in pathsOrigin)
            {
               string nameFile = Path.GetFileName(path).Replace("NAME", Proyecto.Cliente.Nombre); ;
               string pathDestine = Path.Combine(tempPath, nameFile);
               File.Copy(path, pathDestine, true);

            }
            if (tempPath.IsNullOrEmpty()) return "ERROR WORD=> Clonacion de archivos no";

            return tempPath;

         }
         catch (Exception ex) { return "EXCEPTION WORD => " + ex.Message; }
      }


      // OBTENER UNA CADENA DE TEXTO SIN TILDES
      public StringBuilder WithoutTildes(string item)
      {
         StringBuilder sb = new();
         string format = item.Normalize(NormalizationForm.FormD);
         foreach (char f in format)
         {
            if (CharUnicodeInfo.GetUnicodeCategory(f) != UnicodeCategory.NonSpacingMark)
               _ = sb.Append(f);

         }
         return sb;
      }

      public bool DeleteTemp()
      {
         var tempFolder = "Utilities/Temp";
         if (Directory.Exists(tempFolder))
         {
            var oldFiles = Directory.GetFiles(tempFolder);
            if (oldFiles.Length > 0)
               foreach (var file in oldFiles)
                  new FileInfo(file).Delete();

            if (oldFiles.Length == 0) Directory.Delete(tempFolder, true);
            else return false;

            if (Directory.Exists(tempFolder)) return false;
            else return true;

         }
         else return true;
      }
   }
}
