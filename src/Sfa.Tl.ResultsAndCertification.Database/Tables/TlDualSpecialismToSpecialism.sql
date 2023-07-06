CREATE TABLE [dbo].[TlDualSpecialismToSpecialism]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlDualSpecialismId] INT NOT NULL,
	[TlSpecialismId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlDualSpecialismToSpecialism] PRIMARY KEY ([Id]),
	CONSTRAINT TlDualSpecialismToSpecialism_UK UNIQUE (TlDualSpecialismId, TlSpecialismId),
	CONSTRAINT [FK_TlDualSpecialismToSpecialism_DualSpecialismId] FOREIGN KEY(TlDualSpecialismId) REFERENCES [TlDualSpecialism]([Id]),
	CONSTRAINT [FK_TlDualSpecialismToSpecialism_SpecialismId] FOREIGN KEY(TlSpecialismId) REFERENCES [TlSpecialism]([Id])
)
