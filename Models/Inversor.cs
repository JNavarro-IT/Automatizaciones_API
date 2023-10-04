using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_API.Utilities;

namespace backend_API.Models
{
   // CLASE QUE REPRESENTA UN INVERSOR EN UNA INSTALACIÓN
   [Table("Inversores")]
   public class Inversor : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdInversor { get; set; }

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Modelo { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Fabricante { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "float")]
      public double PotenciaNominal { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int VO { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double IO { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int Vmin { get; set; } = 0;

      [Required]
      [Column(TypeName = "int")]
      public int Vmax { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double CorrienteMaxString { get; set; } = 0;

      [Column(TypeName = "int")]
      public int? VminMPPT { get; set; } = 0;

      [Column(TypeName = "int")]
      public int? VmaxMPPT { get; set; } = 0;

      [Column(TypeName = "float")]
      public double? IntensidadMaxMPPT { get; set; } = 0;

      [Column(TypeName = "float")]
      public double? Rendimiento { get; set; } = 0;




      //CONSTRUCTOR POR DEFECTO
      public Inversor() { }
   }
}
