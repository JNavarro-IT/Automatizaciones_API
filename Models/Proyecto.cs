using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
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

        [Column(TypeName = "varchar(100)")]
        public string? Cups { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Justificacion { get; set; } = null;

        [Column(TypeName = "float")]
        public double? Presupuesto { get; set; }

        [Column(TypeName = "float")]
        public double? PresupuestoSyS { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PlazoEjecucion { get; set; }

        //RELATIONS
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("IdInstalacion")]
        public Instalacion Instalacion { get; set; }

        [ForeignKey("IdUbicacion")]
        public Ubicacion Ubicacion { get; set; }

        public List<LugarPRL> LugaresPRL { get; set; }

        // CONSTRUCTOR POR DEFECTO
        public Proyecto() { }
    }
}
