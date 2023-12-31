﻿using Automatizaciones_API.Models.Dto;
using ClosedXML.Excel;
using System.Globalization;
using System.Reflection;


namespace Automatizaciones_API.Service
{
   /// Interfaz que da servicio a otras clases para generar un archivo EXCEL mediante inyección de dependencias
   public interface IEXCELService
   {
      /// Genera un archivo EXCEL con los datos de un proyecto    
      public (byte[]?, string?) CreateEXCEL(ProyectoDto Proyecto);
   }

   /// Clase que implementa IEXCELService para manejo y relleno de un archivo EXCEL
   public class EXCELService(IProjectService projectServices) : IEXCELService
   {
      /// Genera un archivo EXCEL con los datos de un proyecto    
      public (byte[]?, string?) CreateEXCEL(ProyectoDto? Proyecto)
      {
         if (Proyecto == null) return (null, "ERROR => El proyecto no es válido");

         string pathOrigin = "Utilities/Resources/REFERENCIAS_MEMORIA.xlsx";

         using (XLWorkbook workbook = new(pathOrigin))
         {
            IXLWorksheet worksheet = workbook.Worksheet(1);
            InstalacionDto Instalacion = Proyecto.Instalacion;
            ClienteDto Cliente = Proyecto.Cliente;
            UbicacionDto Ubicacion = Cliente.Ubicaciones[0];
            double[] latlng = projectServices.GetUTM(Instalacion);
            string Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Proyecto.Fecha.Month).ToUpper();
            IList<CadenaDto> Cadenas = Instalacion.Cadenas;
            int row = 7;
            int column = 2;
            int startRow = row + 1;

            // Proyecto
            worksheet.Cell(row, column++).Value = Mes;
            worksheet.Cell(row, column++).Value = Proyecto.Fecha.Year;

            // Cliente
            worksheet.Cell(row, column++).Value = Cliente.Nombre.ToUpper();
            worksheet.Cell(row, column++).Value = Cliente.Dni.ToUpper();

            // Ubicación
            worksheet.Cell(row, column++).Value = Ubicacion.GetDireccion();
            worksheet.Cell(row, column++).Value = Ubicacion.Cp;
            worksheet.Cell(row, column++).Value = Ubicacion.Municipio;
            worksheet.Cell(row, column++).Value = Ubicacion.Provincia;
            worksheet.Cell(row, column++).Value = Ubicacion.RefCatastral.ToUpper();
            worksheet.Cell(row, column++).Value = Ubicacion.Superficie;

            worksheet.Cell(row, column++).Value = latlng[0];
            worksheet.Cell(row, column++).Value = latlng[1];
            worksheet.Cell(row, column++).Value = Ubicacion.Latitud;
            worksheet.Cell(row, column++).Value = Ubicacion.Longitud;

            // Instalación
            worksheet.Cell(row, column++).Value = Instalacion.Inclinacion;
            worksheet.Cell(row, column++).Value = Instalacion.Azimut;
            worksheet.Cell(row, column++).Value = Instalacion.TotalPico;
            worksheet.Cell(row, column++).Value = Instalacion.TotalNominal;
            worksheet.Cell(row, column++).Value = Instalacion.Tipo;
            worksheet.Cell(row, column++).Value = Instalacion.CoordXConexion;
            worksheet.Cell(row, column++).Value = Instalacion.CoordYConexion;

            // Cadenas
            int index = 0;
            foreach (CadenaDto c in Cadenas)
            {
               ModuloDto Modulo = c.Modulo;
               InversorDto Inversor = c.Inversor;

               // Módulo
               PropertyInfo[] propiedades = Modulo.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

               if (index == 1) row = 22;
               if (index == 2) row = 36;

               worksheet.Cell(row, column++).Value = Modulo.Fabricante;
               worksheet.Cell(row, column).Value = Modulo.Modelo.TrimEnd();

               foreach (PropertyInfo propiedad in propiedades)
               {
                  string nombre = propiedad.Name;

                  if (nombre.Equals("IdModulo") || nombre.Equals("Modelo") || nombre.Equals("Fabricante")) continue;

                  object? valor = propiedad.GetValue(Modulo);

                  if (valor is int i) worksheet.Cell(startRow, column).Value = i;
                  else if (valor is double d) worksheet.Cell(startRow, column).Value = d;
                  else if (valor is string s) worksheet.Cell(startRow, column).Value = s;

                  startRow++;
               }
               column = 28;
               worksheet.Cell(row, 26).Value = c.NumCadenas;
               worksheet.Cell(row, column++).Value = Modulo.Potencia;
               worksheet.Cell(row, column++).Value = Inversor.Fabricante;
               worksheet.Cell(row, column).Value = Inversor.Modelo.TrimEnd();

               // Inversor
               propiedades = Inversor.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
               startRow = row + 1;

               foreach (PropertyInfo propiedad in propiedades)
               {
                  string nombre = propiedad.Name;
                  object? valor = propiedad.GetValue(Inversor);

                  if (nombre.Equals("IdInversor", StringComparison.Ordinal) || nombre.Equals("Modelo") || nombre.Equals("Fabricante", StringComparison.Ordinal)) continue;
                  else if (valor is int i) worksheet.Cell(startRow, column).Value = i;
                  else if (valor is double d) worksheet.Cell(startRow, column).Value = d;
                  else if (valor is string s) worksheet.Cell(startRow, column).Value = s;

                  startRow++;
               }
               worksheet.Cell(row, 32).Value = Inversor.IO;
               worksheet.Cell(row, 42).Value = c.NumInversores;
               worksheet.Cell(row, 43).Value = c.Inversor.PotenciaNominal;

               index++;
            }

            column = 33;
            worksheet.Cell(row, 25).Value = Instalacion.TotalModulos;
            worksheet.Cell(row, 27).Value = Instalacion.TotalCadenas;
            worksheet.Cell(row, 31).Value = Instalacion.Vatimetro;
            worksheet.Cell(row, column++).Value = Instalacion.Fusible;
            worksheet.Cell(row, column++).Value = Instalacion.IAutomatico;
            worksheet.Cell(row, column++).Value = Instalacion.IDiferencial;
            worksheet.Cell(row, column++).Value = Instalacion.Estructura;
            worksheet.Cell(row, column++).Value = Instalacion.Definicion;

            // Cubiertas
            foreach (CubiertaDto c in Instalacion.Cubiertas)
            {
               worksheet.Cell(row, 38).Value = c.MedidasColectivas;
               worksheet.Cell(row, 39).Value = c.Accesibilidad;
               worksheet.Cell(row, 41).Value = c.Material;
               row++;
            }

            row = 7;
            column = 44;
            worksheet.Cell(row, 40).Value = Instalacion.Inclinacion;
            worksheet.Cell(row, column++).Value = Instalacion.Definicion;
            worksheet.Cell(row, column).Value = Proyecto?.Presupuesto;
            worksheet.Cell(10, column++).Value = Proyecto?.PlazoEjecucion.Date.ToShortDateString();
            worksheet.Cell(row, column++).Value = Proyecto?.PresupuestoSyS;

            // Lugares PRL
            List<LugarPRLDto>? list = Proyecto?.LugaresPRL;
            index = 0;
            for (int i = 0; i < list?.Count; i++)
            {
               LugarPRLDto? l = list[i];
               column = 47;
               if (index == 3)
               {
                  row = 7;
                  column = 53;
                  worksheet.Cell(row, 59).Value = l.NIMA;
                  worksheet.Cell(row, 60).Value = l.Autorizacion;
               }

               l.Direccion = l.Calle + ", " + l.Numero;
               worksheet.Cell(row, column++).Value = l.Nombre;
               worksheet.Cell(row, column++).Value = l.Direccion;
               worksheet.Cell(row, column++).Value = l.Cp;
               worksheet.Cell(row, column++).Value = l.Municipio;
               worksheet.Cell(row, column++).Value = l.Provincia;
               worksheet.Cell(row, column++).Value = l.Telefono;

               row++;
               index++;
            }

            try
            {
               if (Proyecto == null) return (null, "Proyecto erróneo o mal estructurado");

               using (var excelStream = new MemoryStream())
               {
                  workbook.SaveAs(excelStream);
                  excelStream.Position = 0;
                  return (excelStream.ToArray(), string.Empty);
               }
            }
            catch (Exception ex) { return (null, "EXCEPTION => " + ex.Message + " HELP: " + ex.StackTrace); }
         }
      }
   }
}