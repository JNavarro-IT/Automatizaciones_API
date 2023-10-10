using System.Reflection;
using backend_API.Controllers;
using backend_API.Models.Context;
using backend_API.Repository;
using backend_API.Service;
using backend_API.Utilities;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

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
         .AddTransient<IPVGISService, PVGISService>()
         .AddTransient<IPDFService, PDFService>()
         .AddControllers()
         .AddNewtonsoftJson(options =>
         {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            options.UseCamelCasing(false);
         });

      builder.Services
         .AddHttpClient()
         .AddEndpointsApiExplorer();

      builder.Services.AddCors(opt => 
         opt.AddPolicy("Cors", police => 
            police.WithOrigins("*")
                  .WithHeaders("*")
                  .WithMethods("*")
                  .WithExposedHeaders("*"))
      );

          builder.Services.AddDbContext<DBContext>(options =>
          {
            var connection = "";
            if (builder.Environment.IsDevelopment())
            {
               Console.WriteLine("Utilizando perfil de Desarrollo...");
               builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
               connection = builder.Configuration.GetConnectionString("DevConnection");
            }
            else 
            {
               Console.WriteLine("Utilizando perfil de Producción...");
               builder.Configuration.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
               connection = builder.Configuration.GetConnectionString("MasterConnection");
            }
            options.UseSqlServer(connection, sql => sql.EnableRetryOnFailure());
         });   

      var app = builder.Build();    
      app.UseCors("Cors")
         .UseRouting()         
         .UseEndpoints(endpoints => endpoints.MapControllers());
      app.Run();
   }
} 