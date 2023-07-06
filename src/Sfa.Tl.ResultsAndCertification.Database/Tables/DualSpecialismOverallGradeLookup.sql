CREATE TABLE [dbo].[DualSpecialismOverallGradeLookup]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[FirstTlLookupSpecialismGradeId] INT NOT NULL,
	[SecondTlLookupSpecialismGradeId] INT NOT NULL,
	[TlLookupOverallSpecialismGradeId] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
    CONSTRAINT [PK_DualSpecialismOverallGradeLookup] PRIMARY KEY ([Id]),	
	CONSTRAINT [FK_DualSpecialismOverallGradeLookup_FirstTlLookupSpecialismGradeId] FOREIGN KEY ([FirstTlLookupSpecialismGradeId]) REFERENCES [TlLookup]([Id]),
	CONSTRAINT [FK_DualSpecialismOverallGradeLookup_SecondTlLookupSpecialismGradeId] FOREIGN KEY ([SecondTlLookupSpecialismGradeId]) REFERENCES [TlLookup]([Id]),
	CONSTRAINT [FK_DualSpecialismOverallGradeLookup_TlLookupOverallSpecialismGradeId] FOREIGN KEY ([TlLookupOverallSpecialismGradeId]) REFERENCES [TlLookup]([Id])
)