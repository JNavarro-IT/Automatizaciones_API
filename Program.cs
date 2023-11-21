using Automatizaciones_API.Context;
using Automatizaciones_API.Controllers;
using Automatizaciones_API.Repository;
using Automatizaciones_API.Service;
using Automatizaciones_API.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

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
            .AddHttpClient()
            .AddRouting()
            .AddEndpointsApiExplorer()
            .AddControllers();

         var development = false;
         builder.Services.AddDbContext<DBContext>(options =>
         {
            if (builder.Environment.IsDevelopment())
            {
               builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
               development = true;
            }

            else if (builder.Environment.IsStaging())
               builder.Configuration.AddJsonFile("appsettings.Staging.json", optional: true, reloadOnChange: false);


            else
               builder.Configuration.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);

            var connection = builder.Configuration.GetConnectionString("MasterConnection");
            options.UseSqlServer(connection, sql => sql.EnableRetryOnFailure());
         });



         // CONFIG. SOLO TESTEO EN LOCAL
         var httpPort = 4046;
         var httpsPort = 443;
         builder.WebHost
           .PreferHostingUrls(true)
           .UseContentRoot(Directory.GetCurrentDirectory());

         if (development == true)
         {
            builder.WebHost
           .UseKestrel(opt =>
           {
              opt.Limits.MaxRequestBodySize = null;
              opt.AllowSynchronousIO = true;


              opt.ListenLocalhost(httpPort);
              opt.Configure().LocalhostEndpoint(httpPort);
           })
            .UseKestrelCore();
         }

            builder.WebHost.UseIIS().UseIISIntegration();

            builder.Services.AddCors(options =>
            {
               options.AddPolicy("Cors", police =>
               {
                  police.WithOrigins(["*", "192.168.2.250:8087"])
                     .SetIsOriginAllowedToAllowWildcardSubdomains()
                     .WithHeaders("*")
                     .WithMethods(["*", "OPTIONS"])
                     .WithExposedHeaders(["*", "Content-Disposition"]);
               });
            });

            var app = builder.Build();
            app.UseDefaultFiles()
               .UseRouting()
               .UseCors("Cors")
               .UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.Run();
         }
      }
   }