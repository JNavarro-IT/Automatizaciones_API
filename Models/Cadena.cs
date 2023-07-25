using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    /*
    * CLASE QUE REPRESENTA UN CONJUNTO DE MODULOS IGUALES QUE SE 
    *   CONECTAN ENTRE ELLOS Y A UN ÚNICO INVERSOR
    */
    [Table("Cadenas")]
    public class Cadena : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCadena { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int MinModulos { get; set; } = 0;

        [Required]
        [Column(TypeName = "int")]
        public int MaxModulos { get; set; } = 0;

        [Required]
        [Column(TypeName = "int")]
        public int NumModulos { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        public double PotenciaPico { get; set; } = 0;

        //RELATIONS
        [ForeignKey("IdInversor")]
        public Inversor Inversor { get; set; }

        [ForeignKey("IdModulo")]
        public Modulo Modulo { get; set; }

        [ForeignKey("IdInstalacion")]
        public Instalacion? Instalacion { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public Cadena() { }
    }
}
