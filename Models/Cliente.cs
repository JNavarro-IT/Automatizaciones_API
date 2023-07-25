using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    //CLASE QUE REPRESENTA AL CLIENTE EN UN PROYECTO
    [Table("Clientes")]
    public class Cliente : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Dni { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Telefono { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string? Email { get; set; } = null;

        [Column(TypeName = "varchar(300)")]
        public string? Observaciones { get; set; } = string.Empty;

        //RELATIONS
        public List<Proyecto>? Proyectos { get; set; }
        public List<Ubicacion> Ubicaciones { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public Cliente() { }
    }
}