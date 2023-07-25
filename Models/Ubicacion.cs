using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
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
        [Column(TypeName = "varchar(max)")]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "int")]
        public int CP { get; set; } = 0;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Municipio { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Provincia { get; set; } = string.Empty;

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

        //RELATIONS
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        public Instalacion? Instalacion { get; set; }

        public Proyecto? Proyecto { get; set; }

        public List<Cubierta> Cubiertas { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public Ubicacion() { }
    }
}