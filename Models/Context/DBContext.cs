using Microsoft.EntityFrameworkCore;

namespace backend_API.Models.Data
{
    //CLASE QUE GENERA UN CONTEXTO CON LA BD Y MAPEA LAS TABLAS
    public class DBContext : DbContext
    {
        public DbSet<Cliente> Cadenas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cubierta> Cubiertas { get; set; }
        public DbSet<Instalacion> Instalaciones { get; set; }
        public DbSet<Inversor> Inversores { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        //API FLUENTE => PARA INDICAR EXPLICITAMENTE LOS PK, FK, COSTRAINST (HAY FUNCIONES UNICAS)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Ubicacion>()
               .HasOne(u => u.Cliente)
               .WithMany(c => c.Ubicaciones)
               .HasForeignKey(u => u.IdCliente);



            /*
        modelBuilder.Entity<Cadena>()
            .HasOne(c => c.Instalacion) // Relación muchos a uno con Instalacion
            .WithMany(i => i.Cadenas)   // Indicar la propiedad de navegación en Instalacion (si tienes una en Instalacion)
            .HasForeignKey(c => c.IdInstalacion); // Clave externa para la relación

        modelBuilder.Entity<Cadena>()
            .HasOne(c => c.Inversor) // Relación muchos a uno con Inversor
            .WithMany(i => i.Cadenas)   // Indicar la propiedad de navegación en Inversor (si tienes una en Inversor)
            .HasForeignKey(c => c.IdInversor); // Clave externa para la relación

        modelBuilder.Entity<Cadena>()
            .HasOne(c => c.Modulo) // Relación muchos a uno con Modulo
            .WithMany(m => m.Cadenas)   // Indicar la propiedad de navegación en Modulo (si tienes una en Modulo)
            .HasForeignKey(c => c.IdModulo); // Clave externa para la relación*/

        }

    }
}
