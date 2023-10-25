using Microsoft.EntityFrameworkCore.Migrations;


namespace backend_API.Migrations
{
   /// <inheritdoc />
   public partial class InitDB : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         _ = migrationBuilder.CreateTable(
             name: "Clientes",
             columns: table => new
             {
                IdCliente = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                Dni = table.Column<string>(type: "varchar(15)", nullable: false),
                Telefono = table.Column<string>(type: "varchar(15)", nullable: false),
                Email = table.Column<string>(type: "varchar(100)", nullable: true),
                Observaciones = table.Column<string>(type: "varchar(300)", nullable: true)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Clientes", x => x.IdCliente);
             });

         _ = migrationBuilder.CreateTable(
             name: "Errores",
             columns: table => new
             {
                IdError = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Mensaje = table.Column<string>(type: "varchar(300)", nullable: false),
                StackTrace = table.Column<string>(type: "varchar(300)", nullable: false),
                FechaRegistro = table.Column<DateTime>(type: "date", nullable: false)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Errores", x => x.IdError);
             });

         _ = migrationBuilder.CreateTable(
             name: "Instalaciones",
             columns: table => new
             {
                IdInstalacion = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Inclinacion = table.Column<double>(type: "float", nullable: false),
                Azimut = table.Column<string>(type: "varchar(50)", nullable: false),
                Tipo = table.Column<string>(type: "varchar(50)", nullable: false),
                CoordXConexion = table.Column<double>(type: "float", nullable: false),
                CoordYConexion = table.Column<double>(type: "float", nullable: false),
                PotenciaContratada = table.Column<double>(type: "float", nullable: true),
                ConsumoEstimado = table.Column<double>(type: "float", nullable: true),
                GeneracionAnual = table.Column<double>(type: "float", nullable: true),
                Fusible = table.Column<string>(type: "varchar(50)", nullable: false),
                IDiferencial = table.Column<string>(type: "varchar(50)", nullable: false),
                IAutomatico = table.Column<string>(type: "varchar(50)", nullable: false),
                Estructura = table.Column<string>(type: "varchar(100)", nullable: false),
                Vatimetro = table.Column<string>(type: "varchar(250)", nullable: false),
                SeccionFase = table.Column<string>(type: "varchar(20)", nullable: false),
                TotalPico = table.Column<double>(type: "float", nullable: false),
                TotalNominal = table.Column<double>(type: "float", nullable: false),
                TotalInversores = table.Column<int>(type: "int", nullable: false),
                TotalModulos = table.Column<int>(type: "int", nullable: false),
                TotalCadenas = table.Column<int>(type: "int", nullable: false),
                DirectorObra = table.Column<string>(type: "varchar(100)", nullable: false),
                Titulacion = table.Column<string>(type: "varchar(100)", nullable: false),
                ColeOficial = table.Column<string>(type: "varchar(100)", nullable: false),
                NumColegiado = table.Column<string>(type: "varchar(100)", nullable: false)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Instalaciones", x => x.IdInstalacion);
             });

         _ = migrationBuilder.CreateTable(
             name: "Inversores",
             columns: table => new
             {
                IdInversor = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Modelo = table.Column<string>(type: "varchar(100)", nullable: false),
                Fabricante = table.Column<string>(type: "varchar(100)", nullable: false),
                PotenciaNominal = table.Column<double>(type: "float", nullable: false),
                VO = table.Column<int>(type: "int", nullable: false),
                IO = table.Column<double>(type: "float", nullable: false),
                Vmin = table.Column<int>(type: "int", nullable: false),
                Vmax = table.Column<int>(type: "int", nullable: false),
                CorrienteMaxString = table.Column<double>(type: "float", nullable: false),
                VminMPPT = table.Column<int>(type: "int", nullable: true),
                VmaxMPPT = table.Column<int>(type: "int", nullable: true),
                IntensidadMaxMPPT = table.Column<double>(type: "float", nullable: true),
                Rendimiento = table.Column<double>(type: "float", nullable: true)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Inversores", x => x.IdInversor);
             });

         _ = migrationBuilder.CreateTable(
             name: "Lugares",
             columns: table => new
             {
                IdLugarPRL = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Calle = table.Column<string>(type: "varchar(100)", nullable: false),
                Numero = table.Column<string>(type: "varchar(10)", nullable: false),
                Cp = table.Column<int>(type: "int", nullable: false),
                Municipio = table.Column<string>(type: "varchar(100)", nullable: false),
                Provincia = table.Column<string>(type: "varchar(50)", nullable: false),
                Telefono = table.Column<string>(type: "varchar(30)", nullable: false),
                Email = table.Column<string>(type: "varchar(100)", nullable: false),
                NIMA = table.Column<string>(type: "varchar(100)", nullable: true),
                Autorizacion = table.Column<string>(type: "varchar(100)", nullable: true),
                Latitud = table.Column<double>(type: "float", nullable: false),
                Longitud = table.Column<double>(type: "float", nullable: false),
                RutaImg = table.Column<string>(type: "varchar(100)", nullable: true)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Lugares", x => x.IdLugarPRL);
             });

         _ = migrationBuilder.CreateTable(
             name: "Modulos",
             columns: table => new
             {
                IdModulo = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Modelo = table.Column<string>(type: "varchar(100)", nullable: false),
                Fabricante = table.Column<string>(type: "varchar(100)", nullable: false),
                Potencia = table.Column<double>(type: "float", nullable: false),
                Vmp = table.Column<double>(type: "float", nullable: false),
                Imp = table.Column<double>(type: "float", nullable: false),
                Isc = table.Column<double>(type: "float", nullable: false),
                Vca = table.Column<double>(type: "float", nullable: false),
                Eficiencia = table.Column<double>(type: "float", nullable: true),
                Dimensiones = table.Column<string>(type: "varchar(25)", nullable: true),
                Peso = table.Column<double>(type: "float", nullable: true),
                NumCelulas = table.Column<int>(type: "int", nullable: true),
                Tipo = table.Column<string>(type: "varchar(15)", nullable: true),
                TaTONC = table.Column<string>(type: "varchar(15)", nullable: true),
                SalidaPotencia = table.Column<double>(type: "float", nullable: true),
                TensionVacio = table.Column<double>(type: "float", nullable: true),
                Tolerancia = table.Column<double>(type: "float", nullable: true)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Modulos", x => x.IdModulo);
             });

         _ = migrationBuilder.CreateTable(
             name: "Ubicaciones",
             columns: table => new
             {
                IdUbicacion = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                RefCatastral = table.Column<string>(type: "varchar(100)", nullable: false),
                Antiguedad = table.Column<string>(type: "varchar(20)", nullable: false),
                Via = table.Column<string>(type: "varchar(50)", nullable: false),
                Calle = table.Column<string>(type: "varchar(250)", nullable: false),
                Numero = table.Column<string>(type: "varchar(10)", nullable: false),
                Km = table.Column<string>(type: "varchar(10)", nullable: false),
                Bloque = table.Column<string>(type: "varchar(15)", nullable: false),
                Portal = table.Column<string>(type: "varchar(10)", nullable: false),
                Escalera = table.Column<string>(type: "varchar(10)", nullable: false),
                Piso = table.Column<string>(type: "varchar(10)", nullable: false),
                Puerta = table.Column<string>(type: "varchar(10)", nullable: false),
                Cp = table.Column<int>(type: "int", nullable: false),
                Municipio = table.Column<string>(type: "varchar(100)", nullable: false),
                Provincia = table.Column<string>(type: "varchar(100)", nullable: false),
                CCAA = table.Column<string>(type: "varchar(100)", nullable: false),
                Superficie = table.Column<double>(type: "float", nullable: false),
                CoordXUTM = table.Column<double>(type: "float", nullable: true),
                CoordYUTM = table.Column<double>(type: "float", nullable: true),
                Latitud = table.Column<double>(type: "float", nullable: false),
                Longitud = table.Column<double>(type: "float", nullable: false),
                Cups = table.Column<string>(type: "varchar(100)", nullable: false),
                Empresa = table.Column<string>(type: "varchar(100)", nullable: false),
                Cif = table.Column<string>(type: "varchar(50)", nullable: false),
                Cau = table.Column<string>(type: "varchar(50)", nullable: false),
                IdCliente = table.Column<int>(type: "int", nullable: false)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Ubicaciones", x => x.IdUbicacion);
                _ = table.ForeignKey(
                       name: "FK_Ubicaciones_Clientes_IdCliente",
                       column: x => x.IdCliente,
                       principalTable: "Clientes",
                       principalColumn: "IdCliente",
                       onDelete: ReferentialAction.Cascade);
             });

         _ = migrationBuilder.CreateTable(
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
                PotenciaString = table.Column<double>(type: "float", nullable: false),
                TensionString = table.Column<double>(type: "float", nullable: false),
                IdInversor = table.Column<int>(type: "int", nullable: false),
                IdModulo = table.Column<int>(type: "int", nullable: false),
                IdInstalacion = table.Column<int>(type: "int", nullable: true)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Cadenas", x => x.IdCadena);
                _ = table.ForeignKey(
                       name: "FK_Cadenas_Instalaciones_IdInstalacion",
                       column: x => x.IdInstalacion,
                       principalTable: "Instalaciones",
                       principalColumn: "IdInstalacion");
             });

         _ = migrationBuilder.CreateTable(
             name: "Cubiertas",
             columns: table => new
             {
                IdCubierta = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                MedidasColectivas = table.Column<string>(type: "varchar(50)", nullable: false),
                Accesibilidad = table.Column<string>(type: "varchar(50)", nullable: false),
                Material = table.Column<string>(type: "varchar(100)", nullable: false),
                IdInstalacion = table.Column<int>(type: "int", nullable: true)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Cubiertas", x => x.IdCubierta);
                _ = table.ForeignKey(
                       name: "FK_Cubiertas_Instalaciones_IdInstalacion",
                       column: x => x.IdInstalacion,
                       principalTable: "Instalaciones",
                       principalColumn: "IdInstalacion");
             });

         _ = migrationBuilder.CreateTable(
             name: "Proyectos",
             columns: table => new
             {
                IdProyecto = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Referencia = table.Column<string>(type: "varchar(100)", nullable: false),
                Version = table.Column<double>(type: "float", nullable: false),
                Fecha = table.Column<DateTime>(type: "date", nullable: false),
                Presupuesto = table.Column<double>(type: "float", nullable: true),
                PresupuestoSyS = table.Column<double>(type: "float", nullable: true),
                PlazoEjecucion = table.Column<DateTime>(type: "date", nullable: true),
                OCA = table.Column<string>(type: "varchar(100)", nullable: true),
                NumOCA = table.Column<string>(type: "varchar(100)", nullable: true),
                InspeccionOCA = table.Column<string>(type: "varchar(200)", nullable: true),
                IdCliente = table.Column<int>(type: "int", nullable: false),
                IdInstalacion = table.Column<int>(type: "int", nullable: false)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_Proyectos", x => x.IdProyecto);
                _ = table.ForeignKey(
                       name: "FK_Proyectos_Clientes_IdCliente",
                       column: x => x.IdCliente,
                       principalTable: "Clientes",
                       principalColumn: "IdCliente",
                       onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                       name: "FK_Proyectos_Instalaciones_IdInstalacion",
                       column: x => x.IdInstalacion,
                       principalTable: "Instalaciones",
                       principalColumn: "IdInstalacion",
                       onDelete: ReferentialAction.Cascade);
             });

         _ = migrationBuilder.CreateTable(
             name: "LugaresConProyectos",
             columns: table => new
             {
                LugaresPRLIdLugarPRL = table.Column<int>(type: "int", nullable: false),
                ProyectosIdProyecto = table.Column<int>(type: "int", nullable: false)
             },
             constraints: table =>
             {
                _ = table.PrimaryKey("PK_LugaresConProyectos", x => new { x.LugaresPRLIdLugarPRL, x.ProyectosIdProyecto });
                _ = table.ForeignKey(
                       name: "FK_LugaresConProyectos_Lugares_LugaresPRLIdLugarPRL",
                       column: x => x.LugaresPRLIdLugarPRL,
                       principalTable: "Lugares",
                       principalColumn: "IdLugarPRL",
                       onDelete: ReferentialAction.Cascade);
                _ = table.ForeignKey(
                       name: "FK_LugaresConProyectos_Proyectos_ProyectosIdProyecto",
                       column: x => x.ProyectosIdProyecto,
                       principalTable: "Proyectos",
                       principalColumn: "IdProyecto",
                       onDelete: ReferentialAction.Cascade);
             });

         _ = migrationBuilder.CreateIndex(
             name: "IX_Cadenas_IdInstalacion",
             table: "Cadenas",
             column: "IdInstalacion");

         _ = migrationBuilder.CreateIndex(
             name: "IX_Cubiertas_IdInstalacion",
             table: "Cubiertas",
             column: "IdInstalacion");

         _ = migrationBuilder.CreateIndex(
             name: "IX_LugaresConProyectos_ProyectosIdProyecto",
             table: "LugaresConProyectos",
             column: "ProyectosIdProyecto");

         _ = migrationBuilder.CreateIndex(
             name: "IX_Proyectos_IdCliente",
             table: "Proyectos",
             column: "IdCliente");

         _ = migrationBuilder.CreateIndex(
             name: "IX_Proyectos_IdInstalacion",
             table: "Proyectos",
             column: "IdInstalacion");

         _ = migrationBuilder.CreateIndex(
             name: "IX_Ubicaciones_IdCliente",
             table: "Ubicaciones",
             column: "IdCliente");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         _ = migrationBuilder.DropTable(
             name: "Cadenas");

         _ = migrationBuilder.DropTable(
             name: "Cubiertas");

         _ = migrationBuilder.DropTable(
             name: "Errores");

         _ = migrationBuilder.DropTable(
             name: "Inversores");

         _ = migrationBuilder.DropTable(
             name: "LugaresConProyectos");

         _ = migrationBuilder.DropTable(
             name: "Modulos");

         _ = migrationBuilder.DropTable(
             name: "Ubicaciones");

         _ = migrationBuilder.DropTable(
             name: "Lugares");

         _ = migrationBuilder.DropTable(
             name: "Proyectos");

         _ = migrationBuilder.DropTable(
             name: "Clientes");

         _ = migrationBuilder.DropTable(
             name: "Instalaciones");
      }
   }
}
