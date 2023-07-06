using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    public partial class changeProyectos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    IdProyecto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "varchar(100)", nullable: false),
                    Fecha = table.Column<string>(type: "varchar(20)", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdUbicacion = table.Column<int>(type: "int", nullable: true),
                    IdInstalacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.IdProyecto);
                    table.ForeignKey(
                        name: "FK_Proyectos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proyectos_Instalaciones_IdInstalacion",
                        column: x => x.IdInstalacion,
                        principalTable: "Instalaciones",
                        principalColumn: "IdInstalacion");
                    table.ForeignKey(
                        name: "FK_Proyectos_Ubicaciones_IdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdCliente",
                table: "Proyectos",
                column: "IdCliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdInstalacion",
                table: "Proyectos",
                column: "IdInstalacion",
                unique: true,
                filter: "[IdInstalacion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdUbicacion",
                table: "Proyectos",
                column: "IdUbicacion",
                unique: true,
                filter: "[IdUbicacion] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    IdContrato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdInstalacion = table.Column<int>(type: "int", nullable: true),
                    IdUbicacion = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<string>(type: "varchar(20)", nullable: false),
                    Referencia = table.Column<string>(type: "varchar(100)", nullable: false)
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
    }
}
