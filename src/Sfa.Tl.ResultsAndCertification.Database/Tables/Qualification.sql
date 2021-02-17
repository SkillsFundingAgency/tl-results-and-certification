CREATE TABLE [dbo].[Qualification]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[QualificationTypeId] INT NOT NULL,
	[TlLookupId] INT NOT NULL,
	[Code] NVARCHAR(50),
	[Title] NVARCHAR(255),	
	[IsSendQualification] BIT NOT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL DEFAULT 1,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL,
    [ModifiedOn] DATETIME2 NULL,
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_Qualification] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Qualification_QualificationType] FOREIGN KEY ([QualificationTypeId]) REFERENCES [QualificationType]([Id]),
	CONSTRAINT [FK_Qualification_TlLookup] FOREIGN KEY ([TlLookupId]) REFERENCES [TlLookup]([Id]),
	CONSTRAINT Unique_Qualification_Code UNIQUE ([Code])
)