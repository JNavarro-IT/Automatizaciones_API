using Automatizaciones_API.Utilities;

namespace Automatizaciones_API.Models.Dto
{
   public class InstalacionDto : DtoBase
   {
      // CLASE DTO QUE TRANSPORTA LA INFORMACIÓN DE LA ENTIDAD INSTALACIÓN
      public int? IdInstalacion { get; set; }
      public double Inclinacion { get; set; } = 0;
      public string Azimut { get; set; } = string.Empty;
      public string Tipo { get; set; } = string.Empty;
      public double CoordXConexion { get; set; } = 0;
      public double CoordYConexion { get; set; } = 0;
      public double? PotenciaContratada { get; set; } = 0;
      public double? ConsumoEstimado { get; set; } = 0;
      public double? GeneracionAnual { get; set; } = 0;
      public string Fusible { get; set; } = string.Empty;
      public string IDiferencial { get; set; } = string.Empty;
      public string IAutomatico { get; set; } = string.Empty;
      public string Estructura { get; set; } = string.Empty;
      public string Definicion { get => GetDefinicion(Estructura); }
      public string Vatimetro { get; set; } = string.Empty;
      public int? SeccionFase { get; set; } = 0;
      public double? TotalPico { get; set; } = 0;
      public double? TotalNominal { get; set; } = 0;
      public int? TotalInversores { get; set; } = 0;
      public int? TotalModulos { get; set; } = 0;
      public int? TotalCadenas { get; set; } = 0;
      public string? DirectorObra { get; set; }
      public string? Titulacion { get; set; }
      public string? ColeOficial { get; set; }
      public string? NumColegiado { get; set; }

      //RELATIONS
      public IList<CubiertaDto> Cubiertas { get; set; } = new List<CubiertaDto>();
      public IList<CadenaDto> Cadenas { get; set; } = new List<CadenaDto>();

      //CONSTRUCTOR POR DEFECTO
      public InstalacionDto() { }

      public string GetDefinicion(string Estructura)
      {
         return Estructura.Equals("Coplanar")
             ? "La estructura COPLANAR, teniendo en cuenta la integración urbanística" +
           " un módulo fotovoltaico instalado de forma coplanar con la cubierta será menos " +
           " visto desde el exterior que uno instalado triangularmente con una determinada inclinación."
             : "La estructura TRIANGULAR, teniendo en cuenta la optimización de la" +
            " irradiación solar la estructura triangular te permite orientar los módulos" +
            " fotovoltaicos, así como acercarse a la inclinación optima en la latitud geográfica," +
            " en condiciones de cubiertas planas o de poca inclinación.";
      }
   }
}
