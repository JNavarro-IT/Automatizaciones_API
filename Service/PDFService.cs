using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Pdf;
using backend_API.Dto;
using System.Globalization;
using System.Text.RegularExpressions;
using Xceed.Words.NET;
using Xceed.Document.NET;

namespace backend_API.Service
{
   public interface IPDFService
   {
      public string InitFillPDF(ProyectoDto Proyecto);
      public string FillAdecuacion(ProyectoDto Proyecto, string rutaDestino);
      public string FillAnexo(ProyectoDto Proyecto, string rutaDestino);
      public string FillCAU(ProyectoDto Proyecto, string rutaDestino);
      public string FillCertificadoBT(ProyectoDto Proyecto, string rutaDestino);
      public void FillOCA(ProyectoDto Proyecto, PdfAcroForm form);
      public void FillDate(PdfAcroForm form);

   }
   public class PDFService : IPDFService
   {
      public PDFService() { }

      public string InitFillPDF(ProyectoDto Proyecto)
      {
         string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
         string folder = "Legalizaciones_" + Proyecto.Cliente.Nombre;
         string carpetaDestino = Path.Combine(downloads, folder);
         Directory.CreateDirectory(carpetaDestino);
         string carpetaOriginal = "Resources/Legalizaciones";
         string[]? rutasOriginal = Directory.GetFiles(carpetaOriginal);

         try
         {
            foreach (string ruta in rutasOriginal)
            {
               string nombreFile = Path.GetFileName(ruta) + "_" + Proyecto.Cliente.Nombre;
               string rutaDestino = Path.Combine(carpetaDestino, nombreFile);
               File.Copy(ruta, rutaDestino, true);
            }

            string[]? rutasDestino = Directory.GetFiles(carpetaDestino);
            FillAdecuacion(Proyecto, rutasDestino[0] + ".docx");
            FillAnexo(Proyecto, rutasDestino[1] + ".pdf");
            FillCAU(Proyecto, rutasDestino[2] + ".pdf");
            FillCertificadoBT(Proyecto, rutasDestino[3] + ".pdf");
        
            return carpetaDestino;
         }
         catch (Exception error)
         {
            return "No se han generado los archivos de Legalizaciones " + error.Message;
         }
      }

      public string FillAdecuacion(ProyectoDto Proyecto, string rutaDestino)
      {
         var Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);

         Dictionary<string, string?> reemplazos = new()
         {
            { "#Nombre", Proyecto.Cliente.Nombre },
            { "#Cups", Proyecto.Cliente.Ubicaciones[0].Cups },
            { "#Dia", DateTime.Today.Day.ToString()},
            { "#Mes", Mes },
            { "#Año", DateTime.Today.Year.ToString() },
         };

         using (DocX doc = DocX.Load(rutaDestino))
         {
            foreach (Paragraph parrafo in doc.Paragraphs)
            {
               foreach (var kvp in reemplazos)
               {
                  if (parrafo.Text.Contains(kvp.Key))
                     doc.ReplaceText(kvp.Key, kvp.Value, false, RegexOptions.IgnoreCase);
               }
            }    
            doc.Save();
            return (rutaDestino);
         }
      }

      public string FillAnexo(ProyectoDto Proyecto, string rutaDestino)
      {
         ClienteDto Cliente = Proyecto.Cliente;
         UbicacionDto Ubicacion = Cliente.Ubicaciones[0];
         var CpArray = Ubicacion.Cp.ToString().ToCharArray();
         string fileAnexo = "Resources/Legalizaciones/Anexo_III.pdf";

         using (PdfDocument pdfDoc = new(new PdfReader(fileAnexo), new PdfWriter(rutaDestino)))
         {
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            FillCliente(Cliente, form);
            FillUbicacion(Ubicacion, form);
            var textField = (PdfTextFormField)form.GetField("denominacion");
            var value = textField.GetValue() + Ubicacion.Municipio + ", " + Ubicacion.Provincia;
            textField.SetValue(value);

            for (int i = 0; i < 5; i++)
            {
               textField = (PdfTextFormField)form.GetField("cp" + i);
               textField.SetValue(CpArray[i].ToString());
               textField = (PdfTextFormField)form.GetField("cpBis" + i);
               textField.SetValue(CpArray[i].ToString());
            }
            pdfDoc.Close();
            return rutaDestino;
         }
         return rutaDestino;
      }

      public string FillCAU(ProyectoDto Proyecto, string rutaDestino)
      {
         string fileCAU = "Resources/Legalizaciones/CAU_RD244_2019.pdf";
         using (PdfDocument pdfDoc = new(new PdfReader(fileCAU), new PdfWriter(rutaDestino)))
         {
            var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
            var form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            var textField = (PdfTextFormField)form.GetField("cups");
            textField.SetValue(Ubicacion.Cups);
            FillDate(form);
            pdfDoc.Close();
            return rutaDestino;
         }
      }

      public string FillCertificadoBT(ProyectoDto Proyecto, string rutaDestino)
      {
         string fileBT = "Resources/Legalizaciones/CertificadoBT.pdf";
         var Cliente = Proyecto.Cliente;
         var Ubicacion = Cliente.Ubicaciones[0];
         var Instalacion = Proyecto.Instalacion;

         using (PdfDocument pdfDoc = new(new PdfReader(fileBT), new PdfWriter(rutaDestino)))
         {
            var form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            var textField = (PdfTextFormField)form.GetField("referencia0");
            textField.SetValue(Proyecto.Referencia.Split("-")[0]);
            textField = (PdfTextFormField)form.GetField("referencia1");
            textField.SetValue(Proyecto.Referencia.Split("-")[1]);
            textField = (PdfTextFormField)form.GetField("referencia2");
            textField.SetValue(Proyecto.Referencia.Split("-")[2]);

            FillCliente(Cliente, form);
            FillUbicacion(Ubicacion, form);
            FillInstalacion(Instalacion, form);
            if (Instalacion.TotalNominal >= 25)
               FillOCA(Proyecto, form);

            FillDate(form);
            pdfDoc.Close();
            return rutaDestino;
         }
      }
     
      public void FillOCA(ProyectoDto Proyecto,  PdfAcroForm form)
      {
         var textField = (PdfTextFormField)form.GetField("OCA");
         textField.SetValue(Proyecto.OCA);
         textField = (PdfTextFormField)form.GetField("numOCA");
         textField.SetValue(Proyecto.NumOCA);
         textField = (PdfTextFormField)form.GetField("inspeccionOCA");
         textField.SetValue(Proyecto.RefOCA); 
      }

      public void FillDate(PdfAcroForm form)
      {
         var textField = (PdfTextFormField)form.GetField("dia");
         textField.SetValue(DateTime.Now.Day.ToString());
         textField = (PdfTextFormField)form.GetField("mes");
         textField.SetValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToUpper());
         textField = (PdfTextFormField)form.GetField("año");
         textField.SetValue(DateTime.Now.Year.ToString());
      }

      public void FillCliente(ClienteDto Cliente,  PdfAcroForm form)
      {
         var textField = (PdfTextFormField)form.GetField("nombre");
         textField.SetValue(Cliente.Nombre);
         textField = (PdfTextFormField)form.GetField("dni");
         textField.SetValue(Cliente.Dni);
         textField = (PdfTextFormField)form.GetField("email");
         textField.SetValue(Cliente.Email);
         textField = (PdfTextFormField)form.GetField("telefono");
         textField.SetValue(Cliente.Telefono);
      }

      public void FillUbicacion(UbicacionDto Ubicacion,  PdfAcroForm form)
      {
         var Direccion = Ubicacion.Calle + ", " + Ubicacion.Numero + ", "
               + Ubicacion.Bloque + ", " + Ubicacion.Portal + ", " + Ubicacion.Escalera
               + ", " + Ubicacion.Piso + ", " + Ubicacion.Puerta;

         var textField = (PdfTextFormField)form.GetField("direccion");
         textField.SetValue(Direccion);
         textField = (PdfTextFormField)form.GetField("cp");
         textField.SetValue(Ubicacion.Cp.ToString());
         textField = (PdfTextFormField)form.GetField("municipio");
         textField.SetValue(Ubicacion.Municipio);
         textField = (PdfTextFormField)form.GetField("provincia");
         textField.SetValue(Ubicacion.Provincia);
         textField = (PdfTextFormField)form.GetField("calle");
         textField.SetValue(Ubicacion.Calle);
         textField = (PdfTextFormField)form.GetField("numero");
         textField.SetValue(Ubicacion.Numero);
         textField = (PdfTextFormField)form.GetField("bloque");
         textField.SetValue(Ubicacion.Bloque);
         textField = (PdfTextFormField)form.GetField("portal");
         textField.SetValue(Ubicacion.Portal);
         textField = (PdfTextFormField)form.GetField("escalera");
         textField.SetValue(Ubicacion.Escalera);
         textField = (PdfTextFormField)form.GetField("piso");
         textField.SetValue(Ubicacion.Piso);
         textField = (PdfTextFormField)form.GetField("puerta");
         textField.SetValue(Ubicacion.Puerta);
         textField = (PdfTextFormField)form.GetField("municipio2");
         textField.SetValue(Ubicacion.Municipio);
         PdfChoiceFormField choiceField = (PdfChoiceFormField)form.GetField("cbProvincia");
         choiceField.SetValue(Ubicacion.Provincia);
         textField = (PdfTextFormField)form.GetField("cp2");
         textField.SetValue(Ubicacion.Cp.ToString());
         textField = (PdfTextFormField)form.GetField("cups");
         textField.SetValue(Ubicacion.Cups);
         textField = (PdfTextFormField)form.GetField("superficie");
         textField.SetValue(Ubicacion.Superficie.ToString());
         textField = (PdfTextFormField)form.GetField("empresa");
         textField.SetValue(Ubicacion.Empresa);
         textField = (PdfTextFormField)form.GetField("cif");
         textField.SetValue(Ubicacion.Cif);
      }

      public void FillInstalacion(InstalacionDto Instalacion,  PdfAcroForm form)
      {
         var textField = (PdfTextFormField)form.GetField("fusible");
         textField.SetValue(Instalacion.Fusible.Split('A')[0]);
         textField = (PdfTextFormField)form.GetField("nivelAislamiento");
         textField.SetValue("");
         textField = (PdfTextFormField)form.GetField("materialAislamiento");
         textField.SetValue(""); 
         textField = (PdfTextFormField)form.GetField("materialConductor");
         textField.SetValue("");
         textField = (PdfTextFormField)form.GetField("seccionFase");
         textField.SetValue("");
         textField = (PdfTextFormField)form.GetField("totalPico");
         textField.SetValue(Instalacion.TotalPico.ToString());
         textField = (PdfTextFormField)form.GetField("totalPico2");
         textField.SetValue(Instalacion.TotalPico.ToString());
         textField = (PdfTextFormField)form.GetField("VO");
         textField.SetValue(Instalacion.Cadenas[0].Inversor.VO.ToString());
         textField = (PdfTextFormField)form.GetField("nivelAislamiento2");
         textField.SetValue("");
         textField = (PdfTextFormField)form.GetField("materialAislamiento2");
         textField.SetValue("");
         textField = (PdfTextFormField)form.GetField("materialConductor2");
         textField.SetValue(Instalacion.TotalNominal + "");
         textField = (PdfTextFormField)form.GetField("seccionFase2");
         textField.SetValue("");
     
         textField = (PdfTextFormField)form.GetField("iAutomatico");
         textField.SetValue(Instalacion.IAutomatico.Split("A")[0]);
         textField = (PdfTextFormField)form.GetField("iDiferencial");
         textField.SetValue(Instalacion.IDiferencial.Split("A")[0]);
         textField = (PdfTextFormField)form.GetField("totalNominal");
         textField.SetValue(Instalacion.TotalNominal.ToString());        
         textField = (PdfTextFormField)form.GetField("iAutomatico");
         textField.SetValue(Instalacion.Vatimetro);
      }
      /*
      public bool isChecked(string Vatimetro)
      {
         if (Vatimetro.Contains("EMETER") || Vatimetro.Contains("SMART") || Vatimetro.Contains("EM11"))
            return true;
      }*/
   }
}
