
using backend_API.Utilities;

namespace backend_API.Dto
{
    //CLASE DTO QUE TRANSPORTA DATOS DE LA ENTIDAD CADENA
    public class CadenaDto : DtoBase
    {
        public int IdCadena { get; set; }
        public int MinModulos { get; set; } = 0;
        public int MaxModulos { get; set; } = 0;
        public int NumModulos { get; set; } = 0;
        public double PotenciaPico { get; set; } = 0;

        //RELATIONS
        public InversorDto Inversor { get; set; } = new();
        public ModuloDto Modulo { get; set; } = new();

        //CONSTRUCTOR POR PARÁMETROS
        public CadenaDto() { }
    }
}
