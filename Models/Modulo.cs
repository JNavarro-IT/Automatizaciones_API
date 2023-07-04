using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backend_API.Models
{
    [Table("Modulos")]
    public class Modulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string? Nombre { get; set; }

        [Column(TypeName = "float")]
        public double? Potencia { get; set; }

        [Column(TypeName = "float")]
        public double? Vmp { get; set; }

        [Column(TypeName = "float")]
        public double? Imp { get; set; }

        [Column(TypeName = "float")]
        public double? Icc { get; set; }

        [Column(TypeName = "float")]
        public double? Vca { get; set; }

        [Column(TypeName = "float")]
        public double? Eficiencia { get; set; }
        
        [Column(TypeName = "varchar(25)")]
        public string? Dimensiones { get; set; }

        [Column(TypeName = "float")]
        public double? Peso { get; set; }

        [Column(TypeName = "int")]
        public int? Num_Celulas { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? Tipo { get; set; }
       
        [Column(TypeName = "varchar(15)")]
        public string? Ta_TONC { get; set; }

        [Column(TypeName = "float")]
        public double? Salida_Potencia { get; set; }

        [Column(TypeName = "float")]
        public double? Tension_Vacio { get; set; }

        [Column(TypeName = "float")]
        public double? Tolerancia { get; set; }

        public Modulo(int id, string? nombre, double? potencia, double? vmp, double? imp, double? icc, double? vca, double? eficiencia, string? dimensiones, double? peso, int? num_Celulas,
                     string? tipo, string? ta_TONC, double? salida_Potencia, double? tension_Vacio, double? tolerancia)
        {
            Id = id;
            Nombre = nombre;
            Potencia = potencia;
            Vmp = vmp;
            Imp = imp;
            Icc = icc;
            Vca = vca;
            Eficiencia = eficiencia;
            Dimensiones = dimensiones;
            Peso = peso;
            Num_Celulas = num_Celulas;
            Tipo = tipo;
            Ta_TONC = ta_TONC;
            Salida_Potencia = salida_Potencia;
            Tension_Vacio = tension_Vacio;
            Tolerancia = tolerancia;
        }

    }
}
