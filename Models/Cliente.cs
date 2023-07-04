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
        [Index(IsUnique = true)]
        [Column(TypeName = "varchar(15)")]
        public string Dni { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Direccion { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string Cp { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Provincia { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Municipio { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? Telefono { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public string Url { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Observaciones { get; set; }

        public Contrato Contrato { get; set; }

        public Cliente(string nombre, string dni, string direccion, string cp, string provincia, string municipio, string? telefono, string url, string? observaciones)
        {
            Nombre = nombre;
            Dni = dni;
            Direccion = direccion;
            Cp = cp;
            Provincia = provincia;
            Municipio = municipio;
            Telefono = telefono;
            Url = url;
            Observaciones = observaciones;
        }
    }
}
