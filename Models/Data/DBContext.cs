using backend_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace backend_API.Models.Data
{
    public class DBContext : DbContext
    {
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Instalacion> Instalaciones { get; set; }
        public DbSet<Modulo> Modulos { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        //API FLUENTE => PARA INDICAR EXPLICITAMENTE LOS PK, FK, COSTRAINST (HAY FUNCIONES UNICAS)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Proyecto>().HasData(
               new Proyecto()
               {
                   Referencia = "OFC-G23-5001",
                   Version = "1",
                   Fecha = new DateTime(2023, 7, 12),
                   Cups = null,
                   IdCliente = 1,
                   IdUbicacion = 1,
                   IdInstalacion = 1
               }
              
            );

            modelBuilder.Entity<Cliente>().HasKey( c => c.IdCliente );
        }

    }
}
