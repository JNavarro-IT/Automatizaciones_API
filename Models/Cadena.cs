using Automatizaciones_API.Configurations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automatizaciones_API.Models
{
   /*
   * CLASE QUE REPRESENTA UN CONJUNTO DE MODULOS IGUALES QUE SE 
   *   CONECTAN ENTRE ELLOS Y A UN ÚNICO INVERSOR
   */
   [Table("Cadenas")]
   public class Cadena : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdCadena { get; set; }

      [Required]
      [Column(TypeName = "int")]
      public int MinModulos { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int MaxModulos { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int NumModulos { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int NumCadenas { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int NumInversores { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double PotenciaPico { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double PotenciaNominal { get; set; } = 0;

      [Column(TypeName = "float")]
      public double PotenciaString { get; set; } = 0;

      [Column(TypeName = "float")]
      public double TensionString { get; set; } = 0;

      public int IdInversor { get; set; }
      public int IdModulo { get; set; }

      [ForeignKey("IdInstalacion")]
      public Instalacion? Instalacion { get; set; }

      //CONSTRUCTOR POR DEFECTO
      public Cadena() { }
   }
}
