using Automatizaciones_API.Models;
using Automatizaciones_API.Models.Dto;
using Automatizaciones_API.Repository;
using Automatizaciones_API.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace Automatizaciones_API.Controllers
{
   //CONTROLADOR DE LAS PETICIONES HTTP PARA OBTENER ARCHIVOS
   [ApiController]
   [Route("[controller]")]
   public class MicroservicesController : ControllerBase
   {
      private readonly IEXCELServices _excelServices;
      private readonly IWORDService _wordService;
      private readonly IPVGISService _pvgisService;
      private readonly IPDFService _pdfService;
      private readonly IProjectService _projectService;
      private readonly IBaseRepository<Proyecto, ProyectoDto> _projectRepository;

      //CONSTRUCTOR POR PARÁMETROS
      public MicroservicesController(IEXCELServices excelServices, IWORDService wordService, IPVGISService pvgisService, IPDFService pdfService, IProjectService projectService, IBaseRepository<Proyecto, ProyectoDto> projectRepository)
      {
         _excelServices = excelServices;
         _wordService = wordService; 
         _pvgisService = pvgisService;
         _pdfService = pdfService;
         _projectService = projectService;
         _projectRepository = projectRepository;
      }

      // PETICIÓN PARA GENERAR ARCHIVO EXCEL
      [HttpPost("crearExcel")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status409Conflict)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string>? CrearExcel(ProyectoDto Proyecto)
      {
         var result = ValidateAndClean(Proyecto);
         if (result == null || result.Value.StartsWith("ERROR")) return result;

         try
         {
            var tempFile = _excelServices.CreateEXCEL(Proyecto);
            if (tempFile is null || tempFile.StartsWith("ERROR") || tempFile.StartsWith("EXCEPTION"))
               return Conflict(tempFile);

            var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite);
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Proyecto.Referencia + "_MEMORIAS.xlsx");

         }
         catch (Exception ex) { return StatusCode(503, "EXCEPTION => " + ex.Message); }
      }

      // PETICIÓN PARA CREAR MEMORIAS WORD
      [HttpPost("crearMemorias")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string>? CrearMemorias(ProyectoDto Proyecto)
      {
         var result = ValidateAndClean(Proyecto);
         if (result == null || result.Value.StartsWith("ERROR")) return result;

         string? tempPath;
         var memoryStream = new MemoryStream();

         try
         {
            tempPath = _wordService.InitServiceWORD(Proyecto);

            if (tempPath.StartsWith("ERROR") || tempPath.StartsWith("EXCEPTION")) return Conflict(tempPath);

            var pathsWORD = Directory.GetFiles(tempPath);
            if (pathsWORD.Length == 0) return NotFound("ERROR => No se generaron archivos WORD en la carpeta temporal");

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
               foreach (var path in pathsWORD)
               {
                  var fileName = Path.GetFileName(path);
                  var fileInfo = new FileInfo(fileName);
                  var zipEntry = zipArchive.CreateEntry(fileInfo.Name);

                  using var zipStream = zipEntry.Open();
                  using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                  fileStream.CopyTo(zipStream);
               }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/zip", Proyecto.Referencia + "_MEMORIAS_WORD.zip");

         }
         catch (Exception ex) { return StatusCode(503, "EXCEPTION => " + ex.Message); }
      }

      // PETICIÓN PARA CREAR ARCHIVO PDF EN PVGIS
      [HttpPost("crearPVGIS")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string>? CrearPVGIS(ProyectoDto Proyecto)
      {
         var result = ValidateAndClean(Proyecto);
         if (result == null || result.Value.StartsWith("ERROR")) return result;

         try
         {
            string? tempFile = _pvgisService.CreatePVGIS(Proyecto);

            if (tempFile == null || tempFile.StartsWith("ERROR") || tempFile.StartsWith("EXCEPTION"))
               return Conflict("CONFLICT: " + tempFile);

            FileStream fileStream = new(tempFile, FileMode.Open, FileAccess.ReadWrite);
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Proyecto.Referencia + "_PVGIS.pdf");

         }
         catch (Exception ex) { return StatusCode(503, "EXCEPTION => " + ex.Message); }
      }

      // PETICIÓN PARA OBTENER LOS DOUMENTOS DE LEGALIZACIONES
      [HttpPost("crearLegalizaciones")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public ActionResult<string>? CrearLegalizaciones(ProyectoDto Proyecto)
      {
         var result = ValidateAndClean(Proyecto);
         if (result == null || result.Value.StartsWith("ERROR")) return result;

         string newRuta = _pdfService.InitFillPDF(Proyecto);
         return newRuta == null
             ? StatusCode(503, "Error al generar los archivos de subvenciones. ERROR: " + newRuta)
             : Ok("Archivos de subvenciones creados con éxito. RUTA: " + newRuta);
      }

      private ActionResult<string> ValidateAndClean(ProyectoDto Proyecto)
      {
         if (Proyecto == null) return BadRequest("ERROR => Proyecto erróneo o mal estructurado");




         bool? exist = _projectRepository.EntityExists(Proyecto).Result;
         if (Proyecto.IdProyecto == null) return NotFound("ERROR => Proyecto no tiene identificación");
         if (exist.HasValue) return NotFound("ERROR => Proyecto no está registrado en la Base de Datos");

         if (!_projectService.DeleteTemp()) return Conflict("ERROR => No se han podido eliminar los archivos antiguos");

         return Ok();
      }
   }


}