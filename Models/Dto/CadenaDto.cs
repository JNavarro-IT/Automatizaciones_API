using Automatizaciones_API.Utilities;

namespace Automatizaciones_API.Models.Dto
{
   //CLASE DTO QUE TRANSPORTA DATOS DE LA ENTIDAD CADENA
   public class CadenaDto : DtoBase
   {
      public int? IdCadena { get; set; }
      public int MinModulos { get; set; } = 0;
      public int MaxModulos { get; set; } = 0;
      public int NumModulos { get; set; } = 0;
      public int NumCadenas { get; set; } = 0;
      public int NumInversores { get; set; } = 0;
      public double PotenciaPico { get; set; } = 0;
      public double PotenciaNominal { get; set; } = 0;
      public double PotenciaString { get; set; } = 0;
      public double TensionString { get; set; } = 0;

      //RELATIONS
      public int IdInversor { get; set; }
      public InversorDto Inversor { get; set; } = new();
      public int IdModulo { get; set; }
      public ModuloDto Modulo { get; set; } = new();

      //CONSTRUCTOR POR PARÁMETROS
      public CadenaDto() { }
   }
}
