using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend_API.Dto
{
    public class InstalacionDto
    {
        [Required]
        public int Potencia_pico { get; set; }

        [Required]
        public int Potencia_nominal { get; set; }

        [Required]
        public string Tipo { get; set; }

        public string? CoordX_conexion { get; set; }
        public string? CoordY_conexion { get; set; }

        public InstalacionDto(int potencia_pico, int potencia_nominal, string tipo, string? coordX_conexion, string? coordY_conexion)
        {
            Potencia_pico = potencia_pico;
            Potencia_nominal = potencia_nominal;
            Tipo = tipo;
            CoordX_conexion = coordX_conexion;
            CoordY_conexion = coordY_conexion;
        }
    }
}
