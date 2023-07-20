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
        [Column(TypeName = "varchar(50)")]
        public string Version { get; set; } = string.Empty;

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
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } = new();

        public Instalacion? Instalacion { get; set; } = new();

        public List<Lugar>? Lugares { get; set; } = new();

        public Proyecto() { }
    }
}
