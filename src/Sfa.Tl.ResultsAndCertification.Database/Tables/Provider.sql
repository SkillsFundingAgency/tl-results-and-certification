CREATE TABLE [dbo].[Provider]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [UkPrn] BIGINT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [DisplayName] NVARCHAR(100) NULL, 
    [IsTLevelProvider] BIT NOT NULL, 
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_Provider] PRIMARY KEY ([Id])
)
