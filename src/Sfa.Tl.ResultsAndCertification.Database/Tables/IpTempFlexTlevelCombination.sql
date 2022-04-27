CREATE TABLE [dbo].[IpTempFlexTlevelCombination]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlPathwayId] INT NOT NULL,
	[IpLookupId] INT NOT NULL,
	[IsActive] BIT,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_IpTempFlexTlevelCombination] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_IpTempFlexTlevelCombination_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_IpTempFlexTlevelCombination_IpLookup] FOREIGN KEY ([IpLookupId]) REFERENCES [IpLookup]([Id])
)