using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    /// <inheritdoc />
    public partial class LugaresProyectos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LugaresConProyectos_Lugares_IdLugarPRL",
                table: "LugaresConProyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_LugaresConProyectos_Proyectos_IdProyecto",
                table: "LugaresConProyectos");

            migrationBuilder.RenameColumn(
                name: "IdProyecto",
                table: "LugaresConProyectos",
                newName: "ProyectosIdProyecto");

            migrationBuilder.RenameColumn(
                name: "IdLugarPRL",
                table: "LugaresConProyectos",
                newName: "LugaresPRLIdLugarPRL");

            migrationBuilder.RenameIndex(
                name: "IX_LugaresConProyectos_IdProyecto",
                table: "LugaresConProyectos",
                newName: "IX_LugaresConProyectos_ProyectosIdProyecto");

            migrationBuilder.AddForeignKey(
                name: "FK_LugaresConProyectos_Lugares_LugaresPRLIdLugarPRL",
                table: "LugaresConProyectos",
                column: "LugaresPRLIdLugarPRL",
                principalTable: "Lugares",
                principalColumn: "IdLugarPRL",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LugaresConProyectos_Proyectos_ProyectosIdProyecto",
                table: "LugaresConProyectos",
                column: "ProyectosIdProyecto",
                principalTable: "Proyectos",
                principalColumn: "IdProyecto",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LugaresConProyectos_Lugares_LugaresPRLIdLugarPRL",
                table: "LugaresConProyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_LugaresConProyectos_Proyectos_ProyectosIdProyecto",
                table: "LugaresConProyectos");

            migrationBuilder.RenameColumn(
                name: "ProyectosIdProyecto",
                table: "LugaresConProyectos",
                newName: "IdProyecto");

            migrationBuilder.RenameColumn(
                name: "LugaresPRLIdLugarPRL",
                table: "LugaresConProyectos",
                newName: "IdLugarPRL");

            migrationBuilder.RenameIndex(
                name: "IX_LugaresConProyectos_ProyectosIdProyecto",
                table: "LugaresConProyectos",
                newName: "IX_LugaresConProyectos_IdProyecto");

            migrationBuilder.AddForeignKey(
                name: "FK_LugaresConProyectos_Lugares_IdLugarPRL",
                table: "LugaresConProyectos",
                column: "IdLugarPRL",
                principalTable: "Lugares",
                principalColumn: "IdLugarPRL",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LugaresConProyectos_Proyectos_IdProyecto",
                table: "LugaresConProyectos",
                column: "IdProyecto",
                principalTable: "Proyectos",
                principalColumn: "IdProyecto",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
