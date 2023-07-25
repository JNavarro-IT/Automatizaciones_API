using backend_API.Controllers;
using backend_API.Models.Data;
using backend_API.Repository;
using backend_API.Service;
using backend_API.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

//CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
var builder = WebApplication.CreateBuilder(args);

//MARCADORES DE CLASES DUMMY PARA ENSAMBLADO
var baseModelType = typeof(ModelBase);
var baseDtoType = typeof(DtoBase);

var assembly = Assembly.GetExecutingAssembly();
var entityTypes = assembly.GetTypes().Where(t => baseModelType.IsAssignableFrom(t) && !t.IsAbstract).ToList();
var dtoTypes = assembly.GetTypes().Where(t => baseDtoType.IsAssignableFrom(t) && !t.IsAbstract).ToList();

for (int i = 0; i < entityTypes.Count; i++)
{
    var entityType = entityTypes[i];
    var dtoType = dtoTypes[i];

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
    if (dtoType == null) { throw new ArgumentNullException(paramName: dtoType.Name); }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

    var repositoryInterface = typeof(IBaseRepository<,>).MakeGenericType(entityType, dtoType);
    var repository = typeof(BaseRepository<,>).MakeGenericType(entityType, dtoType);
    var controllerInterface = typeof(IBaseController<,>).MakeGenericType(entityType, dtoType);
    var controller = typeof(BaseController<,>).MakeGenericType(entityType, dtoType);

    builder.Services
        .AddScoped(repositoryInterface, repository)
        .AddScoped(controller);
}
// Registra el controlador genérico
var mapper = AutoMapperConfig.Initialize();
builder.Services
    .AddScoped<IInstalacionService, InstalacionService>()
    .AddSingleton(mapper)
    .AddHttpClient()
    .AddEndpointsApiExplorer()
    .AddDbContext<DBContext>(options =>
    {
        var connection = builder.Configuration.GetConnectionString("DevConnection");
        options.UseSqlServer(connection, sqlOptions =>
            sqlOptions.EnableRetryOnFailure());
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