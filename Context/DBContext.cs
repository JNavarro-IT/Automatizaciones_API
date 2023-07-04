using backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_API.Context
{
    public class DBContext : DbContext
    {
        public DbSet<Modulo>? Modulos { get; set; }
        public DbSet<Cliente>? Clientes { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Instalacion> Instalaciones { get; set; }

        public DbSet<Contrato> Contratos { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options){}
    }
}
