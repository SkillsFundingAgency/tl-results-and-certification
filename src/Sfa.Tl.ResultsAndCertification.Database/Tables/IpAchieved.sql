CREATE TABLE [dbo].[IpAchieved]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[IndustryPlacementId] INT NOT NULL,
	[IpLookupId] INT NOT NULL,
	[IsActive] BIT,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_IpAchieved] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_IpAchieved_IndustryPlacement] FOREIGN KEY ([IndustryPlacementId]) REFERENCES [IndustryPlacement]([Id]),
	CONSTRAINT [FK_IpAchieved_IpLookup] FOREIGN KEY ([IpLookupId]) REFERENCES [IpLookup]([Id])
)