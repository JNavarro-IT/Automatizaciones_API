using backend_API.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace backend_API.Service
{
   public interface IPVGISService
   {
      public string CreatePVGIS(ProyectoDto Proyecto);
      public Task<string> GetJSON(ProyectoDto Proyecto);
      public void PDFGenerator();

   }

   class PVGISService : IPVGISService
   {
      string lat = "";
      string lon = "";
      string inclinacion = "";
      string azimuth = "";
      string potenciaPico = "";
      string? datosJSON = null;

      public string CreatePVGIS(ProyectoDto Proyecto)
      {
         try
         {
            datosJSON = GetJSON(Proyecto).GetAwaiter().GetResult();

            if (datosJSON.Length > 0)
            {
               PDFGenerator();
               string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
               string[] rutas = Directory.GetFiles(downloads);
               string nombreCliente = Proyecto.Cliente.Nombre;
               string newFileName = "PVGIS-5_" + nombreCliente + ".pdf";
               string newFilePath = "";
               foreach (var ruta in rutas)
               {
                  string nombreFile = Path.GetFileName(ruta);
                  if (nombreFile.Contains("PVGIS"))
                  {
                     newFilePath = Path.Combine(downloads, newFileName);
                     File.Move(ruta, newFilePath);
                     return "Fichero generado con éxito. RUTA: " + newFilePath;

                  }   
               }
               return "Fichero no encontrado";
            }
            else 
               return "No se han obtenido datos del PVGIS-5";
         }
         catch (Exception ex) { return $"Error: {ex.Message}"; }
      }

      public async Task<string> GetJSON(ProyectoDto Proyecto)
      {
         try
         {
            lat = Proyecto.Cliente.Ubicaciones[0].Latitud + "";
            lon = Proyecto.Cliente.Ubicaciones[0].Longitud + "";
            inclinacion = Proyecto.Instalacion.Inclinacion + "";
            azimuth = Proyecto.Instalacion.Azimut.Split(" ")[0];
            potenciaPico = Proyecto.Instalacion.TotalPico + "";
            string apiUrl = $"https://re.jrc.ec.europa.eu/api/v5_2/PVcalc?outputformat=basic&lat={lat}&lon={lon}&peakpower={potenciaPico}&loss=14&angle={inclinacion}&aspect={azimuth}";

            using (HttpClient client = new())
            {
               try
               {
                  HttpResponseMessage response = await client.GetAsync(apiUrl);
                  if (response.IsSuccessStatusCode)
                  {
                     datosJSON = await response.Content.ReadAsStringAsync();
                     return datosJSON;
                  }
                  else return "La petición no fue exitosa. STATUS: " + response.StatusCode;
               }
               catch (Exception ex) { return "Error al realizar la petición: " + ex.Message; }
            }
         }
         catch (Exception ex) { return "Error al guardar JSON: " + ex.Message; }
      }

      // NAVEGAR POR LA WEB DEL PVGIS Y GENERAR EL PDF
      public void PDFGenerator()
      {
         //Configurar y abrir Chrome
         var options = new ChromeOptions();
         options.AddArgument("--headless=new");
         options.AddUserProfilePreference("download.default_directory", "Files");
         ChromeDriver automatic = new(options)
         {
            Url = "https://re.jrc.ec.europa.eu/pvg_tools/en/tools.html"
         };
         Thread.Sleep(2000);

         //Introducir datos lan/lon
         IWebElement webElement = automatic.FindElement(By.Id("inputLat"));
         webElement.SendKeys(lat);
         webElement = automatic.FindElement(By.Id("inputLon"));
         webElement.SendKeys(lon);
         Thread.Sleep(2000);

         //Click en "GO" (Botón para que lea lat/lon)
         webElement = automatic.FindElement(By.Id("btninputLatLon"));
         webElement.Click();
         Thread.Sleep(2000);
         Console.Error.WriteLine("holaaaa");

         //Introducir poteciaPico, inclinacion y azimuth
         webElement = automatic.FindElement(By.Id("peakpower2"));
         webElement.Clear();
         webElement.SendKeys(potenciaPico);
         Thread.Sleep(1000);

         webElement = automatic.FindElement(By.Id("angle"));
         webElement.Clear();
         webElement.SendKeys(inclinacion);
         Thread.Sleep(2000);

         webElement = automatic.FindElement(By.Id("aspect"));
         webElement.Clear();
         webElement.SendKeys(azimuth);
         Thread.Sleep(1000);

         //Click en "Visualize results" para que agregue los datos y genere los resultados
         webElement = automatic.FindElement(By.Id("btviewPVGridGraph"));
         webElement.Click();
         Thread.Sleep(2000);

         //Click en "PDF" y descarga
         webElement = automatic.FindElement(By.Id("PVP_print"));
         webElement.Click();
         Thread.Sleep(4000);
         automatic.Close();
      }
   }
}