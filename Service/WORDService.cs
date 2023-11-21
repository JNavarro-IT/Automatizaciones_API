using Automatizaciones_API.Models.Dto;
using System.Globalization;
using System.Text.RegularExpressions;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Automatizaciones_API.Service
{
   // INTERFAZ QUE DA SERVICIO A OTRAS CLASES PARA USAR ARCHIVOS WORD MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IWORDService
   {
      public string InitServiceWORD(ProyectoDto Proyecto);
      public Dictionary<string, string?> CreateMap(ProyectoDto Proyecto);
      public string CreateMemories(Dictionary<string, string?> MapWORD, string[] pathsWORD);
   }

   // CLASE QUE IMPLEMENTA IWORDService PARA MANEJO Y RELLENO DE DOCUMENTOS WORD
   public class WORDService : IWORDService
   {
      private readonly IProjectService _projectService;

      // CONSTRUCTOR POR PARÁMETROS PARA INYECTAR DEPENDENCIAS
      public WORDService(IProjectService projectService)
      {
         _projectService = projectService;
      }

      // INICIALIZAR EL PROCESO PARA RELLENAR ARCHIVOS WORD CON UN DICCIONARIO DE CLAVE-VALOR (Los archivos Word deben está customizados para el mapa)
      public string InitServiceWORD(ProyectoDto Proyecto)
      {
         var tempPath = Directory.CreateDirectory("Utilities/Temp").FullName;
         string[]? pathsOrigin = Directory.GetFiles("Utilities/Resources/TemplatesWORD");

         var resultClon = _projectService.ClonarFiles(Proyecto, pathsOrigin, tempPath);

         if (resultClon.StartsWith("ERROR") || resultClon.StartsWith("EXCEPTION)")) return resultClon;
         Dictionary<string, string?> MapWORD = CreateMap(Proyecto);
         var pathsWORD = Directory.GetFiles(tempPath);
         string resultMemorias = CreateMemories(MapWORD, pathsWORD);

         if (resultMemorias.StartsWith("ERROR") || resultMemorias.StartsWith("EXCEPTION)")) return resultMemorias;
         else return tempPath;
      }

      // GENERAR UN DICCIONARIO CON FORMATO CLAVE-VALOR COMO REFERENCIA PARA INTRODUCIR LOS VALORES DE UN PROYECTO
      public Dictionary<string, string?> CreateMap(ProyectoDto Proyecto)
      {
         string Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Proyecto.Fecha.Month);
         Mes = Mes.ToUpper().ToCharArray()[0] + Mes[1..];

         ClienteDto Cliente = Proyecto.Cliente;
         UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         InstalacionDto Instalacion = Proyecto.Instalacion;
         CadenaDto Cadena = Instalacion.Cadenas[0];
         InversorDto Inversor = Cadena.Inversor;
         ModuloDto Modulo = Cadena.Modulo;
         LugarPRLDto Hospital = Proyecto.LugaresPRL[0];
         LugarPRLDto CSalud = Proyecto.LugaresPRL[1];
         LugarPRLDto Mutua = Proyecto.LugaresPRL[2];
         LugarPRLDto Planta = Proyecto.LugaresPRL[3];
         double? Porcentaje = Instalacion.ConsumoEstimado / Instalacion.GeneracionAnual * 100;

         Dictionary<string, string?> MapWORD = new()
         {
            // PROYECTO
            { "#Dia", Proyecto.Fecha.Day.ToString() },
            { "#Mes", Mes },
            { "#Año", Proyecto.Fecha.Year.ToString()},
            { "#Presupuesto", Proyecto.Presupuesto.ToString()},
            { "#PresupuestoSyS", Proyecto.PresupuestoSyS.ToString()},
            { "#PlazoEjecucion", Proyecto.PlazoEjecucionStr },

            // CLIENTE
            { "#Nombre", Cliente.Nombre },
            { "#Dni", Cliente.Dni },

            // UBICACIÓN
            { "#Direccion", Ubicacion.GetDireccion() },
            { "#Cp", Ubicacion.Cp.ToString() },
            { "#Municipio", Ubicacion.Municipio },
            { "#Provincia", Ubicacion.Provincia },
            { "#RefCatastral", Ubicacion.RefCatastral },
            { "#Superficie", Ubicacion.Superficie.ToString() },
            { "#CoordXUTM", Ubicacion.CoordXUTM.ToString() },
            { "#CoordYUTM", Ubicacion.CoordYUTM.ToString() },
            { "#Latitud", Ubicacion.Latitud.ToString() },
            { "#Longitud", Ubicacion.Longitud.ToString() },
            { "#Cups", Ubicacion.Cups },

            // INSTALACIÓN
            { "#Inclinacion", Instalacion.Inclinacion.ToString() },
            { "#Azimut", Instalacion.Azimut },
            { "#Tipo", Instalacion.Tipo },
            { "#CoordXConexion", Instalacion.CoordXConexion.ToString() },
            { "#CoordYConexion", Instalacion.CoordYConexion.ToString() },
            { "#Estructura", Instalacion.Estructura },
            { "#Definicion", Instalacion.Definicion },
            { "#IAutomatico", Instalacion.IAutomatico },
            { "#IDiferencial", Instalacion.IDiferencial },
            { "#TotalNominal", Instalacion.TotalNominal.ToString() },
            { "#TotalPico", Instalacion.TotalPico.ToString() },
            { "#TotalCadenas", Instalacion.TotalCadenas.ToString() },
            { "#TotalModulos", Instalacion.TotalModulos.ToString() },
            { "#TotalInversores", Instalacion.TotalInversores.ToString() },
            { "#PotenciaContratada", Instalacion.PotenciaContratada.ToString() },
            { "#ConsumoEstimado", Instalacion.ConsumoEstimado.ToString() },
            { "#GeneracionAnual", Instalacion.GeneracionAnual.ToString() },
            { "#Porcentaje", Porcentaje.ToString() },

            { "#Cadena.NumModulos", Cadena.NumModulos.ToString() },

            // INVERSOR
            { "#Inversor.Modelo", Inversor.Modelo.TrimEnd() },
            { "#Inversor.PotenciaNominal", Inversor.PotenciaNominal.ToString() },
            { "#Inversor.VmnMPPT", Inversor.VminMPPT.ToString()  },
            { "#Inversor.VmxMPPT", Inversor.VmaxMPPT.ToString()},
            { "#Inversor.Vmax", Inversor.Vmax.ToString() },
            { "#Inversor.Vmin", Inversor.Vmin.ToString() },
            { "#Inversor.IntensidadMaxMPPT", Inversor.IntensidadMaxMPPT.ToString() },
            { "#Inversor.VO", Inversor.VO.ToString() },
            { "#Inversor.Rendimiento", Inversor.Rendimiento.ToString() },

            // MÓDULO
            { "#Modulo.Modelo", Modulo.Modelo.TrimEnd()},
            { "#Modulo.Potencia", Modulo.Potencia.ToString() },
            { "#Modulo.Fabricante", Modulo.Fabricante },
            { "#Modulo.NumCelulas", Modulo.NumCelulas.ToString() },
            { "#Modulo.Tipo", Modulo.Tipo },
            { "#Modulo.Vmp", Modulo.Vmp.ToString() },
            { "#Modulo.Imp", Modulo.Imp.ToString() },
            { "#Modulo.Isc", Modulo.Isc.ToString() },
            { "#Modulo.Vca", Modulo.Vca.ToString() },
            { "#Modulo.Eficiencia", Modulo.Eficiencia.ToString() },
            { "#Modulo.Dimensiones", Modulo.Dimensiones },
            { "#Modulo.Peso", Modulo.Peso.ToString() },
            { "#Modulo.SalidaPotencia", Modulo.SalidaPotencia.ToString() },
            { "#Modulo.TensionVacio", Modulo.TensionVacio.ToString() },
            { "#Modulo.TaTONC", Modulo.TaTONC?.TrimEnd() },
            { "#Modulo.Tolerancia", Modulo.Tolerancia.ToString() },

            // LUGARES PRL
            { "#Hospital.Nombre", Hospital.Nombre },
            { "#Hospital.Telefono", Hospital.Telefono },
            { "#Hospital.Direccion", Hospital.Direccion },

            { "#CSalud.Nombre", CSalud.Nombre },
            { "#CSalud.Telefono", CSalud.Telefono },
            { "#CSalud.Direccion", CSalud.Direccion },

            { "#Mutua.Nombre", Mutua.Nombre },
            { "#Mutua.Telefono", Mutua.Telefono },
            { "#Mutua.Direccion", Mutua.Direccion },

            { "#Planta.Nombre", Planta.Nombre },
            { "#Planta.Direccion", Planta.Direccion },
            { "#Planta.Cp", Planta.Cp.ToString() },
            { "#Planta.Municipio", Planta.Municipio },
            { "#Planta.Provincia", Planta.Provincia },
            { "#Planta.Telefono", Planta.Telefono },
            { "#Planta.NIMA", Planta.NIMA },
            { "#Planta.Autorizacion", Planta.Autorizacion },
         };
         return MapWORD;
      }

      // RELLENAR PLANTILLAS WORD CON EL DICCIONARIO CREADO PARA INTRODUCIR DATOS
      public string CreateMemories(Dictionary<string, string?> MapWORD, string[] pathsWORD)
      {
         if (MapWORD == null || MapWORD.Count == 0)
            return "ERROR => El diccionario de palabras clave está mal estructurado o vacío: " + MapWORD;

         if (pathsWORD == null || pathsWORD.Length == 0)
            return "ERROR => No se han encontrado las rutas de destino:  " + pathsWORD;

         try
         {
            foreach (string path in pathsWORD)
            {
               if (Path.GetExtension(path).Equals(".docx"))
               {
                  using DocX doc = DocX.Load(path);
                  foreach (Paragraph parrafo in doc.Paragraphs)
                  {
                     foreach (KeyValuePair<string, string?> kvp in MapWORD)
                        if (parrafo.Text.Contains(kvp.Key))
                           parrafo.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);

                  }

                  int numSections = doc.Sections.Count;
                  for (int i = 0; i < numSections; i++)
                  {
                     Headers headers = doc.Sections[i].Headers;
                     foreach (Paragraph paragraph in headers.Odd.Paragraphs)
                     {
                        foreach (KeyValuePair<string, string?> kvp in MapWORD)
                           if (paragraph.Text.Contains(kvp.Key))
                              paragraph.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);
                     }
                  }
                  doc.Save();
               }
            }
            return "OK";

         }
         catch (Exception ex) { return new("EXCEPTION => " + ex.Message); }
      }
   }
}
