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
        public int IdInversor { get; set; } 
        public Inversor Inversor { get; set; }

        public int IdModulo { get; set; }
        public Modulo Modulo { get; set; }

        public int IdInstalacion { get; set; }   
        public Instalacion? Instalacion { get; set;}

        //CONSTRUCTOR POR DEFECTO
        public Cadena() { }
    }
}
