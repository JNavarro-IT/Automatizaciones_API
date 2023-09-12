using System.Reflection;
using backend_API.Controllers;
using backend_API.Models.Context;
using backend_API.Repository;
using backend_API.Service;
using backend_API.Utilities;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

internal class Program
{
   private static void Main(string[] args)
   {
      //CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
      var builder = WebApplication.CreateBuilder(args);

      //MARCADORES DUMMYV PARA ENSAMBLADO DE TIPOS
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

         if (dtoType == null || entityType == null)
         {
            Console.Error.WriteLine("ERROR EN LA ENTIDAD O DTO");
            continue;
         }

         Type repositoryInterface = typeof(IBaseRepository<,>).MakeGenericType(entityType, dtoType);
         Type repository = typeof(BaseRepository<,>).MakeGenericType(entityType, dtoType);
         Type controllerInterface = typeof(IBaseController<,>).MakeGenericType(entityType, dtoType);
         Type controller = typeof(BaseController<,>).MakeGenericType(entityType, dtoType);

         builder.Services
             .AddScoped(repositoryInterface, repository)
             .AddScoped(controller);
      }

      // AUTOMAPPER PARA RELACION MODELO-DTO Y SEED-DATA CON .CSV
      var mapper = AutoMapperConfig.Initialize();

      builder.Services
      .AddTransient<IInstalacionService, InstalacionService>()
      .AddTransient<IExcelServices, EXCELService>()
      .AddTransient<IWORDService, WORDService>()
      .AddTransient<IPVGISService, PVGISService>()
      .AddTransient<IPDFService, PDFService>()
      .AddSingleton(mapper)
      .AddHttpClient()
      .AddEndpointsApiExplorer()

      .AddDbContext<DBContext>(options =>
      {
         var connection = "";
         if (builder.Environment.IsDevelopment())
            connection = builder.Configuration.GetConnectionString("DevConnection");
         else if(builder.Environment.IsStaging())
            connection = builder.Configuration.GetConnectionString("DevConnection");
         else
            connection = builder.Configuration.GetConnectionString("MasterConnection");

         options.UseSqlServer(connection, sqlOptions => sqlOptions.EnableRetryOnFailure());
         /*
         var scvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
         {
               BadDataFound = null          
         };

         using var dbContext = new DBContext((DbContextOptions<DBContext>)options.Options);
         using var reader = new StreamReader("SeedData/aa.csv");
         using var csv = new CsvReader(reader, scvConfig);
         csv.Read();

         var modulos = csv.GetRecords<Modulo>();
         mapper.Map<Modulo>(modulos.ToList());
         dbContext.Modulos.AddRange(modulos);
         dbContext.SaveChanges();*/
      })

     .AddCors(options =>
     {
        options.AddPolicy("Acceso API", app =>
          {
             app.AllowAnyOrigin()
            .AllowAnyMethod()
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .DisallowCredentials()
            .Build(); ;
          });
     })

     .Configure<CookiePolicyOptions>(options =>
     {
        options.MinimumSameSitePolicy = SameSiteMode.None;
        options.HttpOnly = HttpOnlyPolicy.None;
        options.Secure = CookieSecurePolicy.None;
     })

     .AddControllers()
     .AddControllersAsServices()
     .AddNewtonsoftJson(options =>
     {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
        options.UseCamelCasing(false);
     });

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
         app.UseDeveloperExceptionPage();
         app.UseStatusCodePages();
         app.UseCors("Acceso API");

         Console.WriteLine("Utilizando perfil de Desarrollo.");
         builder.Configuration
                  .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
      } 
      else if(app.Environment.IsStaging())
      {
         Console.WriteLine("Utilizando perfil de Producción.");
         builder.Configuration
             .AddJsonFile("appsettings.Staging.json", optional: false, reloadOnChange: true);
         app.UseDefaultFiles();
         app.UseStaticFiles();
      }
      else
      {
         Console.WriteLine("Utilizando perfil de Producción.");
         builder.Configuration
             .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
     
      }

      app.UseHttpsRedirection()
      .UseRouting()
      .UseEndpoints(endpoints =>
      {
         endpoints.MapControllers();
      })
      .UseCookiePolicy();
      app.Run();
   }
}