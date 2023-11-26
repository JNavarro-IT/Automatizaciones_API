using Automatizaciones_API.Configurations;
using Automatizaciones_API.Context;
using Microsoft.EntityFrameworkCore;

// CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
var Env = builder.Environment;

new AutoConfig(Services).InitAutoConfig();        // AUTOCONFIG
Services.AddSingleton(AutoMapperConfig.Initialize()); // AUTOMAPPER 
Services.AddHttpClient();
Services.AddRouting();
Services.AddEndpointsApiExplorer();
Services.AddControllers();

builder.Configuration
    .AddUserSecrets(typeof(Program).Assembly)         // SECRETS
    .AddEnvironmentVariables();

Services.AddDbContext<DBContext>(opt =>               // DBCONECTION
{
   var EnvName = Env.EnvironmentName;
   var connectionKey = $"ConnectionStrings:{EnvName[..4]}Connection";
   var connection = builder.Configuration[connectionKey];

   opt.UseSqlServer(connection, sql => sql.EnableRetryOnFailure());
});

var app = builder.Build();
app.UseRouting()
   .UseCors("Cors")
   .UseEndpoints(ep => ep.MapControllers());
app.Run();
