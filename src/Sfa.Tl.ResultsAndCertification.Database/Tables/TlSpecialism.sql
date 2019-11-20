CREATE TABLE [dbo].[TlSpecialism]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[PathwayId] INT NOT NULL,
	[LarId] NVARCHAR(8) NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlSpecialism] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlSpecialism_TlPathway] FOREIGN KEY ([PathwayId]) REFERENCES [TlPathway]([Id])
)
