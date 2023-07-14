using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    [Table("Proyectos")]
    public class Proyecto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProyecto { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Referencia { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Version { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Cups { get; set; }

        [Required]
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        public int IdUbicacion { get; set; }
        public Ubicacion Ubicacion { get; set; }

        public int? IdInstalacion { get; set; }
        public Instalacion? Instalacion { get; set; } = null;

        [Column(TypeName = "float")]
        public double? Presupuesto { get; set; }

        [Column(TypeName = "float")]
        public double? PresupuestoSyS { get; set; }

        [Column(TypeName = "date")]
        public DateTime PlazoEjecucion { get; set; }

        public Proyecto() { }

        public Proyecto(int idProyecto, string referencia, string version, DateTime fecha, string cups, int idCliente, int idUbicacion, int? idInstalacion, double presupuesto, double presupuestoSyS, DateTime plazoEjecucion)
        {
            IdProyecto = idProyecto;
            Referencia = referencia;
            Version = version;
            Fecha = fecha;
            Cups = cups;
            IdCliente = idCliente;
            IdUbicacion = idUbicacion;
            IdInstalacion = idInstalacion;
            Presupuesto = presupuesto;
            PresupuestoSyS = presupuestoSyS;
            PlazoEjecucion = plazoEjecucion;
        }
    }
}
