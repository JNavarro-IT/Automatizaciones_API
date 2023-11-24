using Automatizaciones_API.Configurations;

namespace Automatizaciones_API.Models.Dto
{
   public class ProyectoDto : DtoBase
   {
      public int? IdProyecto { get; set; }
      public string Referencia { get; set; } = string.Empty;
      public double Version { get; set; } = 1.0;
      public DateTime Fecha { get; set; } = new();
      public string FechaStr { get => Fecha.ToString("yyyy-MM-dd"); }
      public double? Presupuesto { get; set; } = 0;
      public double? PresupuestoSyS { get; set; } = 0;
      public DateTime PlazoEjecucion { get; set; } = new();
      public string PlazoEjecucionStr { get => PlazoEjecucion.ToString("yyyy-MM-dd"); }
      public string? OCA { get; set; } = string.Empty;
      public string? NumOCA { get; set; } = string.Empty;
      public string? InspeccionOCA { get; set; } = string.Empty;

      // RELATIONS
      public ClienteDto Cliente { get; set; } = new();
      public InstalacionDto Instalacion { get; set; } = new();
      public List<LugarPRLDto> LugaresPRL { get; set; } = [];

      // CONSTRUCTOR POR DEFECTO
      public ProyectoDto() { }
   }
}