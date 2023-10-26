using System.Globalization;
using backend_API.Models.Dto;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace backend_API.Service
{
   // INTERFAZ QUE FUNCIONA COMO SERVICIO PARA OTRAS CLASES MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IPDFService
   {
      public string InitFillPDF(ProyectoDto Proyecto);
      public string FillPDFs(ProyectoDto Proyecto, string[] pathsOriginLegal, string[] pathsEndLegal);
      public void FillUbicacion(UbicacionDto Ubicacion, IDictionary<string, PdfFormField> fieldsMap);
      public void FillInstalacion(InstalacionDto Instalacion, IDictionary<string, PdfFormField> fieldsMap);
      public void FillDate(IDictionary<string, PdfFormField> fieldsMap);
   }

   public class PDFService(IProjectService projectService, IWORDService wordService) : IPDFService
   {
      public readonly IProjectService _projectService = projectService;
      public readonly IWORDService _wordService = wordService;
      public Dictionary<string, PdfFormField> FieldsMap = new();
      public string pathBase = "Utilities/Resources/Subvenciones/";

      public string InitFillPDF(ProyectoDto Proyecto)
      {
         double? porcentajeGeneracion = 0;
         double diferenciaGeneración = 0;
         string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
         string folderEnd = Path.Combine(downloads, "Documentacion_" + Proyecto.Referencia);
         _ = Directory.CreateDirectory(folderEnd);

         try
         {
            if (Proyecto != null)
            {
               UbicacionDto? Ubicacion = Proyecto.Cliente.Ubicaciones[0];
               InstalacionDto Instalacion = Proyecto.Instalacion;

               if (Ubicacion.CCAA is not "")
               {
                  string folderCCAA = pathBase + Ubicacion.CCAA;
                  List<string> pathsOrigin = Directory.GetFiles(folderCCAA).ToList();

                  if (Ubicacion.CCAA.Equals("Andalucia"))
                  {
                     porcentajeGeneracion = Instalacion.ConsumoEstimado / Instalacion.GeneracionAnual * 100;

                     _ = porcentajeGeneracion >= 78 ? pathsOrigin.Remove(folderCCAA + "/2AND_NAME.docx") : pathsOrigin.Remove(folderCCAA + "/1AND_NAME.docx");

                     bool eco3 = _projectService.CheckMunicipio(Ubicacion);

                     if (eco3) pathsOrigin = new() { folderCCAA + "/AND1_NAME.pdf" };
                     else _ = pathsOrigin.Remove(folderCCAA + "/AND1_NAME.pdf");
                    
                  }
                  _ = _projectService.ClonarFiles(Proyecto, pathsOrigin.ToArray(), folderEnd);
                  string[] pathsEndLegal = Directory.GetFiles(folderEnd);
                  _ = FillPDFs(Proyecto, pathsOrigin.ToArray(), pathsEndLegal);
               }
               else return "ERROR => Se requiere de una CCAA para obtener el documento...";
            }
            else return "ERROR => Proyecto erróneo: " + Proyecto;
            
         }
         catch (Exception error) { return "ERROR => " + error.Message; }

         return folderEnd.Contains("ERROR") ? folderEnd : "OK => Los archivos se crearon con éxito. RUTA: " + folderEnd;
      }

      public string FillPDFs(ProyectoDto Proyecto, string[] pathsOriginLegal, string[] pathsEndLegal)
      {
         if (Proyecto != null)
         {
            ClienteDto Cliente = Proyecto.Cliente;
            UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
            InstalacionDto Instalacion = Proyecto.Instalacion;
            string[] Referencias = Proyecto.Referencia.Split("-");
            int index = 0;

            foreach (string path in pathsOriginLegal)
            {
               if (Path.GetExtension(path).Equals(".pdf"))
               {
                  using PdfDocument pdfDoc = new(new PdfReader(path), new PdfWriter(pathsEndLegal[index]));
                  PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(pdfDoc, true);
                  IDictionary<string, PdfFormField> fieldsMap = acroForm.GetAllFormFields();

                  if (fieldsMap.ContainsKey("Referencia0"))
                  {
                     for (int i = 0; i < Referencias.Length; i++)
                     {
                        _ = ((PdfTextFormField)acroForm.GetField("Referencia" + i)).SetValue(Referencias[i]);
                     }

                     bool check = IsMonofasico(Instalacion.Vatimetro);
                     _ = check
                         ? ((PdfButtonFormField)acroForm.GetField("Monofasico")).SetRadio(true)
                         : ((PdfButtonFormField)acroForm.GetField("Trifasico")).SetRadio(true);
                  }

                  FillCliente(Cliente, fieldsMap);
                  FillUbicacion(Ubicacion, fieldsMap);
                  FillInstalacion(Instalacion, fieldsMap);
                  FillDate(fieldsMap);
                  pdfDoc.Close();
               }
               else if (Path.GetExtension(path).Equals(".docx"))
               {
                  string[] filesWord = [pathsEndLegal[0]];
                  Dictionary<string, string?> MapWORD = _wordService.CreateMap(Proyecto);
                  string result = _wordService.CreateMemorias(MapWORD, filesWord);
               }
               index++;
            }
            return "OK => Los archivos se han generado con éxito";
         }
         else
         {
            return "ERROR => el proyecto no es válido";
         }
      }

      // RELLENAR EL FORMULARIO CON LA FECHA ACTUAÑ
      public void FillDate(IDictionary<string, PdfFormField> fieldsMap)
      {
         string Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToUpper();

         if (fieldsMap.ContainsKey("Dia"))
         {
            _ = ((PdfTextFormField)fieldsMap["Dia"]).SetValue(DateTime.Now.Day.ToString());
            _ = ((PdfTextFormField)fieldsMap["Mes"]).SetValue(Mes);
            _ = ((PdfTextFormField)fieldsMap["Año"]).SetValue(DateTime.Now.Year.ToString());
         }
      }

      // RELLENA EL FORMULARIO CON LOS DATOS DE UN CLIENTE
      public void FillCliente(ClienteDto? Cliente, IDictionary<string, PdfFormField> fieldsMap)
      {
         if (Cliente != null)
         {
            foreach (string fieldName in fieldsMap.Keys)
            {
               System.Reflection.PropertyInfo? propertyInfo = Cliente.GetType().GetProperty(fieldName);
               if (propertyInfo != null)
               {
                  object? value = propertyInfo.GetValue(Cliente);
                  if (value != null)
                  {
                     _ = ((PdfTextFormField)fieldsMap[fieldName]).SetValue(value.ToString());
                  }
               }

               if (fieldsMap.ContainsKey("Nombre2"))
               {
                  _ = ((PdfTextFormField)fieldsMap["Nombre2"]).SetValue(Cliente.Nombre);
               }

               if (fieldsMap.ContainsKey("Dni2"))
               {
                  _ = ((PdfTextFormField)fieldsMap["Dni2"]).SetValue(Cliente.Dni);
               }
            }
         }
      }

      // RELLENAR EL FORMULARIO CON LOS DATOS DE LA UBICAVION 
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
                     object? value = propertyInfo.GetValue(Ubicacion);
                     if (value != null && fieldName != null)
                     {
                        _ = ((PdfTextFormField)fieldsMap[fieldName]).SetValue(value.ToString());
                     }
                  }
               }
            }

            for (int i = 0; i < CpArray.Length; i++)
            {
               if (fieldsMap.ContainsKey("Cp" + i) || fieldsMap.ContainsKey("CpBis" + i))
               {
                  _ = ((PdfTextFormField)fieldsMap["Cp" + i]).SetValue(CpArray[i].ToString());
                  _ = ((PdfTextFormField)fieldsMap["CpBis" + i]).SetValue(CpArray[i].ToString());
               }

               if (i > 0 && i < 3 && fieldsMap.ContainsKey("Municipio" + i))
               {
                  _ = ((PdfTextFormField)fieldsMap["Municipio" + i]).SetValue(Ubicacion.Municipio);
                  _ = ((PdfTextFormField)fieldsMap["Provincia" + i]).SetValue(Ubicacion.Provincia);
               }
            }
            if (fieldsMap.ContainsKey("Direccion1"))
            {
               _ = ((PdfTextFormField)fieldsMap["Direccion1"]).SetValue(Ubicacion.GetDireccion());
            }

            if (fieldsMap.ContainsKey("Provincia3"))
            {
               _ = ((PdfTextFormField)fieldsMap["Provincia3"]).SetValue(Ubicacion.Provincia.ToUpper());
            }

            if (fieldsMap.ContainsKey("cbProvincia"))
            {
               _ = ((PdfChoiceFormField)fieldsMap["cbProvincia"]).SetValue(Ubicacion.Provincia.ToUpper());
            }

            if (fieldsMap.ContainsKey("CpBis"))
            {
               _ = ((PdfTextFormField)fieldsMap["CpBis"]).SetValue(Ubicacion.Cp.ToString());
            }
         }
      }

      // RELLENAR EL FORMULARIO CON LOS DATOS DE LA INSTALACIÓN
      public void FillInstalacion(InstalacionDto Instalacion, IDictionary<string, PdfFormField> fieldsMap)
      {
         foreach (string fieldName in fieldsMap.Keys)
         {
            System.Reflection.PropertyInfo? propertyInfo = Instalacion.GetType().GetProperty(fieldName);
            if (propertyInfo != null)
            {
               object? value = propertyInfo.GetValue(Instalacion);
               if (value != null) _ = fieldsMap[fieldName].SetValue(value.ToString());
            }

            if (fieldsMap.ContainsKey("Fusible"))
            {
               _ = fieldsMap["Fusible"].SetValue(Instalacion.Fusible.Split('A')[0]);
               _ = fieldsMap["IAutomatico"].SetValue(Instalacion.IAutomatico.Split('A')[0]);
               _ = fieldsMap["IDiferencial"].SetValue(Instalacion.IDiferencial.Split(' ')[2].Replace("mA", ""));
               _ = fieldsMap["TotalNominal2"].SetValue(Instalacion.TotalNominal.ToString());

               string? sf = Instalacion.SeccionFase.ToString();
               string seccionFase = sf + "/" + sf + "/" + sf;
               for (int i = 1; i < 3; i++)
                  _ = fieldsMap["SeccionFase" + i].SetValue(seccionFase);
            }
         }
      }

      // DETERMINA SI EL VATIMETRO ES MONOFASICO O TRIFASICO
      public bool IsMonofasico(string Vatimetro)
      {
         return Vatimetro.StartsWith("EMETER") || Vatimetro.StartsWith("SMART") || Vatimetro.StartsWith("EM11");
      }
   }
}