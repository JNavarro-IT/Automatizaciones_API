using backend_API.Models.Dto;
using backend_API.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
   //CONTROLADOR DE LAS PETICIONES HTTP PARA OBTENER ARCHIVOS
   [EnableCors("Cors")]
   [ApiController]
   [Route("[controller]")]
   public class MicroservicesController : ControllerBase
   {
      private readonly IEXCELServices _excelServices;
      private readonly IWORDService _wordService;
      private readonly IPVGISService _pvgisService;
      private readonly IPDFService _pdfService;
      private readonly ILogger<MicroservicesController> _logger;

      //CONSTRUCTOR POR PARÁMETROS
      public MicroservicesController(IEXCELServices excelServices, IWORDService wordService, IPVGISService pvgisService, IPDFService pdfService, ILogger<MicroservicesController> logger)
      {
         _excelServices = excelServices;
         _wordService = wordService;
         _pvgisService = pvgisService;
         _pdfService = pdfService;
         _logger = logger;      
      }

      // PETICIÓN PARA GENERAR ARCHIVO EXCEL
      [HttpPost("crearExcel")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status409Conflict)]
      [ProducesResponseType(StatusCodes.Status200OK)]

      public ActionResult CrearExcel(ProyectoDto Proyecto)
      {
         if (Proyecto == null) return BadRequest("El proyecto no existe");

         try
         {
            string tempPath = _excelServices.CreateEXCEL(Proyecto);
            if (tempPath is null) return Conflict("Ruta null");

            byte[] fileBytes = Convert.FromBase64String(tempPath);

            return Ok(new FileStreamResult(new MemoryStream(fileBytes), "application/vnd.ms-excel") { FileDownloadName = Proyecto.Referencia + ".xlsx" }); 
         } 
         catch (Exception ex) 
         { 
            throw new("EXCEPTION: " + ex.Message + "HELP: " + ex.HelpLink); 
         }   
      }

      // PETICIÓN PARA CREAR MEMORIAS WORD
      [HttpPost("crearMemorias")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> CrearMemorias(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
         {
            return NotFound("El proyecto no está registrado en la base de datos");
         }

         string newRuta = _wordService.InitServiceWORD(Proyecto);

         return newRuta == null
             ? (ActionResult<string>)StatusCode(503, "Error al generar los archivos WORDs. ERROR: " + newRuta)
             : (ActionResult<string>)Ok("Memorias creadas con éxito. RUTA: " + newRuta);
      }

      // PETICIÓN PARA CREAR ARCHIVO PDF EN PVGIS
      [HttpPost("crearPVGIS")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> CrearPVGIS(ProyectoDto Proyecto)
      {
         if (Proyecto == null) return NotFound("El proyecto no está registrado en la base de datos");

         string newRuta = _pvgisService.CreatePVGIS(Proyecto);

         return newRuta == null
             ? (ActionResult<string>)StatusCode(503, "Error al generar el archivo del PVGIS. ERROR: " + newRuta)
             : (ActionResult<string>)Ok("Archivo PVGIS creado con éxito. RUTA: " + newRuta);
      }

      // PETICIÓN PARA OBTENER LOS DOUMENTOS DE LEGALIZACIONES
      [HttpPost("crearLegalizaciones")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> CrearLegalizaciones(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
         {
            return NotFound("El proyecto no está registrado en la base de datos");
         }

         string newRuta = _pdfService.InitFillPDF(Proyecto);

         return newRuta == null
             ? (ActionResult<string>)StatusCode(503, "Error al generar los archivos de subvenciones. ERROR: " + newRuta)
             : (ActionResult<string>)Ok("Archivos de subvenciones creados con éxito. RUTA: " + newRuta);
      }
   }
}