
using backend_API.Utilities;

namespace backend_API.Dto
{
    public class InstalacionDto : DtoBase
    {
        public int IdInstalacion { get; set; }
        public double Inclinacion { get; set; } = 0;
        public string Azimut { get; set; } = string.Empty;
        public double TotalPico { get; set; } = 0;
        public double TotalNominal { get; set; } = 0;
        public List<CadenaDto> Cadenas { get; set; } = new();
        public string Tipo { get; set; } = string.Empty;
        public double CoordXConexion { get; set; } = 0;
        public double CoordYConexion { get; set; } = 0;
        public List<ModuloDto> Modulos { get; set; } = new();
        public List<InversorDto>? Inversores { get; set; } = new();
        public string Fusible { get; set; } = string.Empty;
        public string IDiferencial { get; set; } = string.Empty;
        public string IMagenetico { get; set; } = string.Empty;
        public string Estructura { get; set; } = string.Empty;
        public ProyectoDto? Proyecto { get; set; } = new();
        public InstalacionDto() { }

    }
}
