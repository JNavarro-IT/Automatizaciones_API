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
        [Column(TypeName = "varchar(20)")]
        public string Fecha { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Cups { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [ForeignKey("Ubicacion")]
        public int IdUbicacion { get; set; }
        public Ubicacion Ubicacion { get; set; }

        [ForeignKey("Instalacion")]
        public int? IdInstalacion { get; set; }
        public Instalacion? Instalacion { get; set; }


        public Proyecto(string referencia, string version, string fecha, string cups, int idCliente, int idUbicacion, int? idInstalacion)
        {
            Referencia = referencia;
            Version = version;
            Fecha = fecha;
            Cups = cups;
            IdCliente = idCliente;
            IdUbicacion = idUbicacion;
            IdInstalacion = idInstalacion;
        }

    }
}
