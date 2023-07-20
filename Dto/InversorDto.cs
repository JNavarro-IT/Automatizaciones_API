
using backend_API.Utilities;

namespace backend_API.Dto
{
    public class InversorDto : DtoBase
    {
        public int IdInversor { get; set; }
        public string Fabricante { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public double PotenciaNominal { get; set; } = 0;
        public int VO { get; set; } = 0;
        public double IO { get; set; } = 0;
        public int Vmin { get; set; } = 0;
        public int Vmax { get; set; } = 0;
        public double CorrienteMaxString { get; set; } = 0;
        public int? VminMPPT { get; set; }
        public int? VmaxMPPT { get; set; } 
        public double? IntensidadMaxMPPT { get; set; }
        public double? Rendimiento { get; set; }
        public string Vatimetro { get; set; } = string.Empty;
        public InversorDto() { }
    }
}
