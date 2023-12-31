USE [master]
GO
/****** Object:  Database [Proyectos]    Script Date: 14/09/2023 14:12:10 ******/
CREATE DATABASE [Proyectos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Proyectos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\Proyectos.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Proyectos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER01\MSSQL\DATA\Proyectos_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Proyectos] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Proyectos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Proyectos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Proyectos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Proyectos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Proyectos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Proyectos] SET ARITHABORT OFF 
GO
ALTER DATABASE [Proyectos] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Proyectos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Proyectos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Proyectos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Proyectos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Proyectos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Proyectos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Proyectos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Proyectos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Proyectos] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Proyectos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Proyectos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Proyectos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Proyectos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Proyectos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Proyectos] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Proyectos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Proyectos] SET RECOVERY FULL 
GO
ALTER DATABASE [Proyectos] SET  MULTI_USER 
GO
ALTER DATABASE [Proyectos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Proyectos] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Proyectos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Proyectos] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Proyectos] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Proyectos] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Proyectos', N'ON'
GO
ALTER DATABASE [Proyectos] SET QUERY_STORE = ON
GO
ALTER DATABASE [Proyectos] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Proyectos]
GO

/****** Object:  Table [dbo].[Cadenas]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cadenas](
	[IdCadena] [int] IDENTITY(1,1) NOT NULL,
	[MinModulos] [int] NOT NULL,
	[MaxModulos] [int] NOT NULL,
	[NumModulos] [int] NOT NULL,
	[NumCadenas] [int] NOT NULL,
	[NumInversores] [int] NOT NULL,
	[PotenciaPico] [float] NOT NULL,
	[PotenciaNominal] [float] NOT NULL,
	[PotenciaString] [float] NOT NULL,
	[TensionString] [float] NOT NULL,
	[IdInversor] [int] NOT NULL,
	[IdModulo] [int] NOT NULL,
	[IdInstalacion] [int] NULL,
 CONSTRAINT [PK_Cadenas] PRIMARY KEY CLUSTERED 
(
	[IdCadena] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Dni] [varchar](15) NOT NULL,
	[Telefono] [varchar](15) NOT NULL,
	[Email] [varchar](100) NULL,
	[Observaciones] [varchar](300) NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cubiertas]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cubiertas](
	[IdCubierta] [int] IDENTITY(1,1) NOT NULL,
	[MedidasColectivas] [varchar](50) NOT NULL,
	[Accesibilidad] [varchar](50) NOT NULL,
	[Material] [varchar](100) NOT NULL,
	[IdInstalacion] [int] NULL,
 CONSTRAINT [PK_Cubiertas] PRIMARY KEY CLUSTERED 
(
	[IdCubierta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instalaciones]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instalaciones](
	[IdInstalacion] [int] IDENTITY(1,1) NOT NULL,
	[Inclinacion] [float] NOT NULL,
	[Azimut] [varchar](50) NOT NULL,
	[TotalPico] [float] NOT NULL,
	[TotalNominal] [float] NOT NULL,
	[Tipo] [varchar](50) NOT NULL,
	[CoordXConexion] [float] NOT NULL,
	[CoordYConexion] [float] NOT NULL,
	[Fusible] [varchar](50) NOT NULL,
	[IDiferencial] [varchar](50) NOT NULL,
	[IAutomatico] [varchar](50) NOT NULL,
	[Estructura] [varchar](100) NOT NULL,
	[Vatimetro] [varchar](250) NOT NULL,
	[TotalInversores] [int] NULL,
	[TotalModulos] [int] NOT NULL,
	[TotalCadenas] [int] NULL,
	[Definicion] [varchar](300) NOT NULL,
 CONSTRAINT [PK_Instalaciones] PRIMARY KEY CLUSTERED 
(
	[IdInstalacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inversores]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inversores](
	[IdInversor] [int] IDENTITY(1,1) NOT NULL,
	[Fabricante] [varchar](100) NOT NULL,
	[Modelo] [varchar](100) NOT NULL,
	[PotenciaNominal] [float] NOT NULL,
	[VO] [int] NOT NULL,
	[IO] [float] NOT NULL,
	[Vmin] [int] NOT NULL,
	[Vmax] [int] NOT NULL,
	[CorrienteMaxString] [float] NOT NULL,
	[VminMPPT] [int] NULL,
	[VmaxMPPT] [int] NULL,
	[IntensidadMaxMPPT] [float] NULL,
	[Rendimiento] [float] NULL,
 CONSTRAINT [PK_Inversores] PRIMARY KEY CLUSTERED 
(
	[IdInversor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lugares]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lugares](
	[IdLugarPRL] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Tipo] [nvarchar](max) NOT NULL,
	[Direccion] [varchar](100) NOT NULL,
	[Cp] [int] NOT NULL,
	[Municipio] [varchar](100) NOT NULL,
	[Provincia] [varchar](50) NOT NULL,
	[Telefono] [varchar](30) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[NIMA] [varchar](100) NULL,
	[Autorizacion] [varchar](100) NULL,
	[Latitud] [float] NOT NULL,
	[Longitud] [float] NOT NULL,
	[RutaImg] [varchar](100) NULL,
 CONSTRAINT [PK_Lugares] PRIMARY KEY CLUSTERED 
(
	[IdLugarPRL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LugaresConProyectos]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LugaresConProyectos](
	[LugaresPRLIdLugarPRL] [int] NOT NULL,
	[ProyectosIdProyecto] [int] NOT NULL,
 CONSTRAINT [PK_LugaresConProyectos] PRIMARY KEY CLUSTERED 
(
	[LugaresPRLIdLugarPRL] ASC,
	[ProyectosIdProyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulos]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulos](
	[IdModulo] [int] IDENTITY(1,1) NOT NULL,
	[Fabricante] [varchar](100) NOT NULL,
	[Modelo] [varchar](100) NOT NULL,
	[Potencia] [float] NOT NULL,
	[Vmp] [float] NOT NULL,
	[Imp] [float] NOT NULL,
	[Isc] [float] NOT NULL,
	[Vca] [float] NOT NULL,
	[Eficiencia] [float] NULL,
	[Dimensiones] [varchar](25) NULL,
	[Peso] [float] NULL,
	[NumCelulas] [int] NULL,
	[Tipo] [varchar](20) NULL,
	[TaTONC] [varchar](10) NULL,
	[SalidaPotencia] [float] NULL,
	[TensionVacio] [float] NULL,
	[Tolerancia] [float] NULL,
 CONSTRAINT [PK_Modulos] PRIMARY KEY CLUSTERED 
(
	[IdModulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proyectos]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proyectos](
	[IdProyecto] [int] IDENTITY(1,1) NOT NULL,
	[Referencia] [varchar](100) NOT NULL,
	[Version] [float] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Presupuesto] [float] NULL,
	[PresupuestoSyS] [float] NULL,
	[PlazoEjecucion] [date] NULL,
	[IdCliente] [int] NOT NULL,
	[IdInstalacion] [int] NOT NULL,
 CONSTRAINT [PK_Proyectos] PRIMARY KEY CLUSTERED 
(
	[IdProyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ubicaciones]    Script Date: 14/09/2023 14:12:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ubicaciones](
	[IdUbicacion] [int] IDENTITY(1,1) NOT NULL,
	[RefCatastral] [varchar](100) NOT NULL,
	[Cp] [int] NOT NULL,
	[Municipio] [varchar](100) NOT NULL,
	[Provincia] [varchar](50) NOT NULL,
	[Superficie] [float] NOT NULL,
	[CoordXUTM] [float] NULL,
	[CoordYUTM] [float] NULL,
	[Latitud] [float] NOT NULL,
	[Longitud] [float] NOT NULL,
	[IdCliente] [int] NOT NULL,
	[Cups] [varchar](100) NOT NULL,
	[Bloque] [varchar](15) NOT NULL,
	[Calle] [varchar](250) NOT NULL,
	[Cif] [varchar](50) NOT NULL,
	[Empresa] [varchar](100) NOT NULL,
	[Escalera] [varchar](20) NOT NULL,
	[Numero] [varchar](15) NOT NULL,
	[Piso] [varchar](20) NOT NULL,
	[Portal] [varchar](15) NOT NULL,
	[Puerta] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Ubicaciones] PRIMARY KEY CLUSTERED 
(
	[IdUbicacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cubiertas_InstalacionIdInstalacion]    Script Date: 14/09/2023 14:12:11 ******/
CREATE NONCLUSTERED INDEX [IX_Cubiertas_InstalacionIdInstalacion] ON [dbo].[Cubiertas]
(
	[IdInstalacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LugaresConProyectos_IdProyecto]    Script Date: 14/09/2023 14:12:11 ******/
CREATE NONCLUSTERED INDEX [IX_LugaresConProyectos_IdProyecto] ON [dbo].[LugaresConProyectos]
(
	[ProyectosIdProyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Proyectos_IdCliente]    Script Date: 14/09/2023 14:12:11 ******/
CREATE NONCLUSTERED INDEX [IX_Proyectos_IdCliente] ON [dbo].[Proyectos]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Proyectos_IdInstalacion]    Script Date: 14/09/2023 14:12:11 ******/
CREATE NONCLUSTERED INDEX [IX_Proyectos_IdInstalacion] ON [dbo].[Proyectos]
(
	[IdInstalacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ubicaciones_IdCliente]    Script Date: 14/09/2023 14:12:11 ******/
CREATE NONCLUSTERED INDEX [IX_Ubicaciones_IdCliente] ON [dbo].[Ubicaciones]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Instalaciones] ADD  DEFAULT ('') FOR [Definicion]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Bloque]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Calle]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Cif]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Empresa]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Escalera]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Numero]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Piso]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Portal]
GO
ALTER TABLE [dbo].[Ubicaciones] ADD  DEFAULT ('') FOR [Puerta]
GO
ALTER TABLE [dbo].[Cadenas]  WITH CHECK ADD  CONSTRAINT [FK_Cadenas_Instalaciones_IdInstalacion] FOREIGN KEY([IdInstalacion])
REFERENCES [dbo].[Instalaciones] ([IdInstalacion])
GO
ALTER TABLE [dbo].[Cadenas] CHECK CONSTRAINT [FK_Cadenas_Instalaciones_IdInstalacion]
GO
ALTER TABLE [dbo].[Cubiertas]  WITH CHECK ADD  CONSTRAINT [FK_Cubiertas_Instalaciones_InstalacionIdInstalacion] FOREIGN KEY([IdInstalacion])
REFERENCES [dbo].[Instalaciones] ([IdInstalacion])
GO
ALTER TABLE [dbo].[Cubiertas] CHECK CONSTRAINT [FK_Cubiertas_Instalaciones_InstalacionIdInstalacion]
GO
ALTER TABLE [dbo].[LugaresConProyectos]  WITH CHECK ADD  CONSTRAINT [FK_LugaresConProyectos_IdLugarPRL] FOREIGN KEY([LugaresPRLIdLugarPRL])
REFERENCES [dbo].[Lugares] ([IdLugarPRL])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LugaresConProyectos] CHECK CONSTRAINT [FK_LugaresConProyectos_IdLugarPRL]
GO
ALTER TABLE [dbo].[LugaresConProyectos]  WITH CHECK ADD  CONSTRAINT [FK_LugaresConProyectos_IdProyecto] FOREIGN KEY([ProyectosIdProyecto])
REFERENCES [dbo].[Proyectos] ([IdProyecto])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LugaresConProyectos] CHECK CONSTRAINT [FK_LugaresConProyectos_IdProyecto]
GO
ALTER TABLE [dbo].[Proyectos]  WITH CHECK ADD  CONSTRAINT [FK_Proyectos_Clientes_IdCliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Proyectos] CHECK CONSTRAINT [FK_Proyectos_Clientes_IdCliente]
GO
ALTER TABLE [dbo].[Proyectos]  WITH CHECK ADD  CONSTRAINT [FK_Proyectos_Instalaciones_IdInstalacion] FOREIGN KEY([IdInstalacion])
REFERENCES [dbo].[Instalaciones] ([IdInstalacion])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Proyectos] CHECK CONSTRAINT [FK_Proyectos_Instalaciones_IdInstalacion]
GO
ALTER TABLE [dbo].[Ubicaciones]  WITH CHECK ADD  CONSTRAINT [FK_Ubicaciones_Clientes_IdCliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ubicaciones] CHECK CONSTRAINT [FK_Ubicaciones_Clientes_IdCliente]
GO
USE [master]
GO
ALTER DATABASE [Proyectos] SET  READ_WRITE 
GO
