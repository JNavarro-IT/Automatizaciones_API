using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    //CLASE QUE REPRESENTA LA INSTALACION DE UN PROYECTO
    [Table("Instalaciones")]
    public class Instalacion : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInstalacion { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Inclinacion { get; set; } = 0;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Azimut { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "float")]
        public double TotalPico { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        public double TotalNominal { get; set; } = 0;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "float")]
        public double CoordXConexion { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        public double CoordYConexion { get; set; } = 0;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Fusible { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string IDiferencial { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string IMagenetico { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Estructura { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(250)")]
        public string Vatimetro { get; set; } = string.Empty;

        //RELATIONS
        [ForeignKey("IdUbicacion")]
        public Ubicacion Ubicacion { get; set; }

        public Proyecto? Proyecto { get; set; }

        public List<Cubierta> Cubiertas { get; set; }

        public List<Cadena> Cadenas { get; set; }

        // CONSTRUCTOR POR DEFECTO
        public Instalacion() { }
    }
}