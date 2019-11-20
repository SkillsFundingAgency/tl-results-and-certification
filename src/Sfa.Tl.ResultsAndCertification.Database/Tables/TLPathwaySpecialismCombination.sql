CREATE TABLE [dbo].[TlPathwaySpecialismCombination]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[PathwayId] INT NOT NULL,
	[SpecialismId] INT NOT NULL,	
    [Group] NVARCHAR(50) NOT NULL,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlPathwaySpecialismCombination] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismCombination_TlPathway] FOREIGN KEY ([PathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_TlPathwaySpecialismCombination_TlSpecialism] FOREIGN KEY ([SpecialismId]) REFERENCES [TlSpecialism]([Id])
)
