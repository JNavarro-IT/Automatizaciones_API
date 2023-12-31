﻿using Automatizaciones_API.Models.Dto;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using System.Globalization;

namespace Automatizaciones_API.Service
{
   // INTERFAZ QUE DA SERVICIO A OTRAS CLASES PARA USAR ARCHIVOS PDF MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IPDFService
   {
      public string InitFillPDF(ProyectoDto Proyecto);
      public string FillPDFs(ProyectoDto Proyecto, string[] pathsOrigin, string[] pathsEnd);
      public void FillUbicacion(UbicacionDto Ubicacion, IDictionary<string, PdfFormField> fieldsMap);
      public void FillInstalacion(InstalacionDto Instalacion, IDictionary<string, PdfFormField> fieldsMap);
      public void FillDate(ProyectoDto Proyecto, IDictionary<string, PdfFormField> fieldsMap);
   }

   // CLASE QUE IMPLEMENTA IPDFService PARA MANEJO Y RELLENO DE DOCUMENTOS PDF
   public class PDFService(IProjectService projectService, IWORDService wordService) : IPDFService
   {
      private IDictionary<string, PdfFormField>? fieldsMap;
      private readonly string pathBase = "Utilities/Resources/Subvenciones/";
      private double? PotenciaGeneracion = 0;

      // INICIALIZAR EL PROCESO PARA MANEJAR ARCHIVOS PDF, SE ELIGEN EN FUNCIÓN DE LA CCAA Y SE RELLENAN MEDIANTE UN DICCIONARIO DE CLAVE-VALOR (Los archivos Pdf deben está customizados para el mapa)
      public string InitFillPDF(ProyectoDto Proyecto)
      {
         var tempPath = Directory.CreateDirectory("Utilities/Temp").FullName;

         try
         {
            UbicacionDto? Ubicacion = Proyecto.Cliente.Ubicaciones[0];
            InstalacionDto Instalacion = Proyecto.Instalacion;

            // DETERMINAR CCAA PARA ELEGIR ARCHIVOS
            if (Ubicacion.CCAA is null || Ubicacion.CCAA is "")
               return "ERROR => Se requiere de una CCAA para obtener los documentos correspondientes";

            string folderCCAA = pathBase + Ubicacion.CCAA;
            List<string> pathsOrigin = [.. Directory.GetFiles(folderCCAA)];

            if (Ubicacion.CCAA.Equals("Andalucia"))
            {
               var Porcentaje = Instalacion.ConsumoEstimado / Instalacion.GeneracionAnual * 100;
               var Diferencia = Instalacion.GeneracionAnual - Instalacion.ConsumoEstimado;

               if (Porcentaje >= 78) pathsOrigin.Remove(folderCCAA + "/2AND_NAME.docx");
               else pathsOrigin.Remove(folderCCAA + "/1AND_NAME.docx");

               bool eco3 = projectService.CheckMunicipio(Ubicacion);

               if (eco3) pathsOrigin = [folderCCAA + "/AND1_NAME.pdf"];
               else pathsOrigin.Remove(folderCCAA + "/AND1_NAME.pdf");
            }

            // GENERAR ARCHIVOS TEMPORALES Y RELLENARLOS
            var resultClon = projectService.ClonarFiles(Proyecto, [.. pathsOrigin], tempPath);

            if (resultClon.StartsWith("ERROR") || resultClon.StartsWith("EXCEPTION)")) return resultClon;
            string[] pathsEnd = Directory.GetFiles(tempPath);
            var resultFill = FillPDFs(Proyecto, [.. pathsOrigin], pathsEnd);

            if (resultFill.StartsWith("ERROR") || resultFill.StartsWith("EXCEPTION)")) return resultFill;
            else return tempPath;

         }
         catch (Exception error) { return "EXCEPTION => " + error.Message; }
      }

      // RELLENAR UN ARCHIVO PDF CUSTOMIZADO MEDIANTE UN DICCIONARIO CLAVE-VALOR SOBRE UN PROYECTO
      public string FillPDFs(ProyectoDto Proyecto, string[] pathsOrigin, string[] pathsEnd)
      {
         if (pathsOrigin.Length == 0) return "ERROR => No hay archivos en esta CCAA";

         ClienteDto Cliente = Proyecto.Cliente;
         UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         InstalacionDto Instalacion = Proyecto.Instalacion;
         string[] Referencias = Proyecto.Referencia.Split("-");
         string[] filesWord = [];
         int index = 0;

         foreach (string path in pathsOrigin)
         {
            if (Path.GetExtension(path).Equals(".pdf"))
            {
               using PdfDocument pdfDoc = new(new PdfReader(path), new PdfWriter(pathsEnd[index]));
               PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(pdfDoc, true);
               fieldsMap = acroForm.GetAllFormFields();

               if (fieldsMap.ContainsKey("Referencia0"))
               {
                  for (int i = 0; i < Referencias.Length; i++)
                     ((PdfTextFormField)acroForm.GetField("Referencia" + i)).SetValue(Referencias[i]);

                  bool check = IsMonofasico(Instalacion.Vatimetro);
                  if (check) ((PdfButtonFormField)acroForm.GetField("Monofasico")).SetRadio(true);
                  else ((PdfButtonFormField)acroForm.GetField("Trifasico")).SetRadio(true);
               }

               FillCliente(Cliente, fieldsMap);
               FillUbicacion(Ubicacion, fieldsMap);
               FillInstalacion(Instalacion, fieldsMap);
               FillDate(Proyecto, fieldsMap);
               pdfDoc.Close();
            }
            else if (Path.GetExtension(path).Equals(".docx"))
               filesWord = [pathsEnd[index]];

            index++;
         }

         if (filesWord.Length > 0)
         {
            Dictionary<string, string?> MapWORD = wordService.CreateMap(Proyecto);
            string result = wordService.CreateMemories(MapWORD, filesWord);
         }

         return "OK";
      }

      // RELLENAR EL FORMULARIO CON DATOS GENERALES DEL PROYECTO
      public void FillDate(ProyectoDto Proyecto, IDictionary<string, PdfFormField> fieldsMap)
      {
         string Day = DateTime.Now.Day.ToString();
         string Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToUpper();
         string Year = DateTime.Now.Year.ToString();
         if (fieldsMap.TryGetValue("Dia", out PdfFormField? value))
         {
            ((PdfTextFormField)value).SetValue(Day);
            ((PdfTextFormField)fieldsMap["Mes"]).SetValue(Month);
            ((PdfTextFormField)fieldsMap["Año"]).SetValue(Year);
         }

         if (fieldsMap.TryGetValue("cbDia", out PdfFormField? value1))
         {
            ((PdfChoiceFormField)value1).SetValue(Day);
            ((PdfChoiceFormField)fieldsMap["cbMes"]).SetValue(Month);
         }

         if (fieldsMap.TryGetValue("cbAño", out PdfFormField? value2))
            ((PdfChoiceFormField)value2).SetValue(Year);

         if (fieldsMap.TryGetValue("FechaActual", out PdfFormField? value3))
            ((PdfChoiceFormField)value3).SetValue(Day + "/" + Month + "/" + Year);

         if (fieldsMap.TryGetValue("Fecha", out PdfFormField? value4))
         {
            ((PdfChoiceFormField)value4).SetValue(Proyecto.FechaStr);
            ((PdfChoiceFormField)fieldsMap["Fecha1"]).SetValue(Proyecto.FechaStr);
            ((PdfChoiceFormField)fieldsMap["PlazoEjecucion"]).SetValue(Proyecto.PlazoEjecucionStr);
         }
      }

      // RELLENAR EL FORMULARIO CON LOS DATOS DE UN CLIENTE
      public void FillCliente(ClienteDto? Cliente, IDictionary<string, PdfFormField> fieldsMap)
      {
         if (Cliente != null)
         {
            var words = Cliente.Nombre.Split(" ");

            foreach (string fieldName in fieldsMap.Keys)
            {
               System.Reflection.PropertyInfo? propertyInfo = Cliente.GetType().GetProperty(fieldName);
               if (propertyInfo != null)
               {
                  object? value1 = propertyInfo.GetValue(Cliente);
                  if (value1 != null)
                     ((PdfTextFormField)fieldsMap[fieldName]).SetValue(value1.ToString());
               }

               if (fieldsMap.TryGetValue("Nombre0", out PdfFormField? value))
               {
                  try
                  {
                     if (words.Length == 3)
                     {
                        ((PdfTextFormField)value).SetValue(words[0]);
                        ((PdfTextFormField)fieldsMap["ApellidoA"]).SetValue(words[1]);
                        ((PdfTextFormField)fieldsMap["ApellidoB"]).SetValue(words[2]);
                     }

                     else if (words.Length > 3)
                     {
                        ((PdfTextFormField)value).SetValue(words[0] + " " + words[1]);
                        ((PdfTextFormField)fieldsMap["ApellidoA"]).SetValue(words[2]);
                        ((PdfTextFormField)fieldsMap["ApellidoB"]).SetValue(words[3]);
                     }
                     throw new("WARNING => Nombre compuesto, puede contener errores, verifique el PDF");

                  }
                  catch (Exception e) { throw new("EXCEPTION => " + e.Message); }
               }

               if (fieldsMap.TryGetValue("Nombre1", out PdfFormField? value2))
                  ((PdfTextFormField)value2).SetValue(Cliente.Nombre);

               if (fieldsMap.TryGetValue("ApNombre", out PdfFormField? value3))
               {
                  var ApNombre = words.Reverse().ToArray();
                  ((PdfTextFormField)value3).SetValue(ApNombre.ToString());
               }

               if (fieldsMap.TryGetValue("Dni1", out PdfFormField? value4))
                  ((PdfTextFormField)value4).SetValue(Cliente.Dni);
            }
         }
      }

      // RELLENAR LOS DATOS DE LA UBICACION 
      public void FillUbicacion(UbicacionDto Ubicacion, IDictionary<string, PdfFormField> fieldsMap)
      {
         if (Ubicacion != null)
         {
            char[] CpArray = Ubicacion.Cp.ToString().ToCharArray();

            foreach (string fieldName in fieldsMap.Keys)
            {
               if (fieldName != null)
               {
                  System.Reflection.PropertyInfo? propertyInfo = Ubicacion.GetType().GetProperty(fieldName);

                  if (propertyInfo != null)
                  {
                     object? value4 = propertyInfo.GetValue(Ubicacion);
                     if (value4 != null && fieldName != null)
                        ((PdfTextFormField)fieldsMap[fieldName]).SetValue(value4.ToString());
                  }
               }
            }

            if (fieldsMap.TryGetValue("Direccion1", out PdfFormField? value))
               ((PdfTextFormField)value).SetValue(Ubicacion.GetDireccion());

            if (fieldsMap.TryGetValue("CpB", out PdfFormField? value1))
               ((PdfTextFormField)value1).SetValue(Ubicacion.Cp.ToString());

            for (int i = 0; i < CpArray.Length; i++)
            {
               if (fieldsMap.ContainsKey("Cp" + i) || fieldsMap.ContainsKey("CpBis" + i))
               {
                  ((PdfTextFormField)fieldsMap["Cp" + i]).SetValue(CpArray[i].ToString());
                  ((PdfTextFormField)fieldsMap["CpBis" + i]).SetValue(CpArray[i].ToString());
               }

               if (i > 0 && i < 4)
               {
                  if (fieldsMap.ContainsKey("Municipio" + i))
                     ((PdfTextFormField)fieldsMap["Municipio" + i]).SetValue(Ubicacion.Municipio.ToUpper());

                  if (fieldsMap.ContainsKey("Provincia" + i))
                     ((PdfTextFormField)fieldsMap["Provincia" + i]).SetValue(Ubicacion.Provincia.ToUpper());
               }
            }

            if (fieldsMap.TryGetValue("cbProvincia", out PdfFormField? value3))
               ((PdfChoiceFormField)value3).SetValue(Ubicacion.Provincia.ToUpper());
         }
      }

      // RELLENAR LOS DATOS DE LA INSTALACIÓN
      public void FillInstalacion(InstalacionDto Instalacion, IDictionary<string, PdfFormField> fieldsMap)
      {
         string? sf = Instalacion.SeccionFase.ToString();
         string seccionFase = sf + "/" + sf + "/" + sf;

         foreach (string fieldName in fieldsMap.Keys)
         {
            System.Reflection.PropertyInfo? propertyInfo = Instalacion.GetType().GetProperty(fieldName);
            if (propertyInfo != null)
            {
               object? value5 = propertyInfo.GetValue(Instalacion);
               if (value5 != null) fieldsMap[fieldName].SetValue(value5.ToString());
            }
         }

         if (Instalacion.TotalPico > Instalacion.TotalNominal)
            PotenciaGeneracion = Instalacion.TotalPico;
         else PotenciaGeneracion = Instalacion.TotalNominal;
         fieldsMap["PotenciaGeneracion"].SetValue(PotenciaGeneracion.ToString());

         var NominalIndividual = Instalacion.TotalNominal / Instalacion.TotalInversores;

         if (fieldsMap.TryGetValue("Fusible", out PdfFormField? value))
         {
            value.SetValue(Instalacion.Fusible.Split('A')[0]);
            fieldsMap["IAutomatico"].SetValue(Instalacion.IAutomatico.Split('A')[0]);
            fieldsMap["IDiferencial"].SetValue(Instalacion.IDiferencial.Split(' ')[2].Replace("mA", ""));
            fieldsMap["NominalIndividual"].SetValue(NominalIndividual.ToString());
            fieldsMap["SeccionFase1"].SetValue(seccionFase);
         }

         if (Instalacion.Tipo.Equals("AISLADA"))
            ((PdfButtonFormField)fieldsMap["Aislada"]).SetRadio(true);
         else if (Instalacion.Tipo.Equals("AUTOCONSUMO"))
            ((PdfButtonFormField)fieldsMap["ConExcedentes"]).SetRadio(true);
         else ((PdfButtonFormField)fieldsMap["SinExcedentes"]).SetRadio(true);
      }

      // DETERMINA SI EL VATIMETRO ES MONOFASICO O TRIFASICO
      public bool IsMonofasico(string Vatimetro)
      {
         return Vatimetro.StartsWith("EMETER") || Vatimetro.StartsWith("SMART") || Vatimetro.StartsWith("EM11");
      }
   }
}