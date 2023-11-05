using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Automatizaciones_API.Utilities;


namespace Automatizaciones_API.Models
{
   [Table("Modulos")]
   public class Modulo : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdModulo { get; set; }

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Modelo { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Fabricante { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "float")]
      public double Potencia { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double Vmp { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double Imp { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double Isc { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double Vca { get; set; } = 0;

      [Column(TypeName = "float")]
      public double? Eficiencia { get; set; }

      [Column(TypeName = "varchar(25)")]
      public string? Dimensiones { get; set; }

      [Column(TypeName = "float")]
      public double? Peso { get; set; }

      [Column(TypeName = "int")]
      public int? NumCelulas { get; set; }

      [Column(TypeName = "varchar(15)")]
      public string? Tipo { get; set; }

      [Column(TypeName = "varchar(15)")]
      public string? TaTONC { get; set; }

      [Column(TypeName = "float")]
      public double? SalidaPotencia { get; set; }

      [Column(TypeName = "float")]
      public double? TensionVacio { get; set; }

      [Column(TypeName = "float")]
      public double? Tolerancia { get; set; }

      //CONSTRUCTOR POR DEFECTO
      public Modulo() { }
   }
}
