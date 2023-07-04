using backend_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend_API.Dto
{
    public class ContratoDto
    {
        [Required]
        public string Referencia { get; set; }

        [Required]
        public string Fecha { get; set; }

        [Required]
        public ClienteDto Cliente { get; set; }

        public UbicacionDto? Ubicacion { get; set; }

        public InstalacionDto? Instalacion { get; set; }

        public ContratoDto(string referencia, string fecha, ClienteDto cliente, UbicacionDto? ubicacion, InstalacionDto? instalacion)
        {
            Referencia = referencia;
            Fecha = fecha;
            Cliente = cliente;
            Ubicacion = ubicacion;
            Instalacion = instalacion;
        }
    }
}
