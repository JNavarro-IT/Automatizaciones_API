using System.Reflection;
using Microsoft.EntityFrameworkCore;
using backend_API.Controllers;
using backend_API.Models.Context;
using backend_API.Repository;
using backend_API.Service;
using backend_API.Utilities;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DocumentFormat.OpenXml.Vml.Office;
using ClosedXML;
using System.Net.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Identity.Client;

internal class Program
{
   private static void Main(string[] args)
   {
      // CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
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
         .AddHttpClient();
     
      builder.Services.AddDbContext<DBContext>(options =>
      {
         var connection = "";
         if (builder.Environment.IsDevelopment())
         {
            connection = builder.Configuration.GetConnectionString("DevConnection"); 
            builder.Services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true);
         }
         else if (builder.Environment.IsStaging())
         {
            connection = builder.Configuration.GetConnectionString("DevConnection");
            builder.Services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true);
         }
         else
         {
            connection = builder.Configuration.GetConnectionString("MasterConnection");
            builder.Services.Configure<IISServerOptions>(opt =>
            {
               opt.AllowSynchronousIO = true;
               opt.MaxRequestBodyBufferSize = 10024;
               opt.MaxRequestBodySize = 10024;
            });
         }
      
         options.UseSqlServer(connection, sqlOptions => sqlOptions.EnableRetryOnFailure());
      });
      
      builder.Services.AddControllers();
      builder.Services.AddEndpointsApiExplorer();

      var app = builder.Build();
      if (app.Environment.IsDevelopment())
      {
         Console.WriteLine("Utilizando perfil de Desarrollo...");
         builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
         app.UseStatusCodePages().UseDeveloperExceptionPage();
    
      }
      else if (app.Environment.IsStaging())
      {
         Console.WriteLine("Utilizando perfil de Staging...");
         builder.Configuration.AddJsonFile("appsettings.Staging.json", optional: false, reloadOnChange: true);
       
      }
      else
      {
         Console.WriteLine("Utilizando perfil de Producción...");
         builder.Configuration.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
        
      }
      
      app.Use((context, next) =>
      {
         if (context.Request.Method == "OPTIONS")
         { 
            context.Request.Headers.Clear(); 
            context.Request.Headers.TryGetValue("Origin", out var origins);
            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
            context.Response.Headers.Add("Access-Control-Allow-Origin", origins);
            return Task.CompletedTask;
         }

         return next(context);
      });
      

      app.UseStaticFiles()
         .UseCors(opt =>
         {
            opt.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod()
               .WithExposedHeaders("*");
         })
         .UseRouting()
         .UseEndpoints(endpoints => endpoints.MapControllers());
      app.Run();
   }
}
