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
        public double Inclinacion { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Azimut { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Potencia_pico { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Potencia_nominal { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Tipo { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Coordenadas_conexion { get; set; }

        [ForeignKey("IdModulo")]
        public Modulo Modulo { get; set; }

        [ForeignKey("IdInversor")]
        public Inversor Inversor { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Estructura { get; set; }

        public string Justificacion { get; set; }

        [ForeignKey("IdCubierta")]
        public Cubierta Cubierta { get; set; }

        public Proyecto Contrato { get; set; }

        public Instalacion(double inclinacion, string azimut, string tipo, string? coordenadas_conexion, int idModulo, int idInversor, string estructura, string justificacion, int idCubierta)
        {
            Inclinacion = inclinacion;
            Azimut = azimut;
            Potencia_pico =
            Potencia_nominal = Inversor?.Potencia ?? 0;
            Tipo = tipo;
            Coordenadas_conexion = coordenadas_conexion;
            Estructura = estructura;
            Justificacion = justificacion;
        }

        private double CalcularPotenciaPico()
        {
            return Modulo?.Potencia * Inversor?.Potencia ?? 0;
        }
    }
}
