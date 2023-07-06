using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    [Table("Ubicaciones")]
    public class Ubicacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUbicacion { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Ref_catastral { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Cups { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Superficie { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CoordX_UTM { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CoordY_UTM { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CoordX { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CoordY { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Latitud { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Longitud { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Inclinacion { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Azimut { get; set; }

        public Proyecto Contrato { get; set; }

        public Ubicacion(string ref_catastral, string? cups, double superficie, string? coordX_UTM, string? coordY_UTM, string? coordX, string? coordY, string latitud, string longitud, int inclinacion, int azimut)
        {
            Ref_catastral = ref_catastral;
            Cups = cups;
            Superficie = superficie;
            CoordX_UTM = coordX_UTM;
            CoordY_UTM = coordY_UTM;
            CoordX = coordX;
            CoordY = coordY;
            Latitud = latitud;
            Longitud = longitud;
            Inclinacion = inclinacion;
            Azimut = azimut;
        }
    }
}
