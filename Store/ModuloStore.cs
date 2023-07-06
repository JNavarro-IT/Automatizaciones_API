using backend_API.Dto;

namespace backend_API.Stores
{
    public static class ModuloStore

    {
        
        public static List<ModuloDto> modulosList = new List<ModuloDto>()
        {
                new ModuloDto(nombre: "JAM66S30 500MR", potencia:500, icc:13.04),
                new ModuloDto(nombre: "PS460M5GFH-24/TSH", potencia:460, icc:10.94)

        };



    }

    
}