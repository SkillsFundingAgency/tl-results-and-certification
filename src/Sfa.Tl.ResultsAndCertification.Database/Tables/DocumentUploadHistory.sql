CREATE TABLE [dbo].[DocumentUploadHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TlAwardingOrganisationId] INT NOT NULL,
	[BlobFileName] NVARCHAR(256) NOT NULL,
	[DocumentType] INT NOT NULL,
	[FileType] INT NOT NULL,
	[Status] INT NOT NULL DEFAULT 1,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_DocumentUploadHistory] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_DocumentUploadHistory_TlAwardingOrganisation] FOREIGN KEY ([TlAwardingOrganisationId]) REFERENCES [TlAwardingOrganisation]([Id])
)