using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
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
                    Vatimetro = table.Column<string>(type: "varchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instalaciones", x => x.IdInstalacion);
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
                name: "Ubicaciones",
                columns: table => new
                {
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefCatastral = table.Column<string>(type: "varchar(100)", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(max)", nullable: false),
                    CP = table.Column<int>(type: "int", nullable: false),
                    Municipio = table.Column<string>(type: "varchar(100)", nullable: false),
                    Provincia = table.Column<string>(type: "varchar(50)", nullable: false),
                    Superficie = table.Column<double>(type: "float", nullable: false),
                    CoordXUTM = table.Column<double>(type: "float", nullable: true),
                    CoordYUTM = table.Column<double>(type: "float", nullable: true),
                    Latitud = table.Column<double>(type: "float", nullable: false),
                    Longitud = table.Column<double>(type: "float", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.IdUbicacion);
                    table.ForeignKey(
                        name: "FK_UIdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UIdInstalacion",
                        column: x => x.IdInstalacion,
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
                    IdModulo = table.Column<int>(type: "int", nullable: false),
                    IdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadenas", x => x.IdCadena);
                    table.ForeignKey(
                        name: "FK_CIdInstalacion",
                        column: x => x.IdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                    table.ForeignKey(
                        name: "FK_CIdInversor",
                        column: x => x.IdInversor,
                        principalTable: "Inversores",
                        principalColumn: "IdInversor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CIdModulo",
                        column: x => x.IdModulo,
                        principalTable: "Modulos",
                        principalColumn: "IdModulo",
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
                    IdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cubiertas", x => x.IdCubierta);
                    table.ForeignKey(
                        name: "FK_CuIdInstalacion",
                        column: x => x.IdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                    table.ForeignKey(
                        name: "FK_CunIdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    IdProyecto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "varchar(100)", nullable: false),
                    Version = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Cups = table.Column<string>(type: "varchar(100)", nullable: true),
                    Justificacion = table.Column<string>(type: "varchar(500)", nullable: true),
                    Presupuesto = table.Column<double>(type: "float", nullable: true),
                    PresupuestoSyS = table.Column<double>(type: "float", nullable: true),
                    PlazoEjecucion = table.Column<DateTime>(type: "date", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdInstalacion = table.Column<int>(type: "int", nullable: false),
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.IdProyecto);
                    table.ForeignKey(
                        name: "FK_PIdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PIdInstalacion",
                        column: x => x.IdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PIdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion");
                });

            migrationBuilder.CreateTable(
                name: "LugaresConProyectos",
                columns: table => new
                {
                    IdLugarPRL = table.Column<int>(type: "int", nullable: false),
                    IdProyecto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LugaresConProyectos", x => new { x.IdLugarPRL, x.IdProyecto });
                    table.ForeignKey(
                        name: "FK_LIdLugarPRL",
                        column: x => x.IdLugarPRL,
                        principalTable: "Lugares",
                        principalColumn: "IdLugar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LIdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cadenas_IdInversor",
                table: "Cadenas",
                column: "IdInversor");

            migrationBuilder.CreateIndex(
                name: "IX_Cadenas_IdModulo",
                table: "Cadenas",
                column: "IdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_Cadenas_IdInstalacion",
                table: "Cadenas",
                column: "IdInstalacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cubiertas_IdInstalacion",
                table: "Cubiertas",
                column: "IdInstalacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cubiertas_IdUbicacion",
                table: "Cubiertas",
                column: "IdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_LugaresConProyectos_IdProyecto",
                table: "LugaresConProyectos",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdCliente",
                table: "Proyectos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdInstalacion",
                table: "Proyectos",
                column: "IdInstalacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdUbicacion",
                table: "Proyectos",
                column: "IdUbicacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ubicaciones_IdCliente",
                table: "Ubicaciones",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicaciones_IdInstalacion",
                table: "Ubicaciones",
                column: "IdInstalacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cadenas");

            migrationBuilder.DropTable(
                name: "Cubiertas");

            migrationBuilder.DropTable(
                name: "LugaresConProyectos");

            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Ubicaciones");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Instalaciones");
        }
    }
}