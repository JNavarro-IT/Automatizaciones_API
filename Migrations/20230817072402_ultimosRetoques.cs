using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    /// <inheritdoc />
    public partial class ultimosRetoques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cups",
                table: "Proyectos");

            migrationBuilder.RenameColumn(
                name: "CP",
                table: "Lugares",
                newName: "Cp");

            migrationBuilder.RenameColumn(
                name: "IdLugar",
                table: "Lugares",
                newName: "IdLugarPRL");

            migrationBuilder.AddColumn<string>(
                name: "Cups",
                table: "Ubicaciones",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cups",
                table: "Ubicaciones");

            migrationBuilder.RenameColumn(
                name: "Cp",
                table: "Lugares",
                newName: "CP");

            migrationBuilder.RenameColumn(
                name: "IdLugarPRL",
                table: "Lugares",
                newName: "IdLugar");

            migrationBuilder.AddColumn<string>(
                name: "Cups",
                table: "Proyectos",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
