CREATE TABLE [dbo].[AcademicYear]
(
	[Id] INT IDENTITY(1,1) NOT NULL,	
	[Name] NVARCHAR(50) NOT NULL,
	[Year] INT NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_AcademicYear] PRIMARY KEY ([Id])
)