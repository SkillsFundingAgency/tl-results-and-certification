CREATE TABLE [dbo].[TlPathway]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlRouteId] INT NOT NULL,
	[LarId] NVARCHAR(8) NOT NULL,
	[TlevelTitle] NVARCHAR(255) NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,	
	[IsActive] BIT NOT NULL DEFAULT(1),
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlPathway] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlPathway_TlRoute] FOREIGN KEY ([TlRouteId]) REFERENCES [TlRoute]([Id])
)
