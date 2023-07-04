

using backend_API.Context;
using backend_API.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped <ModulosController>();

builder.Services.AddDbContext<DBContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("DevConnection");
    options.UseSqlServer(connection);
});

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Politica Acceso API", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("Politica Acceso API");
app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.UseCors();
app.MapGet("/", () => "Hello World!");
// Ejecutar aplicación
app.Run();
