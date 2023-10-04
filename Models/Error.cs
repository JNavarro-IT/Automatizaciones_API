using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_API.Utilities;

namespace backend_API.Models
{
   [Table("Errores")]
   public class Error : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdError { get; set; }

      [Column(TypeName = "varchar(300)")]
      public string Mensaje { get; set; } = string.Empty;

      [Column(TypeName = "varchar(300)")]
      public string StackTrace { get; set; } = string.Empty;

      [Column(TypeName = "date")]
      public DateTime FechaRegistro { get; set; } = DateTime.Now;
   }
}