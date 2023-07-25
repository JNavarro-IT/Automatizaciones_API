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
        public DbSet<LugarPRL> LugaresPRL { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        //API FLUENTE => PARA INDICAR EXPLICITAMENTE LOS PK, FK, COSTRAINST
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Proyecto>()
              .HasMany(p => p.LugaresPRL)
              .WithMany(l => l.Proyectos)
              .UsingEntity<Dictionary<string, object>>
              (
                "LugaresConProyectos",
                l => l
                    .HasOne<LugarPRL>()
                    .WithMany()
                    .HasForeignKey("IdLugarPRL"),
                p => p
                    .HasOne<Proyecto>()         
                    .WithMany()
                    .HasForeignKey("IdProyecto"), 
                lp => lp.HasKey("IdLugarPRL", "IdProyecto")       
              );
        }
    }
}
