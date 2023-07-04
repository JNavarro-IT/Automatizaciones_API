using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    public partial class Initial : Migration
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
                    Direccion = table.Column<string>(type: "varchar(100)", nullable: false),
                    Cp = table.Column<string>(type: "varchar(10)", nullable: false),
                    Provincia = table.Column<string>(type: "varchar(50)", nullable: false),
                    Municipio = table.Column<string>(type: "varchar(50)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(15)", nullable: true),
                    Url = table.Column<string>(type: "varchar(500)", nullable: false),
                    Observaciones = table.Column<string>(type: "varchar(100)", nullable: true)
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
                    Potencia_pico = table.Column<double>(type: "float", nullable: false),
                    Potencia_nominal = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(100)", nullable: false),
                    CoordX_conexion = table.Column<string>(type: "varchar(100)", nullable: true),
                    CoordY_conexion = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instalaciones", x => x.IdInstalacion);
                });

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    Potencia = table.Column<double>(type: "float", nullable: true),
                    Vmp = table.Column<double>(type: "float", nullable: true),
                    Imp = table.Column<double>(type: "float", nullable: true),
                    Icc = table.Column<double>(type: "float", nullable: true),
                    Vca = table.Column<double>(type: "float", nullable: true),
                    Eficiencia = table.Column<double>(type: "float", nullable: true),
                    Dimensiones = table.Column<string>(type: "varchar(25)", nullable: true),
                    Peso = table.Column<double>(type: "float", nullable: true),
                    Num_Celulas = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(15)", nullable: true),
                    Ta_TONC = table.Column<string>(type: "varchar(15)", nullable: true),
                    Salida_Potencia = table.Column<double>(type: "float", nullable: true),
                    Tension_Vacio = table.Column<double>(type: "float", nullable: true),
                    Tolerancia = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                columns: table => new
                {
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ref_catastral = table.Column<string>(type: "varchar(100)", nullable: false),
                    Cups = table.Column<string>(type: "varchar(100)", nullable: true),
                    Superficie = table.Column<double>(type: "float", nullable: false),
                    CoordX_UTM = table.Column<string>(type: "varchar(50)", nullable: true),
                    CoordY_UTM = table.Column<string>(type: "varchar(50)", nullable: true),
                    CoordX = table.Column<string>(type: "varchar(50)", nullable: true),
                    CoordY = table.Column<string>(type: "varchar(50)", nullable: true),
                    Latitud = table.Column<string>(type: "varchar(100)", nullable: false),
                    Longitud = table.Column<string>(type: "varchar(100)", nullable: false),
                    Inclinacion = table.Column<int>(type: "int", nullable: false),
                    Azimut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.IdUbicacion);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    IdContrato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "varchar(100)", nullable: false),
                    Fecha = table.Column<string>(type: "varchar(20)", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdUbicacion = table.Column<int>(type: "int", nullable: true),
                    IdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.IdContrato);
                    table.ForeignKey(
                        name: "FK_Contratos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contratos_Instalaciones_IdInstalacion",
                        column: x => x.IdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                    table.ForeignKey(
                        name: "FK_Contratos_Ubicaciones_IdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_IdCliente",
                table: "Contratos",
                column: "IdCliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_IdInstalacion",
                table: "Contratos",
                column: "IdInstalacion",
                unique: true,
                filter: "[IdInstalacion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_IdUbicacion",
                table: "Contratos",
                column: "IdUbicacion",
                unique: true,
                filter: "[IdUbicacion] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Modulos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Instalaciones");

            migrationBuilder.DropTable(
                name: "Ubicaciones");
        }
    }
}
