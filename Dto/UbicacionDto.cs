using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend_API.Dto
{
    public class UbicacionDto
    {
        [Required]
        public string Ref_catastral { get; set; }
        
        [Required]
        public double Superficie { get; set; }

        [Required]
        public double Latitud { get; set; }

        [Required]
        public double Longitud { get; set; }

        [Required]
        public int Inclinacion { get; set; }

        [Required]
        public int Azimut { get; set; }

        public UbicacionDto(string ref_catastral, double superficie, double latitud, double longitud, int inclinacion, int azimut)
        {
            Ref_catastral = ref_catastral;
            Superficie = superficie;
            Latitud = latitud;
            Longitud = longitud;
            Inclinacion = inclinacion;
            Azimut = azimut;
        }
    }
}
