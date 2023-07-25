using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    //CLASE QUE REPRESENTA LA SUPERFICIE DONDE SE REALIZA LA INSTALACIÓN
    [Table("Cubiertas")]
    public class Cubierta : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCubierta { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string MedidasColectivas { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Accesibilidad { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Material { get; set; } = string.Empty;

        //RELATIONS
        public Ubicacion Ubicacion { get; set; }

        public Instalacion? Instalacion { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public Cubierta() { }
    }
}
