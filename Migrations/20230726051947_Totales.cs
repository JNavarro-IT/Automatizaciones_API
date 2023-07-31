using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
    /// <inheritdoc />
    public partial class Totales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "TnumeroCadenas",
                table: "Instalaciones",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TnumModulos",
                table: "Instalaciones",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TnumInversores",
                table: "Instalaciones",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "NumModulos",
                 table: "Cadenas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumCadenas",
                table: "Cadenas",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumInversores",
                table: "Cadenas",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PotenciaNominal",
                table: "Cadenas",
                type: "float",
                nullable: true,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumCadenas",
                table: "Cadenas");

            migrationBuilder.DropColumn(
                name: "NumInversores",
                table: "Cadenas");

            migrationBuilder.DropColumn(
                name: "PotenciaNominal",
                table: "Cadenas");

            migrationBuilder.AlterColumn<string>(
                name: "TnumeroCadenas",
                table: "Instalaciones",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TnumModulos",
                table: "Instalaciones",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TnumInversores",
                table: "Instalaciones",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
