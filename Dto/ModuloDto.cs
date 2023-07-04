
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_API.Dto
{
    public class ModuloDto
    {

        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }
        public double? Potencia { get; set; }
        public double? Vmp { get; set; }
        public double? Imp { get; set; }
        public double? Icc { get; set; }
        public double? Vca { get; set; }
        public double? Eficiencia { get; set; }
        public string? Dimensiones { get; set; }
        public double? Peso { get; set; }
        public int? Num_Celulas { get; set; }
        public string? Tipo { get; set; }
        public string? Ta_TONC { get; set; }
        public double? Salida_Potencia { get; set; }
        public double? Tension_Vacio { get; set; }
        public double? Tolerancia { get; set; }

        public ModuloDto(string? nombre, double? potencia, double? icc)
        {
            Nombre = nombre;
            Potencia = potencia;
            Icc = icc;
        }

    }



}

