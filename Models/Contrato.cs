using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Models
{
    [Table("Contratos")]
    public class Contrato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdContrato { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Referencia { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Fecha { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey("Ubicacion")]
        public int? IdUbicacion { get; set; }
        public Ubicacion? Ubicacion { get; set; }

        [ForeignKey("Instalacion")]
        public int? IdInstalacion { get; set; }
        public Instalacion? Instalacion { get; set; }


        public Contrato(string referencia, string fecha, int idCliente, int? idUbicacion, int? idInstalacion)
        {
            Referencia = referencia;
            Fecha = fecha;
            IdCliente = idCliente;
            IdUbicacion = idUbicacion;
            IdInstalacion = idInstalacion;
        }

    }
}
