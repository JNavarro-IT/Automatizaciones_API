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
   public class MicroservicesController(IEXCELService excelService, IWORDService wordService,
                    IPVGISService pvgisService, IPDFService pdfService, IProjectService projectService,
                    IBaseRepository<Proyecto, ProyectoDto> projectRepository) : ControllerBase
   {
      private readonly IEXCELService _excelService = excelService;
      private readonly IWORDService _wordService = wordService;
      private readonly IPVGISService _pvgisService = pvgisService;
      private readonly IPDFService _pdfService = pdfService;
      private readonly IProjectService _projectService = projectService;
      private readonly IBaseRepository<Proyecto, ProyectoDto> _projectRepository = projectRepository;

      // PETICIÓN PARA GENERAR ARCHIVO EXCEL
      [HttpPost("crearExcel")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status409Conflict)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<string>> CrearExcel(ProyectoDto Proyecto)
      {
         var validation = await ValidateAndClean(Proyecto);


         if (validation.Result is BadRequestObjectResult || validation.Result is NotFoundObjectResult || validation.Result is ConflictObjectResult)
            return validation.Result;

         try
         {
            var (excelStream, error) = _excelService.CreateEXCEL(Proyecto);
            if (excelStream is null) return Conflict(error);

            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{Proyecto.Referencia}_REF_MEMORIAS.xlsx");

         }
         catch (Exception ex) { return StatusCode(500, "EXCEPTION => " + ex.Message + ", " + ex.StackTrace); }
      }

      // PETICIÓN PARA CREAR MEMORIAS WORD
      [HttpPost("crearMemorias")]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<ActionResult<string>?> CrearMemorias(ProyectoDto Proyecto)
      {
         var validation = await ValidateAndClean(Proyecto);

         if (validation.Result is BadRequestObjectResult || validation.Result is NotFoundObjectResult || validation.Result is ConflictObjectResult)
            return validation.Result;

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
      public async Task<ActionResult<string>?> CrearPVGIS(ProyectoDto Proyecto)
      {
         var validation = await ValidateAndClean(Proyecto);

         if (validation.Result is BadRequestObjectResult || validation.Result is NotFoundObjectResult || validation.Result is ConflictObjectResult)
            return validation.Result;

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
      public async Task<ActionResult<string>> CrearLegalizaciones(ProyectoDto Proyecto)
      {
         var validation = await ValidateAndClean(Proyecto);

         if (validation.Result is BadRequestObjectResult || validation.Result is NotFoundObjectResult || validation.Result is ConflictObjectResult)
            return validation.Result;

         string newRuta = _pdfService.InitFillPDF(Proyecto);
         return newRuta == null
             ? StatusCode(404, "Error al generar los archivos de subvenciones. ERROR: " + newRuta)
             : Ok("Archivos de subvenciones creados con éxito. RUTA: " + newRuta);
      }

      // REALIZAR VALIDACIONES SOBRE UN PROYECTO Y ELIMINAR LOS ARCHIVOS TEMPORALES SI LOS HUBIERA
      private async Task<ActionResult<string>> ValidateAndClean(ProyectoDto Proyecto)
      {
         if (Proyecto == null) return BadRequest("ERROR => Proyecto erróneo o mal estructurado");
         if (Proyecto.IdProyecto == null) return NotFound("ERROR => Proyecto no tiene identificación");

         bool? exist = await _projectRepository.EntityExists(Proyecto);

         if (exist is false) return NotFound("ERROR => Proyecto no está registrado en la Base de Datos");
         if (!_projectService.DeleteTemp()) return Conflict("ERROR => No se han podido eliminar los archivos antiguos");

         return Ok();
      }
   }
}