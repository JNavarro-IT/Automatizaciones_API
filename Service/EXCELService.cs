using System.Globalization;
using backend_API.Dto;
using backend_API.Models;
using backend_API.Repository;
using ClosedXML.Excel;

namespace backend_API.Service
{
   public interface IExcelServices
   {
      public string CreateEXCEL(ProyectoDto Proyecto);
   }

   public class EXCELService : IExcelServices
   {
      private readonly IInstalacionService _instalacionServices;
      private readonly IBaseRepository<Modulo, ModuloDto> _moduloRepository;

      public EXCELService(IInstalacionService instalacionServices, IBaseRepository<Modulo, ModuloDto> moduloRepository)
      {
         _instalacionServices = instalacionServices;
         _moduloRepository = moduloRepository;
      }

      public string CreateEXCEL(ProyectoDto Proyecto)
      {
         var rutaArchivo = "Resources/REFERENCIAS_MEMORIA.xlsx";
         var newRuta = "";
         using (var workbook = new XLWorkbook(rutaArchivo))
         {
            var worksheet = workbook.Worksheet(1);
            int columnaActual = 4;
            InstalacionDto Instalacion = Proyecto.Instalacion;
            ClienteDto Cliente = Proyecto.Cliente;
            List<UbicacionDto> Ubicaciones = Cliente.Ubicaciones;
            UbicacionDto Ubicacion = Cliente.Ubicaciones[0];
            var latlng = _instalacionServices.GetUTM(Instalacion);

            //PROYECTO
            worksheet.Cell(7, 2).Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Proyecto.Fecha.Month).ToUpper();
            worksheet.Cell(7, 3).Value = Proyecto.Fecha.Year;

            //Cliente
            worksheet.Cell(7, 4).Value = Cliente.Nombre.ToUpper();
            worksheet.Cell(7, 5).Value = Cliente.Dni.ToUpper();

            //Ubicación
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

            worksheet.Cell(7, 6).Value = Direccion;
            worksheet.Cell(7, 7).Value = Ubicacion.Cp;
            worksheet.Cell(7, 8).Value = Ubicacion.Municipio;
            worksheet.Cell(7, 9).Value = Ubicacion.Provincia;
            worksheet.Cell(7, 10).Value = Ubicacion.RefCatastral;
            worksheet.Cell(7, 11).Value = Ubicacion.Superficie;

            Ubicacion.CoordXUTM = latlng[0];
            Ubicacion.CoordYUTM = latlng[1];
            worksheet.Cell(7, 12).Value = Ubicacion.CoordXUTM;
            worksheet.Cell(7, 13).Value = Ubicacion.CoordYUTM;
            worksheet.Cell(7, 14).Value = Ubicacion.Latitud;
            worksheet.Cell(7, 15).Value = Ubicacion.Longitud;

            //Instalación
            worksheet.Cell(7, 16).Value = Instalacion.Inclinacion;
            worksheet.Cell(7, 17).Value = Instalacion.Azimut;
            worksheet.Cell(7, 18).Value = Instalacion.TotalPico;
            worksheet.Cell(7, 19).Value = Instalacion.TotalNominal;
            worksheet.Cell(7, 20).Value = Instalacion.Tipo;

            if (Instalacion.CoordXConexion == 0 || Instalacion.CoordYConexion == 0)
            {
               worksheet.Cell(7, 21).Value = Ubicacion.Latitud;
               worksheet.Cell(7, 22).Value = Ubicacion.Longitud;
            }
            else
            {
               worksheet.Cell(7, 21).Value = Instalacion.CoordXConexion;
               worksheet.Cell(7, 22).Value = Instalacion.CoordYConexion;
            }

            //CADENAS
            var index = 0;
            var row = 7;
            var Cadenas = Instalacion.Cadenas;
            int rowModulo = row + 1;
            foreach (CadenaDto c in Cadenas)
            {
               if (index == 1)
                  row = 22;
               if (index == 2)
                  row = 36;
               
               Modulo? entity = _moduloRepository.GetEntity(c.Modulo.IdModulo).Result;

               worksheet.Cell(row, 23).Value = c.Modulo.Fabricante;
               worksheet.Cell(row, 24).Value = c.Modulo.Modelo.TrimEnd();
               worksheet.Cell(row + 1, 24).Value = c.Modulo.Potencia;
               worksheet.Cell(row + 2, 24).Value = c.Modulo.Vmp;
               worksheet.Cell(row + 3, 24).Value = c.Modulo.Imp;
               worksheet.Cell(row + 4, 24).Value = c.Modulo.Isc;
               worksheet.Cell(row + 5, 24).Value = c.Modulo.Vca;
               worksheet.Cell(row + 6, 24).Value = c.Modulo.Eficiencia;
               worksheet.Cell(row + 7, 24).Value = entity.Dimensiones;
               worksheet.Cell(row + 8, 24).Value = entity.Peso;
               worksheet.Cell(row + 9, 24).Value = c.Modulo.NumCelulas;
               worksheet.Cell(row + 10, 24).Value = c.Modulo.Tipo;
               worksheet.Cell(row + 11, 24).Value = c.Modulo.TaTONC;
               worksheet.Cell(row + 12, 24).Value = c.Modulo.SalidaPotencia;
               worksheet.Cell(row + 13, 24).Value = c.Modulo.TensionVacio;
               worksheet.Cell(row + 14, 24).Value = c.Modulo.Tolerancia;

               worksheet.Cell(row, 26).Value = c.NumCadenas;
               worksheet.Cell(row, 28).Value = c.Modulo.Potencia;
               worksheet.Cell(row, 29).Value = c.Inversor.Fabricante;
               worksheet.Cell(row, 30).Value = c.Inversor.Modelo.TrimEnd();
               worksheet.Cell(row + 1, 30).Value = c.Inversor.PotenciaNominal;
               worksheet.Cell(row + 2, 30).Value = c.Inversor.VO;
               worksheet.Cell(row + 3, 30).Value = c.Inversor.IO;
               worksheet.Cell(row + 4, 30).Value = c.Inversor.Vmin;
               worksheet.Cell(row + 5, 30).Value = c.Inversor.Vmax;
               worksheet.Cell(row + 6, 30).Value = c.Inversor.CorrienteMaxString;
               worksheet.Cell(row + 7, 30).Value = c.Inversor.VminMPPT;
               worksheet.Cell(row + 8, 30).Value = c.Inversor.VmaxMPPT;
               worksheet.Cell(row + 9, 30).Value = c.Inversor.IntensidadMaxMPPT;
               worksheet.Cell(row + 10, 30).Value = c.Inversor.Rendimiento;

               worksheet.Cell(row, 32).Value = c.Inversor.IO;
               worksheet.Cell(row, 42).Value = c.NumInversores;
               worksheet.Cell(row, 43).Value = c.Inversor.PotenciaNominal;

               rowModulo++;
               index++;
            }

            worksheet.Cell(7, 25).Value = Instalacion.TotalModulos;
            worksheet.Cell(7, 27).Value = Instalacion.TotalCadenas;
            worksheet.Cell(7, 31).Value = Instalacion.Vatimetro;
            worksheet.Cell(7, 33).Value = Instalacion.Fusible;
            worksheet.Cell(7, 34).Value = Instalacion.IAutomatico;
            worksheet.Cell(7, 35).Value = Instalacion.IDiferencial;
            worksheet.Cell(7, 36).Value = Instalacion.Estructura;
            worksheet.Cell(7, 37).Value = Instalacion.Definicion;

            foreach (CubiertaDto c in Instalacion.Cubiertas)
            {
               worksheet.Cell(row, 38).Value = c.MedidasColectivas;
               worksheet.Cell(row, 39).Value = c.Accesibilidad;
               worksheet.Cell(row, 41).Value = c.Material;
               row++;
            }
            worksheet.Cell(7, 40).Value = Instalacion.Inclinacion;
            worksheet.Cell(7, 44).Value = Instalacion.Definicion;
            worksheet.Cell(7, 45).Value = Proyecto.Presupuesto;
            worksheet.Cell(7, 46).Value = Proyecto.PresupuestoSyS;
            worksheet.Cell(10, 45).Value = Proyecto.PlazoEjecucion.Date.ToShortDateString();

            row = 7;
            index = 0;
            foreach (LugarPRLDto l in Proyecto.LugaresPRL)
            {
               var column = 47;
               if (index == 3)
               {
                  column = 53; //+6
                  row = 7;
               }

               worksheet.Cell(row, column++).Value = l.Nombre;
               l.Direccion = l.Calle + ", " + l.Numero;
               worksheet.Cell(row, column++).Value = l.Direccion;
               worksheet.Cell(row, column++).Value = l.Cp;
               worksheet.Cell(row, column++).Value = l.Municipio;
               worksheet.Cell(row, column++).Value = l.Provincia;
               worksheet.Cell(row, column++).Value = l.Telefono;

               if (index == 3)
               {
                  worksheet.Cell(row, column++).Value = l.NIMA;
                  worksheet.Cell(row, column++).Value = l.Autorizacion;
               }
               row++;
               index++;
            }
            var downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            var fileName = "EXCEL_Memorias_" + Cliente.Nombre + ".xlsx";
            newRuta = Path.Combine(downloads, fileName);
            workbook.SaveAs(newRuta);
         }
         return newRuta;
      }
   }
}