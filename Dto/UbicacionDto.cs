using backend_API.Utilities;

namespace backend_API.Dto
{
   // CLASE DTO PARA TRANSFORMAR LA INFORMACIÓN DE LA ENTIDAD UBICACIÓN
   public class UbicacionDto : DtoBase
   {
      public int? IdUbicacion { get; set; }
      public string RefCatastral { get; set; } = string.Empty;
      public string Antiguedad { get; set; } = string.Empty;
      public string Via { get; set; } = string.Empty;
      public string Calle { get; set; } = string.Empty;
      public string Numero { get; set; } = string.Empty;
      public string Km { get; set; } = string.Empty;
      public string Bloque { get; set; } = string.Empty;
      public string Portal { get; set; } = string.Empty;
      public string Escalera { get; set; } = string.Empty;
      public string Piso { get; set; } = string.Empty;
      public string Puerta { get; set; } = string.Empty;
      public string? Direccion { get; set; } = string.Empty;
      public int Cp { get; set; } = 0;
      public string Municipio { get; set; } = string.Empty;
      public string Provincia { get; set; } = string.Empty;
      public string? CCAA { get; set; } = string.Empty;
      public double Superficie { get; set; } = 0;
      public double? CoordXUTM { get; set; }
      public double? CoordYUTM { get; set; }
      public double Latitud { get; set; } = 0;
      public double Longitud { get; set; } = 0;
      public string Cups { get; set; } = string.Empty;
      public string Empresa { get; set; } = string.Empty;
      public string Cif { get; set; } = string.Empty;
      public string Cau { get; set; } = string.Empty;

      // RELATION
      public int? IdCliente { get; set; } = new();

      // CONSTRUCTOR POR DEFECTO
      public UbicacionDto() { }

      // GENERAR UNA DIRECCION CON UN FORMATO CONCRETO
      public string GetDireccion()
      {
         if (!Km.Equals("")) Direccion += Via + " " + Calle;
         if (!Km.Equals("")) Direccion += ", Km." + Km;
         Direccion += ", " + Numero;
         if (!Bloque.Equals("")) Direccion += ", Blq." + Bloque;
         if (!Portal.Equals("")) Direccion += ", P." + Portal;
         if (!Escalera.Equals("")) Direccion += ", Esc." + Escalera;
         if (!Piso.Equals("")) Direccion += ", " + Piso;
         if (!Puerta.Equals("")) Direccion += Puerta.ToUpper();

         return Direccion;
      }
   }
}