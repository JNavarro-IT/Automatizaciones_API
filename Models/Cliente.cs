using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Dni { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Telefono { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string? Observaciones { get; set; }

        public List<Ubicacion> Ubicaciones { get; set; }
        public List<Proyecto> Proyectos { get; set; }

        public Cliente(string nombre, string dni, string? telefono, string? email, string? observaciones)
        {
            Nombre = nombre;
            Dni = dni;
            Telefono = telefono;
            Email = email;
            Observaciones = observaciones;
        }
    }
}
