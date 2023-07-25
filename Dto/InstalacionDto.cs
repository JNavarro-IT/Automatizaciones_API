
using backend_API.Utilities;

namespace backend_API.Dto
{
    public class InstalacionDto : DtoBase
    {
        // CLASE DTO QUE TRANSPORTA LA INFORMACIÓN DE LA ENTIDAD INSTALACIÓN
        public int IdInstalacion { get; set; }
        public double Inclinacion { get; set; } = 0;
        public string Azimut { get; set; } = string.Empty;
        public double TotalPico { get; set; } = 0;
        public double TotalNominal { get; set; } = 0;
        public string Tipo { get; set; } = string.Empty;
        public double CoordXConexion { get; set; } = 0;
        public double CoordYConexion { get; set; } = 0;
        public string Fusible { get; set; } = string.Empty;
        public string IDiferencial { get; set; } = string.Empty;
        public string IMagenetico { get; set; } = string.Empty;
        public string Estructura { get; set; } = string.Empty;
        public string Vatimetro { get; set; } = string.Empty;

        //RELATIONS
        public List<CadenaDto> Cadenas { get; set; } = new();
        public UbicacionDto Ubicacion { get; set; } = new();
        public ProyectoDto? Proyecto { get; set; } = new();

        //CONSTRUCTOR POR DEFECTO
        public InstalacionDto() { }

    }
}
