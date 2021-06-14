CREATE TABLE [dbo].[TlProviderAddress]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [TlProviderId] INT NOT NULL, 
    [DepartmentName] NVARCHAR(100) NULL, 
    [OrganisationName] NVARCHAR(100) NULL,
    [AddressLine1] NVARCHAR(256) NOT NULL,
    [AddressLine2] NVARCHAR(256) NULL,
    [Town] NVARCHAR(100) NOT NULL,
    [Postcode] NVARCHAR(8) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlProviderAddress] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TlProviderAddress_TlProvider] FOREIGN KEY ([TlProviderId]) REFERENCES [TlProvider]([Id])
)