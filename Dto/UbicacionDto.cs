using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend_API.Dto
{
    public class UbicacionDto
    {
        [Required]
        public string Ref_catastral { get; set; }
        public string? Cups { get; set; }
        
        [Required]
        public double Superficie { get; set; }
        
        public string? CoordX_UTM { get; set; }
        public string? CoordY_UTM { get; set; }
        public string? CoordX { get; set; }
        public string? CoordY { get; set; }

        [Required]
        public string Latitud { get; set; }

        [Required]
        public string Longitud { get; set; }

        [Required]
        public int Inclinacion { get; set; }

        [Required]
        public int Azimut { get; set; }

        public UbicacionDto(string ref_catastral, string cups, double superficie, string? coordX_UTM, string? coordY_UTM, string? coordX, string? coordY, string latitud, string longitud, int inclinacion, int azimut)
        {
            Ref_catastral = ref_catastral;
            Cups = cups;
            Superficie = superficie;
            CoordX_UTM = coordX_UTM;
            CoordY_UTM = coordY_UTM;
            CoordX = coordX;
            CoordY = coordY;
            Latitud = latitud;
            Longitud = longitud;
            Inclinacion = inclinacion;
            Azimut = azimut;
        }
    }
}
