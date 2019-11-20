CREATE TABLE [dbo].[TqAwardingOrganisation]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[UkAon] NVARCHAR(100) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL, 
    [DisplayName] NVARCHAR(100) NOT NULL, 
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TqAwardingOrganisation] PRIMARY KEY ([Id])
)
