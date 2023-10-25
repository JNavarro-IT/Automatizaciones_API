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
         Assembly assembly = Assembly.GetExecutingAssembly();
         List<Type> entityTypes = assembly.GetTypes().Where(t => baseModelType.IsAssignableFrom(t) && !t.IsAbstract).ToList();
         List<Type> dtoTypes = assembly.GetTypes().Where(t => baseDtoType.IsAssignableFrom(t) && !t.IsAbstract).ToList();

         for (int i = 0; i < entityTypes.Count; i++)
         {
            _ = CreateMap(entityTypes[i], dtoTypes[i]).ReverseMap();
         }
      }
   }

   public static class AutoMapperConfig
   {
      public static IMapper Initialize()
      {
         MapperConfiguration configuration = new(cfg =>
         {
            cfg.AddProfile<MappingProfile>();
         });
         return configuration.CreateMapper();
      }
   }
}

namespace backend_API.Utilities
{
   public class ModelBase { }

   public class DtoBase { }
}
