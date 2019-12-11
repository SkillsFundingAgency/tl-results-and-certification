CREATE TABLE [dbo].[TlPathway]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[RouteId] INT NOT NULL,
	[LarId] NVARCHAR(8) NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlPathway] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlPathway_TlRoute] FOREIGN KEY ([RouteId]) REFERENCES [TlRoute]([Id])
)
