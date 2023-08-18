using backend_API.Controllers;
using backend_API.Models.Data;
using backend_API.Repository;
using backend_API.Service;
using backend_API.Utilities;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        //CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
        var builder = WebApplication.CreateBuilder(args);

        //MARCADORES DE CLASES DUMMY PARA ENSAMBLADO
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
     .AddScoped<IInstalacionService, InstalacionService>()
     .AddSingleton(mapper)
     .AddHttpClient()
     .AddEndpointsApiExplorer()
     .AddDbContext<DBContext>(options =>
     {
         var connection = builder.Configuration.GetConnectionString("ProdConnection");

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
        options.AddPolicy("Politica Acceso API", app =>
        {
            app.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyHeader();
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
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            options.UseCamelCasing(false);
        });

        var app = builder.Build();
        app.UseCors("Politica Acceso API");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}