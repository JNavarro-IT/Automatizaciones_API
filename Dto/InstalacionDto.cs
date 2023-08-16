
using backend_API.Utilities;

namespace backend_API.Dto
{
    public class InstalacionDto : DtoBase
    {
        // CLASE DTO QUE TRANSPORTA LA INFORMACIÓN DE LA ENTIDAD INSTALACIÓN
        public int? IdInstalacion { get; set; }
        public double Inclinacion { get; set; } = 0;
        public string Azimut { get; set; } = string.Empty;
        public double TotalPico { get; set; } = 0;
        public double TotalNominal { get; set; } = 0;
        public string Tipo { get; set; } = string.Empty;
        public double CoordXConexion { get; set; } = 0;
        public double CoordYConexion { get; set; } = 0;
        public string Fusible { get; set; } = string.Empty;
        public string IDiferencial { get; set; } = string.Empty;
        public string IAutomatico { get; set; } = string.Empty;
        public string Estructura { get; set; } = string.Empty;
        public string Vatimetro { get; set; } = string.Empty;
        public int? TotalInversores { get; set; }
        public int TotalModulos { get; set; } = 0;
        public int? TotalCadenas { get; set; }

        //RELATIONS
        public IList<CubiertaDto> Cubiertas { get; set; } = new List<CubiertaDto>();
        public IList<CadenaDto> Cadenas { get; set; } = new List<CadenaDto>();
        public int IdUbicacion { get; set; } = 0;
        public int? IdProyecto { get; set; } = 0;

        //CONSTRUCTOR POR DEFECTO
        public InstalacionDto() { }

    }
}
