using backend_API.Utilities;

namespace backend_API.Dto
{
    //CLASE DTO QUE TRANSPORTA DATOS DE LA ENTIDAD CUBIERTA
    public class CubiertaDto : DtoBase
    {
        public int IdCubierta { get; set; }
        public string MedidasColectivas { get; set; } = string.Empty;
        public string Accesibilidad { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public int Ubicacion { get; set; } = new();
        public int Instalacion { get; set; } = new();
        public CubiertaDto() { }
    }
}
