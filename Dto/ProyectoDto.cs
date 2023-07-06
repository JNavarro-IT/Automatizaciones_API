using System.ComponentModel.DataAnnotations;

namespace backend_API.Dto
{
    public class ProyectoDto
    {
        [Required]
        public string Referencia { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public string Fecha { get; set; }

        public string Cups { get; set; }

        [Required]
        public ClienteDto Cliente { get; set; }

        [Required]
        public UbicacionDto Ubicacion { get; set; }

        public InstalacionDto? Instalacion { get; set; }

        public ProyectoDto(string referencia, string version, string fecha, string? cups, ClienteDto cliente, UbicacionDto ubicacion, InstalacionDto? instalacion)
        {
            Referencia = referencia;
            Version = version;
            Fecha = fecha;
            Cups = cups;
            Cliente = cliente;
            Ubicacion = ubicacion;
            Instalacion = instalacion;
        }
    }
}
