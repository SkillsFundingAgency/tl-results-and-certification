﻿CREATE TABLE [dbo].[TlPathwaySpecialismCombination]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlPathwayId] INT NOT NULL,
	[TlSpecialismId] INT NOT NULL,	
    [GroupId] INT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlPathwaySpecialismCombination] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismCombination_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismCombination_TlSpecialism] FOREIGN KEY ([TlSpecialismId]) REFERENCES [TlSpecialism]([Id])
)