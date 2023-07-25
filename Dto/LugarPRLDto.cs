
using backend_API.Utilities;

namespace backend_API.Dto
{
    //CLASE QUE REPRESENTA UN LUGAR IMPRESCINDIBLE PARA PROTOCOLOS DE PRL
    public class LugarPRLDto : DtoBase
    {
        public int IdLugar { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int CP { get; set; } = 0;
        public string Municipio { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? NIMA { get; set; } = string.Empty;
        public string? Autorizacion { get; set; } = string.Empty;

        public LugarPRLDto() { }
    }
}