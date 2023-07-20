using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
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

        //RELATIONS
        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }

        public List<Cubierta> Cubiertas { get; set; }

        public List<Inversor> Inversores { get; set; } = new();
       
        public List<Cadena> Cadenas { get; set; } = new();

        public Instalacion() { }
    }
}