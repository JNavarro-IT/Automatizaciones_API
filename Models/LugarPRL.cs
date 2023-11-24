using Automatizaciones_API.Configurations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automatizaciones_API.Models
{
   [Table("Lugares")]
   public class LugarPRL : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdLugarPRL { get; set; }

      [Required]
      public string Nombre { get; set; } = string.Empty;

      [Required]
      public string Tipo { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Calle { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Numero { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "int")]
      public int Cp { get; set; } = 0;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Municipio { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(50)")]
      public string Provincia { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(30)")]
      public string Telefono { get; set; } = string.Empty;

      [Column(TypeName = "varchar(100)")]
      public string Email { get; set; } = string.Empty;

      [Column(TypeName = "varchar(100)")]
      public string? NIMA { get; set; } = string.Empty;

      [Column(TypeName = "varchar(100)")]
      public string? Autorizacion { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "float")]
      public double Latitud { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double Longitud { get; set; } = 0;

      [Column(TypeName = "varchar(100)")]
      public string? RutaImg { get; set; } = string.Empty;

      //RELATION
      public List<Proyecto>? Proyectos { get; set; }

      //CONSTRUCTOR POR DEFECTO
      public LugarPRL() { }
   }
}