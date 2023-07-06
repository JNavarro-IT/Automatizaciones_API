using backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_API.Context
{
    public class DBContext : DbContext
    {
        public DbSet<Cliente>? Clientes { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Instalacion> Instalaciones { get; set; }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options){}


        public DbSet<Modulo>? Modulos { get; set; }

    }
}
