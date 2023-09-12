USE [Proyectos]
GO

/****** Objeto: Table [dbo].[Inversores] Fecha del script: 21/08/2023 17:39:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Inversores];


GO
CREATE TABLE [dbo].[Inversores] (
    [IdInversor]         INT           IDENTITY (1, 1) NOT NULL,
    [Modelo]             VARCHAR (100) NOT NULL,
    [Fabricante]         VARCHAR (100) NOT NULL,
    [PotenciaNominal]    FLOAT (53)    NOT NULL,
    [VO]                 INT           NOT NULL,
    [IO]                 FLOAT (53)    NOT NULL,
    [Vmin]               INT           NOT NULL,
    [Vmax]               INT           NOT NULL,
    [CorrienteMaxString] FLOAT (53)    NOT NULL,
    [VminMPPT]           INT           NULL,
    [VmaxMPPT]           INT           NULL,
    [IntensidadMaxMPPT]  FLOAT (53)    NULL,
    [Rendimiento]        FLOAT (53)    NULL
);


