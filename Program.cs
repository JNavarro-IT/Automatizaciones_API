using System.Reflection;
using Automatizaciones_API.Context;
using Automatizaciones_API.Controllers;
using Automatizaciones_API.Repository;
using Automatizaciones_API.Service;
using Automatizaciones_API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Automatizaciones_API
{

   // CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
   public class Program
   {
      public static void Main(string[] args)
      {
         var builder = WebApplication.CreateBuilder(args);

         // MARCADORES DUMMY PARA ENSAMBLADO DE TIPOS
         Type baseModelType = typeof(ModelBase);
         Type baseDtoType = typeof(DtoBase);

         var assembly = Assembly.GetExecutingAssembly();
         var entityTypes = assembly.GetTypes().Where(t => baseModelType.IsAssignableFrom(t) && !t.IsAbstract).ToList();
         var dtoTypes = assembly.GetTypes().Where(t => baseDtoType.IsAssignableFrom(t) && !t.IsAbstract).ToList();

         // CONFIGURACIÓN DE LAS CLASES REPOSITORY Y CONTROLLER MEDIANTE REFLEXIÓN
         for (int i = 0; i < entityTypes.Count; i++)
         {
            Type entityType = entityTypes[i];
            Type dtoType = dtoTypes[i];

            if (dtoType == null || entityType == null) continue;

            Type repositoryInterface = typeof(IBaseRepository<,>).MakeGenericType(entityType, dtoType);
            Type controllerInterface = typeof(IBaseController<,>).MakeGenericType(entityType, dtoType);
            Type repository = typeof(BaseRepository<,>).MakeGenericType(entityType, dtoType);
            Type controller = typeof(BaseController<,>).MakeGenericType(entityType, dtoType);

            builder.Services
               .AddScoped(repositoryInterface, repository)
               .AddScoped(controller);
         }

         // AUTOMAPPER PARA RELACIÓN MODELO-DTO Y SEED-DATA CON SQL
         var mapper = AutoMapperConfig.Initialize();

         builder.Services
            .AddSingleton(mapper)
            .AddTransient<IProjectService, ProjectService>()
            .AddTransient<IEXCELServices, EXCELService>()
            .AddTransient<IWORDService, WORDService>()
            .AddTransient<IPVGISService, PVGISServices>()
            .AddTransient<IPDFService, PDFService>()
            .AddControllers();

         builder.Services
            .AddEndpointsApiExplorer()
            .AddHttpClient();

         builder.Services.AddDbContext<DBContext>(options =>
         {
            if (builder.Environment.IsDevelopment())
               builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            else if (builder.Environment.IsStaging())
               builder.Configuration.AddJsonFile("appsettings.Staging.json", optional: true, reloadOnChange: true);

            else
               builder.Configuration.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);

            var connection = builder.Configuration.GetConnectionString("MasterConnection");
            options.UseSqlServer(connection, sql => sql.EnableRetryOnFailure());
         });

         builder.Services.AddCors(options =>
         {
            options.AddPolicy("Cors", app =>
            {
               app.WithOrigins(["*", "http://192.168.2.250:8087"])               
               .AllowAnyHeader()
               .WithMethods(["*", "OPTIONS"])
               .WithExposedHeaders(["*"]);
            });
         });

         var app = builder.Build();
         app.UseStaticFiles()
            .UseRouting() 
            .UseCors("Cors")            
            .UseEndpoints(endpoints => { endpoints.MapControllers(); });
         app.Run();
      }
   }
}