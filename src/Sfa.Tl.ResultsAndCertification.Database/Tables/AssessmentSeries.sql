CREATE TABLE [dbo].[AssessmentSeries]
(
	[Id] INT IDENTITY(1,1) NOT NULL,	
	[Name] NVARCHAR(255),
	[Description] NVARCHAR(1000),
	[Year] INT NOT NULL,
	[EndDate] DATETIME NULL,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_AssessmentSeries] PRIMARY KEY ([Id])
)