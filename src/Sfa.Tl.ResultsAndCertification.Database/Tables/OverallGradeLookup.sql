CREATE TABLE [dbo].[OverallGradeLookup]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TlPathwayId] INT NOT NULL,
	[TlLookupCoreGradeId] INT NOT NULL,
	[TlLookupSpecialismGradeId] INT NOT NULL,
	[TlLookupOverallGradeId] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_OverallGradeLookup] PRIMARY KEY ([Id]),	
	CONSTRAINT [FK_OverallGradeLookup_TlPathway] FOREIGN KEY (TlPathwayId) REFERENCES [TlPathway]([Id]),
	CONSTRAINT [FK_OverallGradeLookup_TlLookupCoreGrade] FOREIGN KEY ([TlLookupCoreGradeId]) REFERENCES [TlLookup]([Id]),
	CONSTRAINT [FK_OverallGradeLookup_TlLookupSpecialismGrade] FOREIGN KEY ([TlLookupSpecialismGradeId]) REFERENCES [TlLookup]([Id]),
	CONSTRAINT [FK_OverallGradeLookup_TlLookupOverallGrade] FOREIGN KEY ([TlLookupOverallGradeId]) REFERENCES [TlLookup]([Id]),
)