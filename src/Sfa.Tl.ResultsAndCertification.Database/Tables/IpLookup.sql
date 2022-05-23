CREATE TABLE [dbo].[IpLookup]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TlLookupId] INT NOT NULL,
	[Name] NVARCHAR(255) NULL,
	[StartDate] DATETIME NOT NULL,	
	[EndDate] DATETIME NULL,	
	[ShowOption] INT NULL,
	[SortOrder] INT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_IpLookup] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_IpLookup_TlLookup] FOREIGN KEY ([TlLookupId]) REFERENCES [TlLookup]([Id])
)