using Automatizaciones_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automatizaciones_API.Models
{
   // CLASE QUE REPRESENTA LA UBICACION DEL PROYECTO
   [Table("Ubicaciones")]
   public class Ubicacion : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdUbicacion { get; set; }

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string RefCatastral { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(20)")]
      public string Antiguedad { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(50)")]
      public string Via { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(250)")]
      public string Calle { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Numero { get; set; } = string.Empty;
      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Km { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(15)")]
      public string Bloque { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Portal { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Escalera { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Piso { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(10)")]
      public string Puerta { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "int")]
      public int Cp { get; set; } = 0;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Municipio { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Provincia { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string CCAA { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "float")]
      public double Superficie { get; set; } = 0;

      [Column(TypeName = "float")]
      public double? CoordXUTM { get; set; }

      [Column(TypeName = "float")]
      public double? CoordYUTM { get; set; }

      [Required]
      [Column(TypeName = "float")]
      public double Latitud { get; set; } = 0;

      [Required]
      [Column(TypeName = "float")]
      public double Longitud { get; set; } = 0;

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Cups { get; set; } = string.Empty;

      [Column(TypeName = "varchar(100)")]
      public string Empresa { get; set; } = string.Empty;

      [Column(TypeName = "varchar(50)")]
      public string Cif { get; set; } = string.Empty;

      [Column(TypeName = "varchar(50)")]
      public string Cau { get; set; } = string.Empty;


      // RELATIONS
      [ForeignKey("IdCliente")]
      public Cliente Cliente { get; set; } = new();

      // CONSTRUCTOR POR DEFECTO
      public Ubicacion() { }
   }
}