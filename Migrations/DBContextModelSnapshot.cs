﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend_API.Models.Data;

#nullable disable

namespace backend_API.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LugaresConProyectos", b =>
                {
                    b.Property<int>("IdLugarPRL")
                        .HasColumnType("int");

                    b.Property<int>("IdProyecto")
                        .HasColumnType("int");

                    b.HasKey("IdLugarPRL", "IdProyecto");

                    b.HasIndex("IdProyecto");

                    b.ToTable("LugaresConProyectos");
                });

            modelBuilder.Entity("backend_API.Models.Cadena", b =>
                {
                    b.Property<int>("IdCadena")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCadena"));

                    b.Property<double>("CMaxString")
                        .HasColumnType("float");

                    b.Property<int?>("IdInstalacion")
                        .HasColumnType("int");

                    b.Property<int>("IdInversor")
                        .HasColumnType("int");

                    b.Property<int>("IdModulo")
                        .HasColumnType("int");

                    b.Property<int>("MaxModulos")
                        .HasColumnType("int");

                    b.Property<int>("MinModulos")
                        .HasColumnType("int");

                    b.Property<int>("NumCadenas")
                        .HasColumnType("int");

                    b.Property<int>("NumInversores")
                        .HasColumnType("int");

                    b.Property<int>("NumModulos")
                        .HasColumnType("int");

                    b.Property<double>("PotenciaNominal")
                        .HasColumnType("float");

                    b.Property<double>("PotenciaPico")
                        .HasColumnType("float");

                    b.Property<double>("PotenciaString")
                        .HasColumnType("float");

                    b.Property<double>("TensionString")
                        .HasColumnType("float");

                    b.HasKey("IdCadena");

                    b.HasIndex("IdInstalacion");

                    b.HasIndex("IdInversor");

                    b.HasIndex("IdModulo");

                    b.ToTable("Cadenas");
                });

            modelBuilder.Entity("backend_API.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCliente"));

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Observaciones")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.HasKey("IdCliente");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("backend_API.Models.Cubierta", b =>
                {
                    b.Property<int>("IdCubierta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCubierta"));

                    b.Property<string>("Accesibilidad")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("InstalacionIdInstalacion")
                        .HasColumnType("int");

                    b.Property<string>("Material")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MedidasColectivas")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("UbicacionIdUbicacion")
                        .HasColumnType("int");

                    b.HasKey("IdCubierta");

                    b.HasIndex("InstalacionIdInstalacion");

                    b.HasIndex("UbicacionIdUbicacion");

                    b.ToTable("Cubiertas");
                });

            modelBuilder.Entity("backend_API.Models.Instalacion", b =>
                {
                    b.Property<int>("IdInstalacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdInstalacion"));

                    b.Property<string>("Azimut")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<double>("CoordXConexion")
                        .HasColumnType("float");

                    b.Property<double>("CoordYConexion")
                        .HasColumnType("float");

                    b.Property<string>("Estructura")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Fusible")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("IAutomatico")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("IDiferencial")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("IdUbicacion")
                        .HasColumnType("int");

                    b.Property<double>("Inclinacion")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("TotalCadenas")
                        .HasColumnType("int");

                    b.Property<int?>("TotalInversores")
                        .HasColumnType("int");

                    b.Property<int>("TotalModulos")
                        .HasColumnType("int");

                    b.Property<double>("TotalNominal")
                        .HasColumnType("float");

                    b.Property<double>("TotalPico")
                        .HasColumnType("float");

                    b.Property<string>("Vatimetro")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.HasKey("IdInstalacion");

                    b.HasIndex("IdUbicacion")
                        .IsUnique()
                        .HasFilter("[IdUbicacion] IS NOT NULL");

                    b.ToTable("Instalaciones");
                });

            modelBuilder.Entity("backend_API.Models.Inversor", b =>
                {
                    b.Property<int>("IdInversor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdInversor"));

                    b.Property<double>("CorrienteMaxString")
                        .HasColumnType("float");

                    b.Property<string>("Fabricante")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("IO")
                        .HasColumnType("float");

                    b.Property<double?>("IntensidadMaxMPPT")
                        .HasColumnType("float");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("PotenciaNominal")
                        .HasColumnType("float");

                    b.Property<double?>("Rendimiento")
                        .HasColumnType("float");

                    b.Property<int>("VO")
                        .HasColumnType("int");

                    b.Property<int>("Vmax")
                        .HasColumnType("int");

                    b.Property<int?>("VmaxMPPT")
                        .HasColumnType("int");

                    b.Property<int>("Vmin")
                        .HasColumnType("int");

                    b.Property<int?>("VminMPPT")
                        .HasColumnType("int");

                    b.HasKey("IdInversor");

                    b.ToTable("Inversores");
                });

            modelBuilder.Entity("backend_API.Models.LugarPRL", b =>
                {
                    b.Property<int>("IdLugar")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLugar"));

                    b.Property<string>("Autorizacion")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("CP")
                        .HasColumnType("int");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Latitud")
                        .HasColumnType("float");

                    b.Property<double>("Longitud")
                        .HasColumnType("float");

                    b.Property<string>("Municipio")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NIMA")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Provincia")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RutaImg")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLugar");

                    b.ToTable("Lugares");
                });

            modelBuilder.Entity("backend_API.Models.Modulo", b =>
                {
                    b.Property<int>("IdModulo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdModulo"));

                    b.Property<string>("Dimensiones")
                        .HasColumnType("varchar(25)");

                    b.Property<double?>("Eficiencia")
                        .HasColumnType("float");

                    b.Property<string>("Fabricante")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Imp")
                        .HasColumnType("float");

                    b.Property<double>("Isc")
                        .HasColumnType("float");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("NumCelulas")
                        .HasColumnType("int");

                    b.Property<double?>("Peso")
                        .HasColumnType("float");

                    b.Property<double>("Potencia")
                        .HasColumnType("float");

                    b.Property<double?>("SalidaPotencia")
                        .HasColumnType("float");

                    b.Property<string>("TaTONC")
                        .HasColumnType("varchar(15)");

                    b.Property<double?>("TensionVacio")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .HasColumnType("varchar(15)");

                    b.Property<double?>("Tolerancia")
                        .HasColumnType("float");

                    b.Property<double>("Vca")
                        .HasColumnType("float");

                    b.Property<double>("Vmp")
                        .HasColumnType("float");

                    b.HasKey("IdModulo");

                    b.ToTable("Modulos");
                });

            modelBuilder.Entity("backend_API.Models.Proyecto", b =>
                {
                    b.Property<int>("IdProyecto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProyecto"));

                    b.Property<string>("Cups")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("date");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<int>("IdInstalacion")
                        .HasColumnType("int");

                    b.Property<int>("IdUbicacion")
                        .HasColumnType("int");

                    b.Property<string>("Justificacion")
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime?>("PlazoEjecucion")
                        .HasColumnType("date");

                    b.Property<double?>("Presupuesto")
                        .HasColumnType("float");

                    b.Property<double?>("PresupuestoSyS")
                        .HasColumnType("float");

                    b.Property<string>("Referencia")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Version")
                        .HasColumnType("float");

                    b.HasKey("IdProyecto");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdInstalacion")
                        .IsUnique();

                    b.HasIndex("IdUbicacion")
                        .IsUnique();

                    b.ToTable("Proyectos");
                });

            modelBuilder.Entity("backend_API.Models.Ubicacion", b =>
                {
                    b.Property<int>("IdUbicacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUbicacion"));

                    b.Property<int>("CP")
                        .HasColumnType("int");

                    b.Property<double?>("CoordXUTM")
                        .HasColumnType("float");

                    b.Property<double?>("CoordYUTM")
                        .HasColumnType("float");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<double>("Latitud")
                        .HasColumnType("float");

                    b.Property<double>("Longitud")
                        .HasColumnType("float");

                    b.Property<string>("Municipio")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Provincia")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RefCatastral")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Superficie")
                        .HasColumnType("float");

                    b.HasKey("IdUbicacion");

                    b.HasIndex("IdCliente");

                    b.ToTable("Ubicaciones");
                });

            modelBuilder.Entity("LugaresConProyectos", b =>
                {
                    b.HasOne("backend_API.Models.LugarPRL", null)
                        .WithMany()
                        .HasForeignKey("IdLugarPRL")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_API.Models.Proyecto", null)
                        .WithMany()
                        .HasForeignKey("IdProyecto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_API.Models.Cadena", b =>
                {
                    b.HasOne("backend_API.Models.Instalacion", "Instalacion")
                        .WithMany("Cadenas")
                        .HasForeignKey("IdInstalacion");

                    b.HasOne("backend_API.Models.Inversor", "Inversor")
                        .WithMany()
                        .HasForeignKey("IdInversor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_API.Models.Modulo", "Modulo")
                        .WithMany()
                        .HasForeignKey("IdModulo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instalacion");

                    b.Navigation("Inversor");

                    b.Navigation("Modulo");
                });

            modelBuilder.Entity("backend_API.Models.Cubierta", b =>
                {
                    b.HasOne("backend_API.Models.Instalacion", "Instalacion")
                        .WithMany("Cubiertas")
                        .HasForeignKey("InstalacionIdInstalacion");

                    b.HasOne("backend_API.Models.Ubicacion", "Ubicacion")
                        .WithMany("Cubiertas")
                        .HasForeignKey("UbicacionIdUbicacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instalacion");

                    b.Navigation("Ubicacion");
                });

            modelBuilder.Entity("backend_API.Models.Instalacion", b =>
                {
                    b.HasOne("backend_API.Models.Ubicacion", "Ubicacion")
                        .WithOne("Instalacion")
                        .HasForeignKey("backend_API.Models.Instalacion", "IdUbicacion");

                    b.Navigation("Ubicacion");
                });

            modelBuilder.Entity("backend_API.Models.Proyecto", b =>
                {
                    b.HasOne("backend_API.Models.Cliente", "Cliente")
                        .WithMany("Proyectos")
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_API.Models.Instalacion", "Instalacion")
                        .WithOne("Proyecto")
                        .HasForeignKey("backend_API.Models.Proyecto", "IdInstalacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_API.Models.Ubicacion", "Ubicacion")
                        .WithOne("Proyecto")
                        .HasForeignKey("backend_API.Models.Proyecto", "IdUbicacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Instalacion");

                    b.Navigation("Ubicacion");
                });

            modelBuilder.Entity("backend_API.Models.Ubicacion", b =>
                {
                    b.HasOne("backend_API.Models.Cliente", "Cliente")
                        .WithMany("Ubicaciones")
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("backend_API.Models.Cliente", b =>
                {
                    b.Navigation("Proyectos");

                    b.Navigation("Ubicaciones");
                });

            modelBuilder.Entity("backend_API.Models.Instalacion", b =>
                {
                    b.Navigation("Cadenas");

                    b.Navigation("Cubiertas");

                    b.Navigation("Proyecto");
                });

            modelBuilder.Entity("backend_API.Models.Ubicacion", b =>
                {
                    b.Navigation("Cubiertas");

                    b.Navigation("Instalacion");

                    b.Navigation("Proyecto");
                });
#pragma warning restore 612, 618
        }
    }
}
