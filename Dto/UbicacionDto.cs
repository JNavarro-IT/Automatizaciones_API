using backend_API.Utilities;

namespace backend_API.Dto
{
   // CLASE DTO PARA TRANSFORMAR LA INFORMACIÓN DE LA ENTIDAD UBICACIÓN
   public class UbicacionDto : DtoBase
   {
      public int IdUbicacion { get; set; }
      public string RefCatastral { get; set; } = string.Empty;
      public string Calle { get; set; } = string.Empty;
      public string Numero { get; set; } = string.Empty;
      public string Bloque {  get; set; } = string.Empty;
      public string Portal { get; set; } = string.Empty;
      public string Escalera { get; set; } = string.Empty;
      public string Piso { get; set; } = string.Empty;
      public string Puerta { get; set; } = string.Empty;
      public int Cp { get; set; } = 0;
      public string Municipio { get; set; } = string.Empty;
      public string Provincia { get; set; } = string.Empty;
      public double Superficie { get; set; } = 0;
      public double? CoordXUTM { get; set; }
      public double? CoordYUTM { get; set; }
      public double Latitud { get; set; } = 0;
      public double Longitud { get; set; } = 0;
      public string Cups { get; set; } = string.Empty;
      public string Empresa { get; set; } = string.Empty;
      public string Cif { get; set; } = string.Empty;

      public int IdCliente { get; set; } = new();

      public UbicacionDto() { }
   }
}
