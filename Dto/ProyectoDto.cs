
using System.Globalization;
using backend_API.Utilities;

namespace backend_API.Dto
{
   public class ProyectoDto : DtoBase
   {
      public int IdProyecto { get; set; }
      public string Referencia { get; set; } = string.Empty;
      public string Version { get; set; } = "1.0";
      public DateTime Fecha { get; set; } = new();
      public string FechaStr
      {
         get { return Fecha.ToString("yyyy-MM-dd"); }
         set { }
      }

      public double? Presupuesto { get; set; }
      public double? PresupuestoSyS { get; set; }
      public DateTime PlazoEjecucion { get; set; } = new();
      public string PlazoEjecucionStr
      {
         get { return PlazoEjecucion.ToString("yyyy-MM-dd"); }
         set { }
      }
      public string OCA { get; set; } = string.Empty;
      public string NumOCA { get; set; } = string.Empty;
      public string RefOCA {  get; set; } = string.Empty;
      public ClienteDto Cliente { get; set; } = new();
      public InstalacionDto Instalacion { get; set; } = new();
      public List<LugarPRLDto>? LugaresPRL { get; set; } = new();

      // CONSTRUCTOR POR DEFECTO
      public ProyectoDto() { }
   }
}