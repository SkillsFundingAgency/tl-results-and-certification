CREATE TABLE [dbo].[QualificationAchieved]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[TqRegistrationProfileId] INT NOT NULL,
	[QualificationId] INT NOT NULL,
	[QualificationGradeId] INT NOT NULL,	
	[IsAchieved] BIT NOT NULL DEFAULT 0,	
	[CreatedOn] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NULL, 
    [ModifiedOn] DATETIME2 NULL, 
    [ModifiedBy] NVARCHAR(50) NULL,
	CONSTRAINT [PK_QualificationAchieved] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_QualificationAchieved_TqRegistrationProfile] FOREIGN KEY ([TqRegistrationProfileId]) REFERENCES [TqRegistrationProfile]([Id]),
	CONSTRAINT [FK_QualificationAchieved_Qualification] FOREIGN KEY ([QualificationId]) REFERENCES [Qualification]([Id]),
	CONSTRAINT [FK_QualificationAchieved_QualificationGrade] FOREIGN KEY ([QualificationGradeId]) REFERENCES [QualificationGrade]([Id]),
	INDEX IX_QualificationAchieved_TqRegistrationProfileId NONCLUSTERED (TqRegistrationProfileId)
)