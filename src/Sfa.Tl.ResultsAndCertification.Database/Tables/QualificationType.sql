CREATE TABLE [dbo].[QualificationType]
(
	[Id] INT IDENTITY(1,1) NOT NULL,	
	[Name] NVARCHAR(255),	
	[IsActive] BIT NOT NULL DEFAULT 1,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_QualificationType] PRIMARY KEY ([Id])
)