USE [Proyectos]
GO

/****** Objeto: Table [dbo].[Modulos] Fecha del script: 21/08/2023 17:34:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Modulos];


GO
CREATE TABLE [dbo].[Modulos] (
    [IdModulo]       INT           IDENTITY (1, 1) NOT NULL,
    [Modelo]         VARCHAR (100) NOT NULL,
    [Fabricante]     VARCHAR (100) NOT NULL,
    [Potencia]       FLOAT (53)    NOT NULL,
    [Vmp]            FLOAT (53)    NOT NULL,
    [Imp]            FLOAT (53)    NOT NULL,
    [Isc]            FLOAT (53)    NOT NULL,
    [Vca]            FLOAT (53)    NOT NULL,
    [Eficiencia]     FLOAT (53)    NULL,
    [Dimensiones]    VARCHAR (25)  NULL,
    [Peso]           FLOAT (53)    NULL,
    [NumCelulas]     INT           NULL,
    [Tipo]           VARCHAR (15)  NULL,
    [TaTONC]         VARCHAR (15)  NULL,
    [SalidaPotencia] FLOAT (53)    NULL,
    [TensionVacio]   FLOAT (53)    NULL,
    [Tolerancia]     FLOAT (53)    NULL
);


