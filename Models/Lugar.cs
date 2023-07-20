using backend_API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    [Table("Lugares")]
    public class Lugar : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLugar { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
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
        [Column(TypeName = "varchar(30)")]
        public string Telefono { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string? NIMA { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string? Autorizacion { get; set; } = string.Empty;

        //RELATION
        public List<Proyecto>? Proyectos { get; set; }

        public Lugar() { }
    }
}
