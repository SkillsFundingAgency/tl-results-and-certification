CREATE TABLE [dbo].[PrintBatchItem]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[BatchId] INT NOT NULL,
	[TlProviderAddressId] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_PrintBatchItem] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_PrintBatchItem_Batch] FOREIGN KEY ([BatchId]) REFERENCES [Batch]([Id]),
	CONSTRAINT [FK_PrintBatchItem_TlProviderAddress] FOREIGN KEY ([TlProviderAddressId]) REFERENCES [TlProviderAddress]([Id]),
	INDEX IX_PrintBatchItem_BatchId NONCLUSTERED ([BatchId])
)