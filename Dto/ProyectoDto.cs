
using backend_API.Utilities;

namespace backend_API.Dto
{
    public class ProyectoDto : DtoBase
    {
        public int IdProyecto { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public string Version { get; set; } = "1.0";
        public DateTime Fecha { get; set; } = new();
        public string? Justificacion { get; set; }
        public double? Presupuesto { get; set; }
        public double? PresupuestoSyS { get; set; }
        public DateTime? PlazoEjecucion { get; set; }
        public ClienteDto Cliente { get; set; } = new();
        public InstalacionDto Instalacion { get; set; } = new();
        public List<LugarPRLDto>? Lugares { get; set; } = new();

        public ProyectoDto() { }
    }
}