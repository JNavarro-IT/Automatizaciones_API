using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    public partial class NewStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    Dni = table.Column<string>(type: "varchar(15)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(15)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: true),
                    Observaciones = table.Column<string>(type: "varchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Lugares",
                columns: table => new
                {
                    IdLugar = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(100)", nullable: false),
                    CP = table.Column<int>(type: "int", nullable: false),
                    Municipio = table.Column<string>(type: "varchar(100)", nullable: false),
                    Provincia = table.Column<string>(type: "varchar(50)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(30)", nullable: false),
                    NIMA = table.Column<string>(type: "varchar(100)", nullable: true),
                    Autorizacion = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugares", x => x.IdLugar);
                });

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    IdModulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fabricante = table.Column<string>(type: "varchar(100)", nullable: false),
                    Modelo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Potencia = table.Column<double>(type: "float", nullable: false),
                    Vmp = table.Column<double>(type: "float", nullable: false),
                    Imp = table.Column<double>(type: "float", nullable: false),
                    Isc = table.Column<double>(type: "float", nullable: false),
                    Vca = table.Column<double>(type: "float", nullable: false),
                    Eficiencia = table.Column<double>(type: "float", nullable: true),
                    Dimensiones = table.Column<string>(type: "varchar(25)", nullable: true),
                    Peso = table.Column<double>(type: "float", nullable: true),
                    NumCelulas = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(15)", nullable: false),
                    TaTONC = table.Column<string>(type: "varchar(15)", nullable: true),
                    SalidaPotencia = table.Column<double>(type: "float", nullable: true),
                    TensionVacio = table.Column<double>(type: "float", nullable: true),
                    Tolerancia = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.IdModulo);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    IdProyecto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "varchar(100)", nullable: false),
                    Version = table.Column<string>(type: "varchar(50)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Cups = table.Column<string>(type: "varchar(100)", nullable: true),
                    Justificacion = table.Column<string>(type: "varchar(500)", nullable: true),
                    Presupuesto = table.Column<double>(type: "float", nullable: true),
                    PresupuestoSyS = table.Column<double>(type: "float", nullable: true),
                    PlazoEjecucion = table.Column<DateTime>(type: "date", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.IdProyecto);
                    table.ForeignKey(
                        name: "FK_Proyectos_Clientes_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                columns: table => new
                {
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ref_catastral = table.Column<string>(type: "varchar(100)", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(max)", nullable: false),
                    CP = table.Column<int>(type: "int", nullable: false),
                    Municipio = table.Column<string>(type: "varchar(100)", nullable: false),
                    Provincia = table.Column<string>(type: "varchar(50)", nullable: false),
                    Superficie = table.Column<double>(type: "float", nullable: false),
                    CoordXUTM = table.Column<double>(type: "float", nullable: true),
                    CoordYUTM = table.Column<double>(type: "float", nullable: true),
                    Latitud = table.Column<double>(type: "float", nullable: false),
                    Longitud = table.Column<double>(type: "float", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.IdUbicacion);
                    table.ForeignKey(
                        name: "FK_Ubicaciones_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instalaciones",
                columns: table => new
                {
                    IdInstalacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inclinacion = table.Column<double>(type: "float", nullable: false),
                    Azimut = table.Column<string>(type: "varchar(50)", nullable: false),
                    TotalPico = table.Column<double>(type: "float", nullable: false),
                    TotalNominal = table.Column<double>(type: "float", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(50)", nullable: false),
                    CoordXConexion = table.Column<double>(type: "float", nullable: false),
                    CoordYConexion = table.Column<double>(type: "float", nullable: false),
                    Fusible = table.Column<string>(type: "varchar(50)", nullable: false),
                    IDiferencial = table.Column<string>(type: "varchar(50)", nullable: false),
                    IMagenetico = table.Column<string>(type: "varchar(50)", nullable: false),
                    Estructura = table.Column<string>(type: "varchar(100)", nullable: false),
                    IdProyecto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instalaciones", x => x.IdInstalacion);
                    table.ForeignKey(
                        name: "FK_Instalaciones_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LugarProyecto",
                columns: table => new
                {
                    LugaresIdLugar = table.Column<int>(type: "int", nullable: false),
                    ProyectoIdProyecto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LugarProyecto", x => new { x.LugaresIdLugar, x.ProyectoIdProyecto });
                    table.ForeignKey(
                        name: "FK_LugarProyecto_Lugares_LugaresIdLugar",
                        column: x => x.LugaresIdLugar,
                        principalTable: "Lugares",
                        principalColumn: "IdLugar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LugarProyecto_Proyectos_ProyectoIdProyecto",
                        column: x => x.ProyectoIdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cubiertas",
                columns: table => new
                {
                    IdCubierta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedidasColectivas = table.Column<string>(type: "varchar(50)", nullable: false),
                    Accesibilidad = table.Column<string>(type: "varchar(50)", nullable: false),
                    Material = table.Column<string>(type: "varchar(100)", nullable: false),
                    IdUbicacion = table.Column<int>(type: "int", nullable: false),
                    UbicacionIdUbicacion = table.Column<int>(type: "int", nullable: true),
                    IdInstalacion = table.Column<int>(type: "int", nullable: false),
                    InstalacionIdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cubiertas", x => x.IdCubierta);
                    table.ForeignKey(
                        name: "FK_Cubiertas_Instalaciones_InstalacionIdInstalacion",
                        column: x => x.InstalacionIdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                    table.ForeignKey(
                        name: "FK_Cubiertas_Ubicaciones_UbicacionIdUbicacion",
                        column: x => x.UbicacionIdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion");
                });

            migrationBuilder.CreateTable(
                name: "Inversores",
                columns: table => new
                {
                    IdInversor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fabricante = table.Column<string>(type: "varchar(100)", nullable: false),
                    Modelo = table.Column<string>(type: "varchar(100)", nullable: false),
                    PotenciaNominal = table.Column<double>(type: "float", nullable: false),
                    VO = table.Column<int>(type: "int", nullable: false),
                    IO = table.Column<double>(type: "float", nullable: false),
                    Vmin = table.Column<int>(type: "int", nullable: false),
                    Vmax = table.Column<int>(type: "int", nullable: false),
                    CorrienteMaxString = table.Column<double>(type: "float", nullable: false),
                    VminMPPT = table.Column<int>(type: "int", nullable: true),
                    VmaxMPPT = table.Column<int>(type: "int", nullable: true),
                    IntensidadMaxMPPT = table.Column<double>(type: "float", nullable: true),
                    Rendimiento = table.Column<double>(type: "float", nullable: true),
                    Vatimetro = table.Column<string>(type: "varchar(250)", nullable: false),
                    InstalacionIdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inversores", x => x.IdInversor);
                    table.ForeignKey(
                        name: "FK_Inversores_Instalaciones_InstalacionIdInstalacion",
                        column: x => x.InstalacionIdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                });

            migrationBuilder.CreateTable(
                name: "Cadenas",
                columns: table => new
                {
                    IdCadena = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinModulos = table.Column<int>(type: "int", nullable: false),
                    MaxModulos = table.Column<int>(type: "int", nullable: false),
                    NumModulos = table.Column<int>(type: "int", nullable: false),
                    PotenciaPico = table.Column<double>(type: "float", nullable: false),
                    IdInversor = table.Column<int>(type: "int", nullable: false),
                    InversorIdInversor = table.Column<int>(type: "int", nullable: false),
                    IdModulo = table.Column<int>(type: "int", nullable: false),
                    ModuloIdModulo = table.Column<int>(type: "int", nullable: false),
                    IdInstalacion = table.Column<int>(type: "int", nullable: false),
                    InstalacionIdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadenas", x => x.IdCadena);
                    table.ForeignKey(
                        name: "FK_Cadenas_Instalaciones_InstalacionIdInstalacion",
                        column: x => x.InstalacionIdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                    table.ForeignKey(
                        name: "FK_Cadenas_Inversores_InversorIdInversor",
                        column: x => x.InversorIdInversor,
                        principalTable: "Inversores",
                        principalColumn: "IdInversor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cadenas_Modulos_ModuloIdModulo",
                        column: x => x.ModuloIdModulo,
                        principalTable: "Modulos",
                        principalColumn: "IdModulo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cadenas_InstalacionIdInstalacion",
                table: "Cadenas",
                column: "InstalacionIdInstalacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cadenas_InversorIdInversor",
                table: "Cadenas",
                column: "InversorIdInversor");

            migrationBuilder.CreateIndex(
                name: "IX_Cadenas_ModuloIdModulo",
                table: "Cadenas",
                column: "ModuloIdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_Cubiertas_InstalacionIdInstalacion",
                table: "Cubiertas",
                column: "InstalacionIdInstalacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cubiertas_UbicacionIdUbicacion",
                table: "Cubiertas",
                column: "UbicacionIdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Instalaciones_IdProyecto",
                table: "Instalaciones",
                column: "IdProyecto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inversores_InstalacionIdInstalacion",
                table: "Inversores",
                column: "InstalacionIdInstalacion");

            migrationBuilder.CreateIndex(
                name: "IX_LugarProyecto_ProyectoIdProyecto",
                table: "LugarProyecto",
                column: "ProyectoIdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_ClienteIdCliente",
                table: "Proyectos",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicaciones_IdCliente",
                table: "Ubicaciones",
                column: "IdCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cadenas");

            migrationBuilder.DropTable(
                name: "Cubiertas");

            migrationBuilder.DropTable(
                name: "LugarProyecto");

            migrationBuilder.DropTable(
                name: "Inversores");

            migrationBuilder.DropTable(
                name: "Modulos");

            migrationBuilder.DropTable(
                name: "Ubicaciones");

            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropTable(
                name: "Instalaciones");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
