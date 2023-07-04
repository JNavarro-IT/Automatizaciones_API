using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend_API.Dto
{
    public class ClienteDto
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Dni { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Cp { get; set; }

        [Required]
        public string Provincia { get; set; }

        [Required]
        public string Municipio { get; set; }

        public string? Telefono { get; set; }

        [Required]
        public string Url { get; set; }

        public string? Observaciones { get; set; }



        public ClienteDto(string nombre, string dni, string direccion, string cp, string provincia, string municipio, string? telefono, string url, string? observaciones)
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
