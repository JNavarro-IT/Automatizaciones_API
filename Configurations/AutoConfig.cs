using Automatizaciones_API.Controllers;
using Automatizaciones_API.Repository;
using System.Reflection;

namespace Automatizaciones_API.Configurations
{
   // INTERFAZ DE SERVICIO PARA CREAR Y REGISTRAR DINÁMICAMENTE Y AUTOMÁTICO MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IAutomaticAPI
   {
      public void AssemblyModelDto();
      public void AssemblyService();
      public void ConfigureCors();
   }

   // CLASE QUE AUTOMATIZA LOS REGISTRO DINÁMICO DE CLASES Y OBJETOS POR REFLEXIÓN
   public class AutoConfig(IServiceCollection services) : IAutomaticAPI
   {
      private readonly Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
      private readonly IServiceCollection _services = services;

      public IServiceCollection InitAutoConfig()
      {
         AssemblyModelDto();
         AssemblyService();
         ConfigureCors();
         return _services;
      }
      // FILTRAR, ENSAMBLAR Y CREAR UN REGISTRO DINÁMICO DE SUS REPOSITORIOS Y CONTROLADORES MEDIANTE LA REFLEXIÓN
      public void AssemblyModelDto()
      {
         var entityTypes = assembly.Where(tType => typeof(ModelBase).IsAssignableFrom(tType) && !tType.IsAbstract).ToList();
         var dtoTypes = assembly.Where(tDtoType => typeof(DtoBase).IsAssignableFrom(tDtoType) && !tDtoType.IsAbstract).ToList();

         for (var i = 0; i < entityTypes.Count; i++)
         {
            var entityType = entityTypes[i];
            var dtoType = dtoTypes[i];
            if (dtoType == null || entityType == null) continue;

            var repositoryInterface = typeof(IBaseRepository<,>).MakeGenericType(entityType, dtoType);
            var controllerInterface = typeof(IBaseController<,>).MakeGenericType(entityType, dtoType);
            var repository = typeof(BaseRepository<,>).MakeGenericType(entityType, dtoType);
            var controller = typeof(BaseController<,>).MakeGenericType(entityType, dtoType);

            _services.AddScoped(repositoryInterface, repository)
                     .AddScoped(controller);
         }
      }

      // FILTRAR, ENSAMBLAR Y  CREAR UN REGISTRO DINÁMICO DE 
      public void AssemblyService()
      {
         var iServiceTypes = assembly.Where(iType => iType.IsInterface && iType.Name.EndsWith("Service")).ToList();

         foreach (var iType in iServiceTypes)
         {

            var impTypes = assembly.Where(impType =>
                       !impType.IsInterface && !impType.IsAbstract && iType.IsAssignableFrom(impType)).ToList();

            if (impTypes.Count <= 0) continue;

            var impType = impTypes.FirstOrDefault(type => iType.IsAssignableFrom(type));

            if (impType == null || iType == null) continue;

            _services.AddScoped(iType, impType);
         }
      }

      // CONFIGURA LA POLÍTICA DE CORS
      public void ConfigureCors()
      {
         _services.AddCors(opt =>
         {
            opt.AddPolicy("Cors", policy =>
            {
               policy.WithOrigins(["http://localhost:8087", "http://192.168.2.250:8087"])
                     .SetIsOriginAllowed(_ => true)
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .WithExposedHeaders("*");
            });
         });
      }
   }

   // CLASES DUMMY (VACÍAS) PARA TIPAR Y ENSAMBLAR CLASES SIMILARES
   public class ModelBase { }
   public class DtoBase { }
}