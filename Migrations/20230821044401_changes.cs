using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
   /// <inheritdoc />
   public partial class Changes : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {

         migrationBuilder.CreateTable(
             name: "Cadenas",
             columns: table => new
             {
                IdCadena = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                MinModulos = table.Column<int>(type: "int", nullable: false),
                MaxModulos = table.Column<int>(type: "int", nullable: false),
                NumModulos = table.Column<int>(type: "int", nullable: false),
                NumCadenas = table.Column<int>(type: "int", nullable: false),
                NumInversores = table.Column<int>(type: "int", nullable: false),
                PotenciaPico = table.Column<double>(type: "float", nullable: false),
                PotenciaNominal = table.Column<double>(type: "float", nullable: false),
                CMaxString = table.Column<double>(type: "float", nullable: false),
                PotenciaString = table.Column<double>(type: "float", nullable: false),
                TensionString = table.Column<double>(type: "float", nullable: false),
                IdInversor = table.Column<int>(type: "int", nullable: false),
                IdModulo = table.Column<int>(type: "int", nullable: false),
                IdInstalacion = table.Column<int>(type: "int", nullable: true)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Cadenas", x => x.IdCadena);
                table.ForeignKey(
                       name: "FK_Cadenas_Instalaciones_IdInstalacion",
                       column: x => x.IdInstalacion,
                       principalTable: "Instalaciones",
                       principalColumn: "IdInstalacion");

             });
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {

      }
   }
}
