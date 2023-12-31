﻿using Automatizaciones_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Automatizaciones_API.Context
{
   //CLASE QUE GENERA UN CONTEXTO CON LA BD Y MAPEA LAS TABLAS
   public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
   {
      public DbSet<Cadena> Cadenas { get; set; }
      public DbSet<Cliente> Clientes { get; set; }
      public DbSet<Cubierta> Cubiertas { get; set; }
      public DbSet<Instalacion> Instalaciones { get; set; }
      public DbSet<Inversor> Inversores { get; set; }
      public DbSet<LugarPRL> LugaresPRL { get; set; }
      public DbSet<Modulo> Modulos { get; set; }
      public DbSet<Proyecto> Proyectos { get; set; }
      public DbSet<Ubicacion> Ubicaciones { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

      //API FLUENTE => PARA INDICAR EXPLICITAMENTE LOS PK, FK, COSTRAINST
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Proyecto>()
        .HasMany(p => p.LugaresPRL)
        .WithMany(l => l.Proyectos)
        .UsingEntity(j => j.ToTable("LugaresConProyectos"));
      }
   }
}
