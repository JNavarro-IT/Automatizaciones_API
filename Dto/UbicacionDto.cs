using backend_API.Utilities;

namespace backend_API.Dto
{
    // CLASE DTO PARA TRANSFORMAR LA INFORMACIÓN DE LA ENTIDAD UBICACIÓN
    public class UbicacionDto : DtoBase
    {
        public int IdUbicacion { get; set; }
        public string RefCatastral { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int CP { get; set; } = 0;
        public string Municipio { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public double Superficie { get; set; } = 0;
        public double Latitud { get; set; } = 0;
        public double Longitud { get; set; } = 0;
        public string Cups { get; set; } = string.Empty;
        public int IdCliente { get; set; } = new();
        public int IdInstalacion { get; set; } = new();
        public int? IdProyecto { get; set; }

        public List<CubiertaDto> Cubiertas { get; set; } = new();

        public UbicacionDto() { }
    }
}
