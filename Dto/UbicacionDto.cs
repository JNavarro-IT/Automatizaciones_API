
using backend_API.Utilities;

namespace backend_API.Dto
{
    public class UbicacionDto : DtoBase
    {
        public int IdUbicacion { get; set; }
        public string Ref_catastral { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int CP { get; set; } = 0;
        public string Municipio { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public double Superficie { get; set; } = 0;
        public double Latitud { get; set; } = 0;
        public double Longitud { get; set; } = 0;

        public int IdCliente { get; set; } = new();
        public List<CubiertaDto> Cubiertas { get; set; } = new();

        public UbicacionDto() { }
    }
}
