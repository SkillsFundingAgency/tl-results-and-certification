CREATE TABLE [dbo].[QualificationGrade]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[QualificationTypeId] INT NOT NULL,
	[Grade] NVARCHAR(50),
	[IsAllowable] BIT NOT NULL DEFAULT 0,	
	[IsActive] BIT NOT NULL DEFAULT 1,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL,
    [ModifiedOn] DATETIME2 NULL,
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_QualificationGrade] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_QualificationGrade_QualificationType] FOREIGN KEY ([QualificationTypeId]) REFERENCES [QualificationType]([Id])
)