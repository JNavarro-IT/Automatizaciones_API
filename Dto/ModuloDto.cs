using backend_API.Utilities;

namespace backend_API.Dto
{
    //CLASE DTO QUE TRANSPORTA LOS DATOS DE LA ENTIDAD MÓDULO
    public class ModuloDto : DtoBase
    {
        public int IdModulo { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public double Potencia { get; set; } = 0;
        public double Vmp { get; set; } = 0;
        public double Imp { get; set; } = 0;
        public double Isc { get; set; } = 0;
        public double Vca { get; set; } = 0;
        public double? Eficiencia { get; set; }
        public string? Dimensiones { get; set; }
        public double? Peso { get; set; }
        public int? NumCelulas { get; set; }
        public string? Tipo { get; set; } = string.Empty;
        public string? TaTONC { get; set; }
        public double? SalidaPotencia { get; set; }
        public double? TensionVacio { get; set; }
        public double? Tolerancia { get; set; }

        //CONSTRUCTOR POR DEFECTO
        public ModuloDto() { }
    }
}

