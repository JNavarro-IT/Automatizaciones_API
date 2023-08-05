using backend_API.Utilities;
using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend_API.Models
{
    [Table("Modulos")]
    public class Modulo : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Name("IdModulo")]
        public int IdModulo { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [Name("Fabricante")]
        public string Fabricante { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        [Name("Modelo")]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "float")]
        [Name("Potencia")]
        public double Potencia { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        [Name("Vmp")]
        public double Vmp { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        [Name("Imp")]
        public double Imp { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        [Name("Isc")]
        public double Isc { get; set; } = 0;

        [Required]
        [Column(TypeName = "float")]
        [Name("Vca")]
        public double Vca { get; set; } = 0;

        [Column(TypeName = "float")]
        [Name("Eficiencia")]
        public double? Eficiencia { get; set; }

        [Column(TypeName = "varchar(25)")]
        [Name("Dimensiones")]
        public string? Dimensiones { get; set; }

        [Column(TypeName = "float")]
        [Name("Peso")]
        public double? Peso { get; set; }

        [Column(TypeName = "int")]
        [Name("NumCelulas")]
        public int? NumCelulas { get; set; }

        [Column(TypeName = "varchar(15)")]
        [Name("Tipo")]
        public string? Tipo { get; set; }

        [Column(TypeName = "varchar(15)")]
        [Name("TaTONC")]
        public string? TaTONC { get; set; }

        [Column(TypeName = "float")]
        [Name("SalidaPotencia")]
        public double? SalidaPotencia { get; set; }

        [Column(TypeName = "float")]
        [Name("TensionVacio")]
        public double? TensionVacio { get; set; }

        [Column(TypeName = "float")]
        [Name("Tolerancia")]
        public double? Tolerancia { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public Modulo() { }
    }
}
