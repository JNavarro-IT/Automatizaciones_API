using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    public partial class changeProyectos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Ubicaciones_IdUbicacion",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Proyectos_IdUbicacion",
                table: "Proyectos");

            migrationBuilder.AlterColumn<int>(
                name: "IdUbicacion",
                table: "Proyectos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cups",
                table: "Proyectos",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Proyectos",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdUbicacion",
                table: "Proyectos",
                column: "IdUbicacion",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Ubicaciones_IdUbicacion",
                table: "Proyectos",
                column: "IdUbicacion",
                principalTable: "Ubicaciones",
                principalColumn: "IdUbicacion",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Ubicaciones_IdUbicacion",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Proyectos_IdUbicacion",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Cups",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Proyectos");

            migrationBuilder.AlterColumn<int>(
                name: "IdUbicacion",
                table: "Proyectos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdUbicacion",
                table: "Proyectos",
                column: "IdUbicacion",
                unique: true,
                filter: "[IdUbicacion] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Ubicaciones_IdUbicacion",
                table: "Proyectos",
                column: "IdUbicacion",
                principalTable: "Ubicaciones",
                principalColumn: "IdUbicacion");
        }
    }
}
