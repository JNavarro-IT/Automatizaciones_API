using Automatizaciones_API.Controllers;
using Automatizaciones_API.Repository;
using System.Reflection;

namespace Automatizaciones_API.Configurations
{
   // INTERFAZ DE SERVICIO PARA CREAR Y REGISTRAR DINÁMICAMENTE Y AUTOMÁTICO MEDIANTE INYECCION DE DEPENDENCIAS
   public interface IAutomaticAPI
   {
      public void AssemblyModelDto(IServiceCollection services);
      public void AssemblyService(IServiceCollection services);
      public void ConfigureCors(IServiceCollection services);
   }

   // CLASE QUE AUTOMATIZA LOS REGISTRO DINÁMICO DE CLASES Y OBJETOS POR REFLEXIÓN
   public class Automationfig : IAutomaticAPI
   {
      private readonly Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
      private IServiceCollection _services;


      // CONSTRUCTOR POR DEFECTO
      public Automationfig(IServiceCollection services) { }

      // FILTRAR, ENSAMBLAR Y CREAR UN REGISTRO DINÁMICO DE SUS REPOSITORIOS Y CONTROLADORES MEDIANTE LA REFLEXIÓN
      public void AssemblyModelDto(IServiceCollection services)
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

            services.AddScoped(repositoryInterface, repository)
                     .AddScoped(controller);
         }
      }

      // FILTRAR, ENSAMBLAR Y  CREAR UN REGISTRO DINÁMICO DE 
      public void AssemblyService(IServiceCollection services)
      {
         var iServiceTypes = assembly.Where(iType => iType.IsInterface && iType.Name.EndsWith("Service")).ToList();

         for (var i = 0; i < iServiceTypes.Count; i++)
         {
            var iType = iServiceTypes[i];
            var impTypes = assembly.Where(impType =>
                       !impType.IsInterface && !impType.IsAbstract && iType.IsAssignableTo(impType)).ToList();

            if (impTypes.Count <= 0) continue;

            var impType = impTypes[i];

            if (impType == null || iType == null) continue;

            services.AddSingleton(iType, impType);
         }
      }

      public void ConfigureCors(IServiceCollection services)
      {
         services.AddCors(opt =>
         {
            opt.AddPolicy("Cors", policy =>
            {
               policy.WithOrigins(["http://localhost:6333"])
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
 