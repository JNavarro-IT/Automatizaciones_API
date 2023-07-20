using AutoMapper;
using backend_API.Dto;
using backend_API.Models;

namespace backend_API.Utilities
{
    // CLASE PARA CREAR UN PERFIL MAPPER PERSONALIZADO Y GENÉRICO
    public class MappingProfile : Profile
  
    {
        // CONSTRUCTOR POR DEFECTO
        public MappingProfile()
        {
            var entityTypes = new List<Type> { typeof(Cadena), typeof(Cliente), typeof(Cubierta), typeof(Instalacion), typeof(Inversor), typeof(Modulo), typeof(Proyecto), typeof(Ubicacion) };
            var dtoTypes = new List<Type> { typeof(CadenaDto), typeof(ClienteDto), typeof(CubiertaDto), typeof(InstalacionDto), typeof(InversorDto), typeof(ModuloDto), typeof(ProyectoDto), typeof(UbicacionDto) };

            for(int i = 0; i < entityTypes.Count; i++)
                CreateMap(entityTypes[i], dtoTypes[i]).ReverseMap();
        }
    }

    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                
                    cfg.AddProfile<MappingProfile>();
                
            });
            return configuration.CreateMapper();
        }
    }
}
