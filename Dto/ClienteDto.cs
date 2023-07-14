using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using backend_API.Models;

namespace backend_API.Dto
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Dni { get; set; }

        [Required]
        public string Telefono { get; set; }
        public string? Email { get; set; }

        public string? Observaciones { get; set; }

        public int IdContrato { get; set; }
        public int IdProyecto { get; set; }

        public ClienteDto(string nombre, string dni, string? telefono, string? email, string? observaciones)
        {
            Nombre = nombre;
            Dni = dni;
            Telefono = telefono;
            Email = email;
            Observaciones = observaciones;
        }
    }
}
