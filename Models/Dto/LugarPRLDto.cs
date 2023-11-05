using Automatizaciones_API.Utilities;

namespace Automatizaciones_API.Models.Dto
{
   //CLASE QUE REPRESENTA UN LUGAR IMPRESCINDIBLE PARA PROTOCOLOS DE PRL
   public class LugarPRLDto : DtoBase
   {
      public int? IdLugarPRL { get; set; }
      public string Nombre { get; set; } = string.Empty;
      public string Tipo { get; set; } = string.Empty;
      public string Direccion { get; set; } = string.Empty;
      public string Calle { get; set; } = string.Empty;
      public string Numero { get; set; } = string.Empty;
      public int Cp { get; set; } = 0;
      public string Municipio { get; set; } = string.Empty;
      public string Provincia { get; set; } = string.Empty;
      public string Telefono { get; set; } = string.Empty;
      public string Email { get; set; } = string.Empty;
      public string? NIMA { get; set; } = string.Empty;
      public string? Autorizacion { get; set; } = string.Empty;
      public double Latitud { get; set; } = 0;
      public double Longitud { get; set; } = 0;
      public string? RutaImg { get; set; } = string.Empty;

      //CONSTRUCTOR POR DEFECTO
      public LugarPRLDto() { }
   }
}