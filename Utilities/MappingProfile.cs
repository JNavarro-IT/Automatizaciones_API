using System.Reflection;
using AutoMapper;

namespace backend_API.Utilities
{
   // CLASE PARA CREAR UN PERFIL MAPPER PERSONALIZADO Y GENÉRICO
   public class MappingProfile : Profile

   {
      // CONSTRUCTOR POR DEFECTO
      public MappingProfile()
      {
         Type baseModelType = typeof(ModelBase);
         Type baseDtoType = typeof(DtoBase);
         var assembly = Assembly.GetExecutingAssembly();
         var entityTypes = assembly.GetTypes().Where(t => baseModelType.IsAssignableFrom(t) && !t.IsAbstract).ToList();
         var dtoTypes = assembly.GetTypes().Where(t => baseDtoType.IsAssignableFrom(t) && !t.IsAbstract).ToList();

         for (int i = 0; i < entityTypes.Count; i++)
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
