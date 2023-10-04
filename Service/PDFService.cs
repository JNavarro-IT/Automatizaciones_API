using System.Globalization;
using backend_API.Dto;
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
      public string pathBase = "Resources\\Subvenciones\\";

      public string InitFillPDF(ProyectoDto Proyecto)
      {
         var downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
         var folderEnd = Path.Combine(downloads, ("Documentacion_" + Proyecto.Referencia));
         Directory.CreateDirectory(folderEnd);

         try
         {
            if (Proyecto != null)
            {
               UbicacionDto Ubicacion = Proyecto.Cliente.Ubicaciones[0];
               var Instalacion = Proyecto.Instalacion;

               if (Ubicacion.CCAA != null || Ubicacion.CCAA != "")
               {
                  var folderCCAA = pathBase + Ubicacion.CCAA;
                  List<string> pathsOrigin = Directory.GetFiles(folderCCAA).ToList();
                 
                  if (Ubicacion.CCAA.Equals("Andalucia")) 
                  { 
                     var porcentaje = (Instalacion.ConsumoEstimado / Instalacion.GeneracionAnual) * 100;

                     if (porcentaje >= 78) pathsOrigin.Remove(folderCCAA + "/2AND_NAME.docx");
                        else pathsOrigin.Remove(folderCCAA + "/1AND_NAME.docx");

                     var Provincia = _projectService.WithoutTildes(Ubicacion.Provincia);
                     if (Provincia.Equals("Jaen")) 
                     {
                        bool eco3 = _projectService.CheckMunicipio(Ubicacion);
                        
                        if (eco3) pathsOrigin = new(){ folderCCAA + "/AND1_NAME.pdf" };
                           else pathsOrigin.Remove(folderCCAA + "/AND1_NAME.pdf");
                     
                     } else pathsOrigin.Remove(folderCCAA + "/AND1_NAME.pdf");
                  }
                  _projectService.ClonarFiles(Proyecto, pathsOrigin.ToArray(), folderEnd);
                  var pathsEndLegal = Directory.GetFiles(folderEnd);
                  FillPDFs(Proyecto, pathsOrigin.ToArray(), pathsEndLegal);

               } else return "ERROR => Se requiere de una CCAA para obtener el documento...";
            } else return "ERROR => Proyecto erróneo: " + Proyecto;

         } catch (Exception error) { return "ERROR => " + error.Message; }

         if (folderEnd.Contains("ERROR")) return folderEnd;
         else return "OK => Los archivos se crearon con éxito. RUTA: " + folderEnd;
      }

      public string FillPDFs(ProyectoDto Proyecto, string[] pathsOriginLegal, string[] pathsEndLegal)
      {
         if (Proyecto != null)
         {
            var Cliente = Proyecto.Cliente;
            var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
            var Instalacion = Proyecto.Instalacion;
            var Referencias = Proyecto.Referencia.Split("-");
            var index = 0;

            foreach (var path in pathsOriginLegal)
            {
               if (Path.GetExtension(path).Equals(".pdf"))
               {
                  using (PdfDocument pdfDoc = new(new PdfReader(path), new PdfWriter(pathsEndLegal[index])))
                  {
                     var acroForm = PdfAcroForm.GetAcroForm(pdfDoc, true);
                     var fieldsMap = acroForm.GetAllFormFields();

                     if (fieldsMap.ContainsKey("Referencia0"))
                     {
                        for (int i = 0; i < Referencias.Length; i++)
                           ((PdfTextFormField)acroForm.GetField("Referencia" + i)).SetValue(Referencias[i]);

                        var check = IsMonofasico(Instalacion.Vatimetro);
                        if (check) ((PdfButtonFormField)acroForm.GetField("Monofasico")).SetRadio(true);
                        else ((PdfButtonFormField)acroForm.GetField("Trifasico")).SetRadio(true);
                     }

                     FillCliente(Cliente, fieldsMap);
                     FillUbicacion(Ubicacion, fieldsMap);
                     FillInstalacion(Instalacion, fieldsMap);
                     FillDate(fieldsMap);
                     pdfDoc.Close();
                  }
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
         else return "ERROR => el proyecto no es válido";
      }

      // RELLENAR EL FORMULARIO CON LA FECHA ACTUAÑ
      public void FillDate(IDictionary<string, PdfFormField> fieldsMap)
      {
         var Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToUpper();

         if (fieldsMap.ContainsKey("Dia"))
         {
            ((PdfTextFormField)fieldsMap["Dia"]).SetValue(DateTime.Now.Day.ToString());
            ((PdfTextFormField)fieldsMap["Mes"]).SetValue(Mes);
            ((PdfTextFormField)fieldsMap["Año"]).SetValue(DateTime.Now.Year.ToString());
         }
      }

      // RELLENA EL FORMULARIO CON LOS DATOS DE UN CLIENTE
      public void FillCliente(ClienteDto? Cliente, IDictionary<string, PdfFormField> fieldsMap)
      {
         if (Cliente != null)
         {
            foreach (var fieldName in fieldsMap.Keys)
            {
               var propertyInfo = Cliente.GetType().GetProperty(fieldName);
               if (propertyInfo != null)
               {
                  var value = propertyInfo.GetValue(Cliente);
                  if (value != null)
                     ((PdfTextFormField)fieldsMap[fieldName]).SetValue(value.ToString());
               }

               if (fieldsMap.ContainsKey("Nombre2"))
                  ((PdfTextFormField)fieldsMap["Nombre2"]).SetValue(Cliente.Nombre);

               if (fieldsMap.ContainsKey("Dni2"))
                  ((PdfTextFormField)fieldsMap["Dni2"]).SetValue(Cliente.Dni);
            }
         }
      }

      // RELLENAR EL FORMULARIO CON LOS DATOS DE LA UBICAVION 
      public void FillUbicacion(UbicacionDto Ubicacion, IDictionary<string, PdfFormField> fieldsMap)
      {
         if (Ubicacion != null)
         {
            var CpArray = Ubicacion.Cp.ToString().ToCharArray();

            foreach (var fieldName in fieldsMap.Keys)
            {
               if (fieldName != null)
               {
                  var propertyInfo = Ubicacion.GetType().GetProperty(fieldName);

                  if (propertyInfo != null)
                  {
                     var value = propertyInfo.GetValue(Ubicacion);
                     if (value != null && fieldName != null)
                        ((PdfTextFormField)fieldsMap[fieldName]).SetValue(value.ToString());
                  }
               }
            }

            for (int i = 0; i < CpArray.Length; i++)
            {
               if (fieldsMap.ContainsKey("Cp" + i) || fieldsMap.ContainsKey("CpBis" + i))
               {
                  ((PdfTextFormField)fieldsMap["Cp" + i]).SetValue(CpArray[i].ToString());
                  ((PdfTextFormField)fieldsMap["CpBis" + i]).SetValue(CpArray[i].ToString());
               }

               if ((i > 0 && i < 3) && fieldsMap.ContainsKey("Municipio" + i))
               {
                  ((PdfTextFormField)fieldsMap["Municipio" + i]).SetValue(Ubicacion.Municipio);
                  ((PdfTextFormField)fieldsMap["Provincia" + i]).SetValue(Ubicacion.Provincia);
               }
            }
            if (fieldsMap.ContainsKey("Direccion1"))
               ((PdfTextFormField)fieldsMap["Direccion1"]).SetValue(Ubicacion.GetDireccion());

            if (fieldsMap.ContainsKey("Provincia3"))
               ((PdfTextFormField)fieldsMap["Provincia3"]).SetValue(Ubicacion.Provincia.ToUpper());

            if (fieldsMap.ContainsKey("cbProvincia"))
               ((PdfChoiceFormField)fieldsMap["cbProvincia"]).SetValue(Ubicacion.Provincia.ToUpper());

            if (fieldsMap.ContainsKey("CpBis"))
               ((PdfTextFormField)fieldsMap["CpBis"]).SetValue(Ubicacion.Cp.ToString());

         }
      }

      // RELLENAR EL FORMULARIO CON LOS DATOS DE LA INSTALACIÓN
      public void FillInstalacion(InstalacionDto Instalacion, IDictionary<string, PdfFormField> fieldsMap)
      {
         foreach (var fieldName in fieldsMap.Keys)
         {
            var propertyInfo = Instalacion.GetType().GetProperty(fieldName);
            if (propertyInfo != null)
            {
               var value = propertyInfo.GetValue(Instalacion);
               if (value != null)
                  ((PdfFormField)fieldsMap[fieldName]).SetValue(value.ToString());
            }

            if (fieldsMap.ContainsKey("Fusible"))
            {
               ((PdfFormField)fieldsMap["Fusible"]).SetValue(Instalacion.Fusible.Split('A')[0]);
               ((PdfFormField)fieldsMap["IAutomatico"]).SetValue(Instalacion.IAutomatico.Split('A')[0]);
               ((PdfFormField)fieldsMap["IDiferencial"]).SetValue(Instalacion.IDiferencial.Split(' ')[2].Replace("mA", ""));
               ((PdfFormField)fieldsMap["TotalNominal2"]).SetValue(Instalacion.TotalNominal.ToString());

               var sf = Instalacion.SeccionFase.ToString();
               var seccionFase = sf + "/" + sf + "/" + sf;
               for (int i = 1; i < 3; i++)
                  ((PdfFormField)fieldsMap["SeccionFase" + i]).SetValue(seccionFase);
            }
         }
      }

      // DETERMINA SI EL VATIMETRO ES MONOFASICO O TRIFASICO
      public bool IsMonofasico(string Vatimetro)
      {
         if (Vatimetro.StartsWith("EMETER") || Vatimetro.StartsWith("SMART") || Vatimetro.StartsWith("EM11")) return true;
         else return false;
      }
   }
}