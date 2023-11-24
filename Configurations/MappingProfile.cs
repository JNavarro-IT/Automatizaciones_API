using AutoMapper;
using System.Reflection;

namespace Automatizaciones_API.Configurations
{
   // CLASE PARA CREAR UN PERFIL MAPPER PERSONALIZADO Y GENÉRICO
   public class MappingProfile : Profile

   {
      // CONSTRUCTOR POR DEFECTO
      public MappingProfile()
      {
         var assembly = Assembly.GetExecutingAssembly().GetTypes();
         var entityTypes = assembly.Where(t => typeof(ModelBase).IsAssignableFrom(t) && !t.IsAbstract).ToList();
         var dtoTypes = assembly.Where(t => typeof(DtoBase).IsAssignableFrom(t) && !t.IsAbstract).ToList();

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
