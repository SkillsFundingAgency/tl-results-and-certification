CREATE TABLE [dbo].[TlProvider]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [UkPrn] BIGINT NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL, 
    [DisplayName] NVARCHAR(256) NULL, 	
    [IsActive] BIT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlProvider] PRIMARY KEY ([Id]),
)
