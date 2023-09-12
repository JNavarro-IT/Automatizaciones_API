using backend_API.Dto;
using backend_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend_API.Controllers
{
   //CONTROLADOR DE LAS PETICIONES HTTP PARA OBTENER ARCHIVOS
   [ApiController]
   [Route("[controller]")]
   public class MicroservicesController : ControllerBase
   {
      private readonly IExcelServices _excelServices;
      private readonly IWORDService _wordService;
      private readonly IPVGISService _pvgisService;
      private readonly IPDFService _pdfService;

      //CONSTRUCTOR POR PARÁMETROS
      public MicroservicesController(IExcelServices excelServices, IWORDService wordService, IPVGISService pvgisService, IPDFService pdfService)
      {
         _excelServices = excelServices;
         _wordService = wordService;
         _pvgisService = pvgisService;
         _pdfService = pdfService;
      }

      // PETICIÓN DE GENERAR ARCHIVO EXCEL
      [HttpPost("crearExcel")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> CrearExcel(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return BadRequest("El proyecto no existe");

         var rutaNewExcel = _excelServices.CreateEXCEL(Proyecto);

         if(rutaNewExcel == null)
            return NoContent();

         return Ok(rutaNewExcel);
      }

      [HttpPost("crearMemorias")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<string>> CrearMemorias(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return NotFound("El proyecto no está registrado en la base de datos");

         var newRuta = await _wordService.CreateMemorias(Proyecto);

         if (newRuta == null)
            return StatusCode(503, "Error al generar los archivos WORDs. ERROR: " + newRuta);

         return Ok("Memorias creadas con éxito. RUTA: " + newRuta);
      }

      [HttpPost("crearPVGIS")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> CrearPVGIS(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return NotFound("El proyecto no está registrado en la base de datos");

         var newRuta = _pvgisService.CreatePVGIS(Proyecto);

         if(newRuta == null)
            return StatusCode(503, "Error al generar el archivo del PVGIS. ERROR: " + newRuta);

         return Ok("Archivo PVGIS creado con éxito. RUTA: " + newRuta);
      }

      [HttpPost("crearPDF")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string> CrearPDFs(ProyectoDto Proyecto)
      {
         if (Proyecto == null)
            return NotFound("El proyecto no está registrado en la base de datos");

         var newRuta = _pdfService.InitFillPDF(Proyecto);

         if (newRuta == null || !newRuta.Contains(".pdf"))
            return StatusCode(503, "Error al generar los archivos de subvenciones. ERROR: " + newRuta);

         return Ok("Archivos de subvenciones creados con éxito. RUTA: " + newRuta);
      }
   }
}