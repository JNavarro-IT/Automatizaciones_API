using backend_API.Utilities;

namespace backend_API.Models.Dto
{
   //CLASE DTO PARA TRANSPORTAR LA INFORMACIÓN DE LA ENTIDAD CLIENTE
   public class ClienteDto : DtoBase
   {
      public int? IdCliente { get; set; }
      public string Nombre { get; set; } = string.Empty;
      public string Dni { get; set; } = string.Empty;
      public string Telefono { get; set; } = string.Empty;
      public string? Email { get; set; } = string.Empty;
      public string? Observaciones { get; set; } = string.Empty;

      //RELATIONS
      public List<UbicacionDto> Ubicaciones { get; set; } = new List<UbicacionDto>();

      //CONSTRUCTOR POR DEFECTO
      public ClienteDto() { }
   }
}