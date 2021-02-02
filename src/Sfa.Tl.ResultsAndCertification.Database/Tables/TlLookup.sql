CREATE TABLE [dbo].[TlLookup]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
    [Category] NVARCHAR(50) NOT NULL, 
    [Code] NVARCHAR(10) NOT NULL, 
    [Value] NVARCHAR(50) NOT NULL,
    [SortOrder] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_TlLookup] PRIMARY KEY ([Id]),
    CONSTRAINT Unique_Code UNIQUE ([Code])
)