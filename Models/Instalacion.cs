using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    [Table("Instalaciones")]
    public class Instalacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInstalacion { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public int Potencia_pico { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Potencia_nominal { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Tipo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? CoordX_conexion { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? CoordY_conexion { get; set; }

        public Proyecto Contrato { get; set; }

        public Instalacion(int potencia_pico, int potencia_nominal, string tipo, string? coordX_conexion, string? coordY_conexion)
        {
            Potencia_pico = potencia_pico;
            Potencia_nominal = potencia_nominal;
            Tipo = tipo;
            CoordX_conexion = coordX_conexion;
            CoordY_conexion = coordY_conexion;
        }
    }
}
