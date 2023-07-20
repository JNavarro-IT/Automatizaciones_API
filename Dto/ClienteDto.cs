using backend_API.Utilities;

namespace backend_API.Dto 
{
    //CLASE DTO PARA TRANSPORTAR LA INFORMACION DE LA ENTIDAD CLIENTE
    public class ClienteDto : DtoBase
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Observaciones { get; set; } = string.Empty;
        public List<ProyectoDto>? Proyectos { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public ClienteDto() { }
    }
}