﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend_API.Context;

#nullable disable

namespace backend_API.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20230705105745_changeProyectos")]
    partial class changeProyectos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("backend_API.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCliente"), 1L, 1);

                    b.Property<string>("Cp")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Municipio")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Observaciones")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Provincia")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Telefono")
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(500)");

                    b.HasKey("IdCliente");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("backend_API.Models.Instalacion", b =>
                {
                    b.Property<int>("IdInstalacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdInstalacion"), 1L, 1);

                    b.Property<string>("CoordX_conexion")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CoordY_conexion")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Potencia_nominal")
                        .HasColumnType("int");

                    b.Property<double>("Potencia_pico")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("IdInstalacion");

                    b.ToTable("Instalaciones");
                });

            modelBuilder.Entity("backend_API.Models.Modulo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Dimensiones")
                        .HasColumnType("varchar(25)");

                    b.Property<double?>("Eficiencia")
                        .HasColumnType("float");

                    b.Property<double?>("Icc")
                        .HasColumnType("float");

                    b.Property<double?>("Imp")
                        .HasColumnType("float");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("Num_Celulas")
                        .HasColumnType("int");

                    b.Property<double?>("Peso")
                        .HasColumnType("float");

                    b.Property<double?>("Potencia")
                        .HasColumnType("float");

                    b.Property<double?>("Salida_Potencia")
                        .HasColumnType("float");

                    b.Property<string>("Ta_TONC")
                        .HasColumnType("varchar(15)");

                    b.Property<double?>("Tension_Vacio")
                        .HasColumnType("float");

                    b.Property<string>("Tipo")
                        .HasColumnType("varchar(15)");

                    b.Property<double?>("Tolerancia")
                        .HasColumnType("float");

                    b.Property<double?>("Vca")
                        .HasColumnType("float");

                    b.Property<double?>("Vmp")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Modulos");
                });

            modelBuilder.Entity("backend_API.Models.Proyecto", b =>
                {
                    b.Property<int>("IdProyecto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProyecto"), 1L, 1);

                    b.Property<string>("Fecha")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<int?>("IdInstalacion")
                        .HasColumnType("int");

                    b.Property<int?>("IdUbicacion")
                        .HasColumnType("int");

                    b.Property<string>("Referencia")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("IdProyecto");

                    b.HasIndex("IdCliente")
                        .IsUnique();

                    b.HasIndex("IdInstalacion")
                        .IsUnique()
                        .HasFilter("[IdInstalacion] IS NOT NULL");

                    b.HasIndex("IdUbicacion")
                        .IsUnique()
                        .HasFilter("[IdUbicacion] IS NOT NULL");

                    b.ToTable("Proyectos");
                });

            modelBuilder.Entity("backend_API.Models.Ubicacion", b =>
                {
                    b.Property<int>("IdUbicacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUbicacion"), 1L, 1);

                    b.Property<int>("Azimut")
                        .HasColumnType("int");

                    b.Property<string>("CoordX")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CoordX_UTM")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CoordY")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CoordY_UTM")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Cups")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Inclinacion")
                        .HasColumnType("int");

                    b.Property<string>("Latitud")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Longitud")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Ref_catastral")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<double>("Superficie")
                        .HasColumnType("float");

                    b.HasKey("IdUbicacion");

                    b.ToTable("Ubicaciones");
                });

            modelBuilder.Entity("backend_API.Models.Proyecto", b =>
                {
                    b.HasOne("backend_API.Models.Cliente", "Cliente")
                        .WithOne("Contrato")
                        .HasForeignKey("backend_API.Models.Proyecto", "IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_API.Models.Instalacion", "Instalacion")
                        .WithOne("Contrato")
                        .HasForeignKey("backend_API.Models.Proyecto", "IdInstalacion");

                    b.HasOne("backend_API.Models.Ubicacion", "Ubicacion")
                        .WithOne("Contrato")
                        .HasForeignKey("backend_API.Models.Proyecto", "IdUbicacion");

                    b.Navigation("Cliente");

                    b.Navigation("Instalacion");

                    b.Navigation("Ubicacion");
                });

            modelBuilder.Entity("backend_API.Models.Cliente", b =>
                {
                    b.Navigation("Contrato")
                        .IsRequired();
                });

            modelBuilder.Entity("backend_API.Models.Instalacion", b =>
                {
                    b.Navigation("Contrato")
                        .IsRequired();
                });

            modelBuilder.Entity("backend_API.Models.Ubicacion", b =>
                {
                    b.Navigation("Contrato")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
