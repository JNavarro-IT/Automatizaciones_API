using System.Reflection;
using backend_API.Context;
using backend_API.Controllers;
using backend_API.Repository;
using backend_API.Service;
using backend_API.Utilities;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace backend_API
{

   // CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
   public class Program
   {
      public static void Main(string[] args)
      {
         var builder = WebApplication.CreateBuilder(args);
         builder.Logging
            .AddConsole()
            .AddDebug()
            .AddJsonConsole();

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
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
               options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
               options.UseCamelCasing(false);
            });

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
               app.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin()
               .WithMethods(["*", "OPTIONS"])
               .WithExposedHeaders("*");
            });
         });

         var app = builder.Build();
         app.UseStaticFiles() 
            .UseDeveloperExceptionPage()
            .UseRewriter(new RewriteOptions()
            .AddRedirect("^/?$", "/")
            .AddRedirect("/", "http://192.168.2.250:8087/"))
            .UseRouting()
            .UseCors("Cors")
            .UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.Run();
      }
   }
}