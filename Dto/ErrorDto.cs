using backend_API.Utilities;

namespace backend_API.Dto
{
   public class ErrorDto : DtoBase
   {
      public int IdError { get; set; }
      public string Mensaje { get; set; } = string.Empty;
      public string StackTrace { get; set; } = string.Empty;
      public DateTime FechaRegistro { get; set; } = DateTime.Now;
   }
}