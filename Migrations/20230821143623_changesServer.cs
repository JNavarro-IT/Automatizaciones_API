using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
   /// <inheritdoc />
   public partial class ChangesServer : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {

      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateIndex(
             name: "IX_Cadenas_IdInversor",
             table: "Cadenas",
             column: "IdInversor");

         migrationBuilder.CreateIndex(
             name: "IX_Cadenas_IdModulo",
             table: "Cadenas",
             column: "IdModulo");

      }
   }
}
