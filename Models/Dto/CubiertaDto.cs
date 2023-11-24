using Automatizaciones_API.Configurations;

namespace Automatizaciones_API.Models.Dto
{
   //CLASE DTO QUE TRANSPORTA DATOS DE LA ENTIDAD CUBIERTA
   public class CubiertaDto : DtoBase
   {
      public int? IdCubierta { get; set; }
      public string MedidasColectivas { get; set; } = string.Empty;
      public string Accesibilidad { get; set; } = string.Empty;
      public string Material { get; set; } = string.Empty;

      //RELATIONS
      public int? IdInstalacion { get; set; }

      //CONSTRUCTOR POR DEFECTO
      public CubiertaDto() { }
   }
}
