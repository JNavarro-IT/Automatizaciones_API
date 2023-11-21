using Automatizaciones_API.Models.Dto;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Automatizaciones_API.Service
{
   // INTERFAZ PARA OBTENER UN ARCHIVO PDF DE LA WEB DEL PVGIS MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IPVGISService
   {
      public string CreatePVGIS(ProyectoDto Proyecto);
      public Task<string> GetJSON(ProyectoDto Proyecto);
      public void PDFGenerator();
   }

   // CLASE QUE IMPLEMENTA IPVGISService PARA OBTENER DATOS FOTOVOLTAICOS EN UN ARCHIVO PDF SACADO DESDE LA WEB OFICIAL DEL PVGIS DE LA COMISIÓN EUROPEA
   public class PVGISServices : IPVGISService
   {
      string Latitud = "";
      string Longitud = "";
      string Inclinacion = "";
      string Azimuth = "";
      string? TotalPico = "";
      string? datosJSON = null;
      ChromeDriver? browser = null;

      // INICIALIZACIÓN DEL PROCESO DE OBTENCIÓN DEL ARCHIVO DEL PVGIS EMULANDO LA NAVEGACIÓN POR LA PÁGINA OFICIAL DE LA COMISIÓN EUROPEA 
      public string CreatePVGIS(ProyectoDto Proyecto)
      {
         if (Proyecto == null) return "ERROR => El proyecto no es válido";

         var fileName = Proyecto.Referencia + "_PVGIS-5.pdf";
         var tempFolder = Directory.CreateDirectory("Utilities/Temp").FullName;
         var tempFile = Path.Combine(tempFolder, fileName);

         try
         {
            datosJSON = GetJSON(Proyecto).GetAwaiter().GetResult();

            if (datosJSON.Length > 0)
            {
               PDFGenerator();
               return tempFile;
            }
            else return "ERROR => No se han obtenido datos del PVGIS-5";

         }
         catch (Exception ex) { return "EXCEPTION => Al generar el archivo PVGIS: " + ex.Message; }
      }

      public async Task<string> GetJSON(ProyectoDto Proyecto)
      {
         var Ubicacion = Proyecto.Cliente.Ubicaciones[0];
         var Instalacion = Proyecto.Instalacion;

         try
         {
            Latitud = Ubicacion.Latitud.ToString();
            Longitud = Ubicacion.Longitud.ToString();
            Inclinacion = Instalacion.Inclinacion.ToString();
            Azimuth = Instalacion.Azimut.Split(" ")[0];
            TotalPico = Instalacion.TotalPico.ToString();

            string apiUrl = $"https://re.jrc.ec.europa.eu/api/v5_2/PVcalc?outputformat=basic&lat={Latitud}&lon={Longitud}&peakpower={TotalPico}&loss=14&angle={Inclinacion}&aspect={Azimuth}";

            using HttpClient client = new();
            try
            {
               HttpResponseMessage response = await client.GetAsync(apiUrl);
               if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
               else return "ERROR => Mientras se generaba el archivo PVGIS:  " + response.RequestMessage;

            }
            catch (Exception ex) { return "EXCEPTION => Mientras se generaba el archivo PVGIS: " + ex.Message; }

         }
         catch (Exception ex) { return "EXCEPTION => Al obtener el archivo JSON: " + ex.Message; }
      }

      public ChromeOptions GetOptions()
      {
         ChromeOptions options = new();
         options.AddArgument("--silent");
         options.AddArgument("--incognito");
         options.AddArgument("--mute-audio");
         options.AddArgument("--no-sandbox");
         options.AddArgument("--headless=new");

         options.AddArgument("--disable-gpu");
         options.AddArgument("--disable-logging");
         options.AddArgument("--disable-features");
         options.AddArgument("--disable-infobars");

         options.AddArgument("--disable-extensions");
         options.AddArgument("--disable-web-security");
         options.AddArgument("--disable-popup-blocking");
         options.AddArgument("--disable-settings-window");
         options.AddArgument("--disable-impl-side-painting");
         options.AddArgument("--disable-features=UserAgentClientHint");
         options.AddArgument("--disable-blink-features=AutomationControlled");

         options.AddArgument("--dns-prefetch-disable");
         options.AddArguments("--blink-settings=imagesEnabled=false");

         options.AddArgument("--enable-javascript");
         options.AddArgument("--enable-automation");
         options.AddArguments("--allow-file-access");
         options.AddArguments("--allow-file-access-from-files");
         options.AddArgument("--allow-running-insecure-content");
         options.AddArguments("--allow-cross-origin-auth-prompt");

         options.AddArguments("user-data-dir=" + "Utilities/Temp");

         options.AddArgument("--dump-dom");
         options.AddArgument("--kiosk");

         options.AddArgument("--log-level=3");
         options.AddArgument("--remote-debugging-port=8087");

         options.DebuggerAddress = "192.168.2.250:8087";

         options.AddLocalStatePreference("browser.download.folderList", 2);
         options.AddUserProfilePreference("download.directory_upgrade", true);
         options.AddUserProfilePreference("download.prompt_for_download", false);
         options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
         options.AddLocalStatePreference("browser.helperApps.alwaysAsk.force", false);
         options.AddUserProfilePreference("download.default_directory", "Utilities/Temp");
         options.AddLocalStatePreference("profile.managed_default_content_settings.popups", 2);
         options.AddLocalStatePreference("profile.managed_default_content_settings.cookies", 2);
         options.AddLocalStatePreference("profile.managed_default_content_settings.plugins", 2);
         options.AddLocalStatePreference("profile.default_content_setting_values.geolocation", 2);
         options.AddLocalStatePreference("profile.default_content_setting_values.notifications", 2);
         options.AddLocalStatePreference("profile.managed_default_content_settings.geolocation", 2);
         options.AddLocalStatePreference("profile.managed_default_content_settings.media_stream", 2);

         return options;
      }

      public IWebElement FillValues(string idElement, string? value)
      {
         IWebElement? element = null;
         var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
         element = wait.Until(brw => brw.FindElement(By.Id(idElement)));

         if (value != null)
         {
            element.Clear();
            element.SendKeys(value);

         }
         else element.Click();

         return element;
      }
      // NAVEGAR POR LA WEB DEL PVGIS Y GENERAR EL PDF
      public void PDFGenerator()
      {
         // CONFIGURAR OPCIONES Y ABRIR CHROME

         browser = new(GetOptions())
         {
            Url = "https://re.jrc.ec.europa.eu/pvg_tools/en/tools.html"
         };

         string? clickValue = null;
         //  DATOS Latitud Y Longitud
         FillValues("inputLat", Latitud);
         FillValues("inputLon", Longitud);

         // CLICK EN BOTÓN 'GO' PARA OBTENER UBICACIÓN
         FillValues("btninputLatLon", clickValue);

         // DATOS TotalPico, Inclinacion y Azimuth
         FillValues("peakpower2", TotalPico);
         FillValues("angle", Inclinacion);
         FillValues("aspect", Azimuth);

         // CLICK EN "Visualize results" para INSERTAR DATOS y GENERAR RESULTADOS
         FillValues("btviewPVGridGraph", clickValue);

         // CLICK EN "PDF" y DESCARGA EL ARCHIVO SOLO
         FillValues("PVP_print", clickValue);

         // CERRAR CHROME
         browser.Close();
      }
   }
}