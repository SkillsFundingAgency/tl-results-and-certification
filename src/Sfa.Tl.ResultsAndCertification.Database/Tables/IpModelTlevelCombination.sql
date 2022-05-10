CREATE TABLE [dbo].[IpModelTlevelCombination]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlPathwayId] INT NOT NULL,
	[IpLookupId] INT NOT NULL,
	[IsActive] BIT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_IpModelTlevelCombination] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_IpModelTlevelCombination_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_IpModelTlevelCombination_IpLookup] FOREIGN KEY ([IpLookupId]) REFERENCES [IpLookup]([Id])
)