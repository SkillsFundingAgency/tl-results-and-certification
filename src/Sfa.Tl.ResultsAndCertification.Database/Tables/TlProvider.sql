CREATE TABLE [dbo].[TlProvider]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[TlPathwayId] INT NOT NULL,
    [UkPrn] BIGINT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [DisplayName] NVARCHAR(100) NULL, 	
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlProvider] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_TlProvider_TlPathway] FOREIGN KEY ([TlPathwayId]) REFERENCES [TlPathway]([Id])
)
