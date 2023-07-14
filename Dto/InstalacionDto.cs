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

        public string? Coordenadas_conexion { get; set; }

   
    }
}
