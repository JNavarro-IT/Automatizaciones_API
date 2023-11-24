using Automatizaciones_API.Configurations;
using Automatizaciones_API.Context;
using Microsoft.EntityFrameworkCore;


    // CLASE DE ARRANQUE Y CONFIGURACIÓN DE LA API
      var builder = WebApplication.CreateBuilder(args);
      var Services = builder.Services;
      builder.Configuration
          .AddUserSecrets(typeof(Program).Assembly)
          .AddEnvironmentVariables();

      Services.AddDbContext<DBContext>(opt =>
      {
         var env = builder.Environment.EnvironmentName;
         var connectionKey = $"ConnectionStrings:{env[..3]}Connection";

         var connection = builder.Configuration[connectionKey];
         opt.UseSqlServer(connection, sql => sql.EnableRetryOnFailure());
      });

      // AUTOMAPPER MODELO-DTO 
      var automaticService = new Automationfig(Services);
      Services.AddSingleton(AutoMapperConfig.Initialize());
      Services.AddHttpClient();
      Services.AddRouting();
      Services.AddEndpointsApiExplorer();
      Services.AddControllers();

      var app = builder.Build(); ;

      app.UseRouting()
         .UseCors()
         .UseEndpoints(ep => ep.MapControllers());
      app.Run();
