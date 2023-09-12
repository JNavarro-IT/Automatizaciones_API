using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_API.Migrations
{
   /// <inheritdoc />
   public partial class SaveData : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropForeignKey(
             name: "FK_Cubiertas_Ubicaciones_UbicacionIdUbicacion",
             table: "Cubiertas");

         migrationBuilder.DropForeignKey(
             name: "FK_Proyectos_Ubicaciones_IdUbicacion",
             table: "Proyectos");

         migrationBuilder.DropIndex(
             name: "IX_Proyectos_IdUbicacion",
             table: "Proyectos");

         migrationBuilder.DropIndex(
             name: "IX_Cubiertas_UbicacionIdUbicacion",
             table: "Cubiertas");

         migrationBuilder.DropColumn(
             name: "IdUbicacion",
             table: "Proyectos");

         migrationBuilder.DropColumn(
             name: "UbicacionIdUbicacion",
             table: "Cubiertas");

         migrationBuilder.AddColumn<int>(
             name: "ProyectoIdProyecto",
             table: "Ubicaciones",
             type: "int",
             nullable: true);

         migrationBuilder.CreateIndex(
             name: "IX_Ubicaciones_ProyectoIdProyecto",
             table: "Ubicaciones",
             column: "ProyectoIdProyecto");

         migrationBuilder.AddForeignKey(
             name: "FK_Ubicaciones_Proyectos_ProyectoIdProyecto",
             table: "Ubicaciones",
             column: "ProyectoIdProyecto",
             principalTable: "Proyectos",
             principalColumn: "IdProyecto");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropForeignKey(
             name: "FK_Ubicaciones_Proyectos_ProyectoIdProyecto",
             table: "Ubicaciones");

         migrationBuilder.DropIndex(
             name: "IX_Ubicaciones_ProyectoIdProyecto",
             table: "Ubicaciones");

         migrationBuilder.DropColumn(
             name: "ProyectoIdProyecto",
             table: "Ubicaciones");

         migrationBuilder.AddColumn<int>(
             name: "IdUbicacion",
             table: "Proyectos",
             type: "int",
             nullable: false,
             defaultValue: 0);

         migrationBuilder.AddColumn<int>(
             name: "UbicacionIdUbicacion",
             table: "Cubiertas",
             type: "int",
             nullable: false,
             defaultValue: 0);

         migrationBuilder.CreateIndex(
             name: "IX_Proyectos_IdUbicacion",
             table: "Proyectos",
             column: "IdUbicacion",
             unique: true);

         migrationBuilder.CreateIndex(
             name: "IX_Cubiertas_UbicacionIdUbicacion",
             table: "Cubiertas",
             column: "UbicacionIdUbicacion");

         migrationBuilder.AddForeignKey(
             name: "FK_Cubiertas_Ubicaciones_UbicacionIdUbicacion",
             table: "Cubiertas",
             column: "UbicacionIdUbicacion",
             principalTable: "Ubicaciones",
             principalColumn: "IdUbicacion",
             onDelete: ReferentialAction.Cascade);

         migrationBuilder.AddForeignKey(
             name: "FK_Proyectos_Ubicaciones_IdUbicacion",
             table: "Proyectos",
             column: "IdUbicacion",
             principalTable: "Ubicaciones",
             principalColumn: "IdUbicacion",
             onDelete: ReferentialAction.Cascade);
      }
   }
}
