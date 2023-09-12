using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    /// <inheritdoc />
    public partial class UbicacionSub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Ubicaciones");

            migrationBuilder.RenameColumn(
                name: "CP",
                table: "Ubicaciones",
                newName: "Cp");

            migrationBuilder.AddColumn<string>(
                name: "Bloque",
                table: "Ubicaciones",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Calle",
                table: "Ubicaciones",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cif",
                table: "Ubicaciones",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "Ubicaciones",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Escalera",
                table: "Ubicaciones",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Ubicaciones",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Piso",
                table: "Ubicaciones",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Portal",
                table: "Ubicaciones",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Puerta",
                table: "Ubicaciones",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bloque",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Calle",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Cif",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Escalera",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Piso",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Portal",
                table: "Ubicaciones");

            migrationBuilder.DropColumn(
                name: "Puerta",
                table: "Ubicaciones");

            migrationBuilder.RenameColumn(
                name: "Cp",
                table: "Ubicaciones",
                newName: "CP");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Ubicaciones",
                type: "varchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
