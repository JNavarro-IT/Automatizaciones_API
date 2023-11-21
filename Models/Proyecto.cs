using Automatizaciones_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automatizaciones_API.Models
{
   [Table("Proyectos")]
   public class Proyecto : ModelBase
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int IdProyecto { get; set; }

      [Required]
      [Column(TypeName = "varchar(100)")]
      public string Referencia { get; set; } = string.Empty;

      [Required]
      [Column(TypeName = "float")]
      public double Version { get; set; } = 1.0;

      [Required]
      [Column(TypeName = "date")]
      public DateTime Fecha { get; set; } = new();

      [Column(TypeName = "float")]
      public double? Presupuesto { get; set; } = 0;

      [Column(TypeName = "float")]
      public double? PresupuestoSyS { get; set; } = 0;

      [Column(TypeName = "date")]
      public DateTime? PlazoEjecucion { get; set; }

      [Column(TypeName = "varchar(100)")]
      public string? OCA { get; set; } = string.Empty;

      [Column(TypeName = "varchar(100)")]
      public string? NumOCA { get; set; } = string.Empty;

      [Column(TypeName = "varchar(200)")]
      public string? InspeccionOCA { get; set; } = string.Empty;

      //RELATIONS
      [ForeignKey("IdCliente")]
      public Cliente Cliente { get; set; } = new();

      [ForeignKey("IdInstalacion")]
      public Instalacion Instalacion { get; set; } = new();

      public List<LugarPRL> LugaresPRL { get; set; } = new();

      // CONSTRUCTOR POR DEFECTO
      public Proyecto() { }
   }
}
