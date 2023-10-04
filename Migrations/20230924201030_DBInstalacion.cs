using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
   /// <inheritdoc />
   public partial class DBInstalacion : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.AddColumn<double>(
             name: "ConsumoEstimado",
             table: "Instalaciones",
             type: "float",
             nullable: true);

         migrationBuilder.AddColumn<double>(
             name: "GeneracionAnual",
             table: "Instalaciones",
             type: "float",
             nullable: true);
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropColumn(
             name: "ConsumoEstimado",
             table: "Instalaciones");

         migrationBuilder.DropColumn(
             name: "GeneracionAnual",
             table: "Instalaciones");
      }
   }
}
