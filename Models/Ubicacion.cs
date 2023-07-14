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

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Direccion { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Superficie { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Coordenadas_UTM { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")] 
        public string LatLng { get; set; }

        public Proyecto Proyecto { get; set; }

        public Ubicacion()
        {
        }

        public Ubicacion(string ref_catastral, string direccion, double superficie, string coordenadas_UTM, string latLng)
        {
            Ref_catastral = ref_catastral;
            Direccion = direccion;
            Superficie = superficie;
            Coordenadas_UTM = coordenadas_UTM;
            LatLng = latLng;
        }
    }
}
