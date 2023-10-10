using System.Globalization;
using System.Text.RegularExpressions;
using backend_API.Dto;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace backend_API.Service
{
   // Interfaz que funciona como servicio para otras clases mediante inyeccion de dependencias
   public interface IWORDService
   {
      public string InitServiceWORD(ProyectoDto Proyecto);
      public Dictionary<string, string?> CreateMap(ProyectoDto Proyecto);
      public string CreateMemorias(Dictionary<string, string?> MapWORD, string[] pathsWORD);
   }

   // Clase que sirve de servicio para rellenar documentos WORD de un proyecto
   public class WORDService : IWORDService
   {
      private readonly IProjectService _projectService;

      // Contructor por parámetros con inyeccion de dependencias del CRUD de Mod
      public WORDService(IProjectService projectService)
      {
         _projectService = projectService;
      }

      public string InitServiceWORD(ProyectoDto Proyecto)
      {
         var folderDownloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
         var folderEnd = Path.Combine(folderDownloads, "MemoriasWORD_" + Proyecto.Referencia);
         Directory.CreateDirectory(folderEnd);

         string[]? pathsOrigin = Directory.GetFiles("Resources/TemplatesWORD");
         folderEnd = _projectService.ClonarFiles(Proyecto, pathsOrigin, folderEnd);
         string[]? pathsWORD = Directory.GetFiles(folderEnd);

         Dictionary<string, string?> MapWORD = CreateMap(Proyecto);
         var result = CreateMemorias(MapWORD, pathsWORD);
         if (result != null) return result;
         return "No se ha generado el archivo";
      }

      // Crear las memorias justificativas de un proyecto
      public Dictionary<string, string?> CreateMap(ProyectoDto Proyecto)
      {
         var Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Proyecto.Fecha.Month);
         Mes = Mes.ToUpper().ToCharArray()[0] + Mes[1..];

         var Cliente = Proyecto.Cliente;
         var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         var Instalacion = Proyecto.Instalacion;
         var Cadena = Instalacion.Cadenas[0];
         var Inversor = Cadena.Inversor;
         var Modulo = Cadena.Modulo;
         var Hospital = Proyecto.LugaresPRL[0];
         var CSalud = Proyecto.LugaresPRL[1];
         var Mutua = Proyecto.LugaresPRL[2];
         var Planta = Proyecto.LugaresPRL[3];

         Dictionary<string, string?> MapWORD = new()
         {
            { "#Dia", Proyecto.Fecha.Day.ToString() },
            { "#Mes", Mes },
            { "#Año", Proyecto.Fecha.Year.ToString()},
            { "#Presupuesto", Proyecto.Presupuesto.ToString()},
            { "#PrespSyS", Proyecto.PresupuestoSyS.ToString()},
            { "#PlazoEjecucion", Proyecto.PlazoEjecucionStr },

            { "#Nombre", Cliente.Nombre },
            { "#Dni", Cliente.Dni },

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

            { "#Cadena.NumModulos", Cadena.NumModulos.ToString() },

            { "#Inversor.Modelo", Inversor.Modelo.TrimEnd() },
            { "#Inversor.PotenciaNominal", Inversor.PotenciaNominal.ToString() },
            { "#Inversor.VmnMPPT", Inversor.VminMPPT.ToString()  },
            { "#Inversor.VmxMPPT", Inversor.VmaxMPPT.ToString()},
            { "#Inversor.Vmax", Inversor.Vmax.ToString() },
            { "#Inversor.Vmin", Inversor.Vmin.ToString() },
            { "#Inversor.IntensidadMaxMPPT", Inversor.IntensidadMaxMPPT.ToString() },
            { "#Inversor.VO", Inversor.VO.ToString() },
            { "#Inversor.Rendimiento", Inversor.Rendimiento.ToString() },

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

      public string CreateMemorias(Dictionary<string, string?> MapWORD, string[] pathsWORD)
      {
         if (MapWORD == null) return "ERROR => El diccionario está vacío o mal estructurado: " + MapWORD;
         if (pathsWORD == null) return "ERROR => NO se han creado las rutas de destino:  " + pathsWORD;

         foreach (string ruta in pathsWORD)
         {
            if (Path.GetExtension(ruta).Equals(".docx"))
            {
               using (DocX doc = DocX.Load(ruta))
               {
                  foreach (Paragraph parrafo in doc.Paragraphs)
                     foreach (var kvp in MapWORD)
                        if (parrafo.Text.Contains(kvp.Key))
                           parrafo.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);

                  int numSections = doc.Sections.Count;
                  for (int i = 0; i < numSections; i++)
                  {
                     Headers headers = doc.Sections[i].Headers;
                     foreach (Paragraph paragraph in headers.Odd.Paragraphs)
                        foreach (var kvp in MapWORD)
                           if (paragraph.Text.Contains(kvp.Key))
                              paragraph.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);
                  }
                  doc.Save();
               }
            }
         }

         string? folderEnd = Path.GetPathRoot(pathsWORD[0]);
         return "OK => Las memorias en formato WORD se han generado con éxito: " + folderEnd;
      }
   }
}
