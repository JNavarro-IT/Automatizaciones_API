using System.Globalization;
using System.Text.RegularExpressions;
using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace backend_API.Service
{
   public interface IWORDService
   {
      public Task<string[]> CreateMemorias(ProyectoDto Proyecto);
      public string ClonarWORD(ProyectoDto Proyecto);
   }
   public class WORDService : IWORDService
   {

      private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;

      public WORDService(IBaseRepository<Modulo, ModuloDto> moduloRepository) 
      { 
         _moduloRepository = moduloRepository;
      }

      public async Task<string[]> CreateMemorias(ProyectoDto Proyecto)
      {
         string? rutaDestino = ClonarWORD(Proyecto);
         string[]? rutasWORD = Directory.GetFiles(rutaDestino);
         var Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Proyecto.Fecha.Month);
         ClienteDto Cliente = Proyecto.Cliente;
         UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         InstalacionDto Instalacion = Proyecto.Instalacion;
         CadenaDto Cadena = Instalacion.Cadenas[0];
         ModuloDto Modulo = Cadena.Modulo;
         Modulo? Entity = await _moduloRepository.GetEntity(Modulo.IdModulo);
         InversorDto Inversor = Cadena.Inversor;

         var Direccion = Ubicacion.Calle + ", " + Ubicacion.Numero;
         if (!Ubicacion.Bloque.Equals(""))
            Direccion += ", " + Ubicacion.Bloque;
         if (!Ubicacion.Portal.Equals(""))
            Direccion += ", " + Ubicacion.Portal;
         if (!Ubicacion.Escalera.Equals(""))
            Direccion += ", " + Ubicacion.Escalera;
         if (!Ubicacion.Piso.Equals(""))
            Direccion += ", " + Ubicacion.Piso;
         if (!Ubicacion.Puerta.Equals(""))
            Direccion += ", " + Ubicacion.Puerta;

         LugarPRLDto Hospital = Proyecto.LugaresPRL[0];
         LugarPRLDto CSalud = Proyecto.LugaresPRL[1];
         LugarPRLDto Mutua = Proyecto.LugaresPRL[2];
         LugarPRLDto Planta = Proyecto.LugaresPRL[3];
         Hospital.Direccion = Hospital.Calle + ", " + Hospital.Numero;
         CSalud.Direccion = CSalud.Calle + ", " + CSalud.Numero;
         Mutua.Direccion = Mutua.Calle + ", " + Mutua.Numero;
         Planta.Direccion = Planta.Calle + ", " + Planta.Numero;

         Dictionary<string, string?> reemplazos = new()
         {
            { "#Mes", Mes },
            { "#Año", Proyecto.Fecha.Year.ToString()},
            { "#Presupuesto", Proyecto.Presupuesto.ToString()},
            { "#PresupuestoSyS", Proyecto.PresupuestoSyS.ToString()},
            { "#PlazoEjecucion", Proyecto.PlazoEjecucionStr },

            { "#Nombre", Cliente.Nombre },
            { "#Dni", Cliente.Dni },

            { "#Direccion", Direccion },
            { "#Cp", Ubicacion.Cp.ToString() },
            { "#Municipio", Ubicacion.Municipio },
            { "#Provincia", Ubicacion.Provincia },
            { "#RefCatastral", Ubicacion.RefCatastral },
            { "#Superficie", Ubicacion.Superficie.ToString() },
            { "#CoordXUTM", Ubicacion.CoordXUTM.ToString() },
            { "#CoordYUTM", Ubicacion.CoordYUTM.ToString() },

            { "#Tipo", Instalacion.Tipo },
            { "#CoordXConexion", Instalacion.CoordXConexion.ToString() },
            { "#CoordYConexion", Instalacion.CoordYConexion.ToString() },
            { "#Inclinacion", Instalacion.Inclinacion.ToString() },
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

            { "#Inversor.Modelo", Inversor.Modelo },
            { "#Inversor.PotenciaNominal", Inversor.PotenciaNominal.ToString() },
            { "#Inversor.VmnMPPT", Inversor.VminMPPT.ToString()  },
            { "#Inversor.VmxMPPT", Inversor.VmaxMPPT.ToString()},
            { "#Inversor.Vmax", Inversor.Vmax.ToString() },
            { "#Inversor.Vmin", Inversor.Vmin.ToString() },
            { "#Inversor.IntensidadMaxMPPT", Inversor.IntensidadMaxMPPT.ToString() },
            { "#Inversor.VO", Inversor.VO.ToString() },
            { "#Inversor.Rendimiento", Inversor.Rendimiento.ToString() },

            { "#Modulo.Modelo", Modulo.Modelo },
            { "#Modulo.Potencia", Modulo.Potencia.ToString() },
            { "#Modulo.Fabricante", Modulo.Fabricante },
            { "#Modulo.NumCelulas", Modulo.NumCelulas.ToString() },
            { "#Modulo.Tipo", Modulo.Tipo },
            { "#Modulo.Vmp", Modulo.Vmp.ToString() },
            { "#Modulo.Imp", Modulo.Imp.ToString() },
            { "#Modulo.Isc", Modulo.Isc.ToString() },
            { "#Modulo.Vca", Modulo.Vca.ToString() },
            { "#Modulo.Eficiencia", Modulo.Eficiencia.ToString() },
            { "#Modulo.Dimensiones", Entity.Dimensiones },
            { "#Modulo.Peso", Entity.Peso.ToString() },
            { "#Modulo.SalidaPotencia", Modulo.SalidaPotencia.ToString() },
            { "#Modulo.TensionVacio", Modulo.TensionVacio.ToString() },
            { "#Modulo.TaTONC", Modulo.TaTONC },
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
         var index = 1;
         foreach (string ruta in rutasWORD)
         {
            using (DocX doc = DocX.Load(ruta))
            {
               foreach (Paragraph parrafo in doc.Paragraphs)
               {
                  foreach (var kvp in reemplazos)
                  {
                     if (parrafo.Text.Contains(kvp.Key))
                     {
                        parrafo.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);
                        Console.Error.WriteLine(kvp.Key);
                     }
                  }
               }
            
               Console.Error.WriteLine("------------------------------");

               int numSections = doc.Sections.Count;
               for (int i = 0; i < numSections; i++)
               {
                  Headers headers = doc.Sections[i].Headers;
                  foreach (Paragraph paragraph in headers.Odd.Paragraphs)
                  {
                     foreach (var kvp in reemplazos)
                     {
                        if (paragraph.Text.Contains(kvp.Key))
                        {
                           Console.WriteLine(kvp.Key);
                           paragraph.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);
                        }
                     }
                  }
               }

               Console.Error.WriteLine(index);
               index++;
               doc.Save();
            } 
         }
         return rutasWORD;
      }

      public string ClonarWORD(ProyectoDto Proyecto)
      {
         try
         {
            string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string folder = "MemoriasWORD_" + Proyecto.Cliente.Nombre;
            string carpetaDestino = Path.Combine(downloads, folder);
            Directory.CreateDirectory(carpetaDestino);
            string carpetaOriginal = "Resources/templatesWORD";
            string[]? rutasOriginal = Directory.GetFiles(carpetaOriginal);

            foreach (string ruta in rutasOriginal)
            {
               string nombreFile = Path.GetFileName(ruta);
               nombreFile = nombreFile.Replace("NOMBRE PROYECTO", Proyecto.Cliente.Nombre);
               string rutaDestino = Path.Combine(carpetaDestino, nombreFile);
               File.Copy(ruta, rutaDestino, true);
            }
            return carpetaDestino;
         } 
         catch (Exception error) 
         { 
            return  "No se han generado los archivos WORD. ERROR: " + error.Message;
         }
      }
   }
}
